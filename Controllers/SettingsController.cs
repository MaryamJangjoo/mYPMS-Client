using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using mYPMS.Data;
using mYPMS.Models;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace mYPMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly mYPMSContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        public SettingsController(
            mYPMSContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager
            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index() => View();
        public IActionResult Users()
        {
            //await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name), "Admin");
            List<UserWithRoles> users = new();
            _context.Users.ToList().ForEach(u =>
            {
                var roleId = _context.UserRoles.Where(e => e.UserId == u.Id).FirstOrDefault();
                var roleName = roleId == null ? "بدون نقش" : _roleManager.FindByIdAsync(roleId.RoleId).Result.Name;
                users.Add(new UserWithRoles
                {
                    User = u,
                    RolesName = roleName
                });
            });
            var model = new UsersAndRoles { Users = users, Roles = _roleManager.Roles.ToList() };
            return View(model);
        }
        [HttpPost]
        public IActionResult UserDelete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null) return NotFound(id);
            try
            {
                IdentityResult result = _userManager.DeleteAsync(user).Result;
                return result.Succeeded ? Redirect("~/Settings/Users") : Problem(result.Errors.ToJson());

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UserEdit(string username, string? password, string? role)
        {
            IdentityResult? result = null;
            try
            {
                var user = _userManager.FindByNameAsync(username).Result;
                if (user == null)
                {
                    user = new(username);
                    result = _userManager.CreateAsync(user, password).Result;
                }
                else if (password != null)
                {
                    _ = _userManager.RemovePasswordAsync(user).Result;
                    result = _userManager.AddPasswordAsync(user, password).Result;
                }

                if (role != null)
                {
                    var r = _roleManager.RoleExistsAsync(role).Result;
                    _ = r ? _userManager.AddToRoleAsync(user, role).Result.Succeeded : true;
                }
                else
                {
                    var r = _userManager.IsInRoleAsync(user, "Admin").Result;
                    if (r == true) _ = _userManager.RemoveFromRoleAsync(user, "Admin").Result;
                }
                if (result == null) return Redirect("~/Settings/Users");
                return result.Succeeded ? Redirect("~/Settings/Users") : Problem(result.Errors.ToJson());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        public async Task<IActionResult> Parkings() => View(await _context.tParkings.Include(b => b.xTariff).ToListAsync());
        public async Task<IActionResult> Gates() => View(await _context.tGates.Include(p => p.xParking).ToListAsync());
        public async Task<IActionResult> Price() => View(await _context.tTariffs.ToListAsync());
        public IActionResult CardGroup(Guid? groupId)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (groupId.HasValue)
                    return PartialView("_partialCardGroupGrid", _context.tTagList.Include(b => b.xTagGroup).Where(b => b.xTagGroupId == groupId));
                else
                    return PartialView("_partialCardGroupGrid", _context.tTagList.Include(b => b.xTagGroup));
            }
            return View(_context.tTagList.Include(b => b.xTagGroup));
        }
        public JsonResult ParkingExists(Guid? xId = null)
        {
            if (xId == null) return Json(_context.tParkings);
            var parking = _context.tParkings.Include(b => b.xTariff).Where(b => b.xId == xId).FirstOrDefault();
            return Json(parking);
        }
        public async Task<JsonResult> PriceExists(Guid? xId = null)
        {
            if (xId == null) return Json(_context.tTariffs);
            var tariff = await _context.tTariffs.FindAsync(xId);
            return Json(tariff);
        }
        public async Task<JsonResult> GateExists(Guid? xId = null)
        {
            if (xId == null) return Json(_context.tGates);
            var gate = await _context.tGates.FindAsync(xId);
            return Json(gate);
        }
        public async Task<JsonResult> TagExists(string? xId = null)
        {
            if (xId == null) return Json(_context.tTagList);
            var tag = await _context.tTagList.FindAsync(xId);
            return Json(tag);
        }

        [HttpPost]
        public async Task<IActionResult> ParkingEdit([Bind("xId,xCode,xTitle,xAddress,xPhone,xCordination,xCapacity,xReservedCapacity,xPassword,xContractor,xDescription,xStatusCode,xSettings,xTariffId")] tParking parking)
        {
            if (parking.xId == Guid.Empty)
            {
                parking.xId = Guid.NewGuid();
                parking.xReservedCapacity = 0;
                parking.xStatusCode = 0;
                _context.Add(parking);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (_context.tParkings == null)
                {
                    return Problem("Entity set 'PMSContext.Parkings'  is null.");
                }
                if (_context.tParkings.Any(e => e.xId == parking.xId))
                {
                    try
                    {
                        _context.Update(parking);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> TagGroupEdit([Bind("xId,xName,xTariffId")] tTagGroup group)
        {
            if (group.xId == Guid.Empty)
            {
                group.xId = Guid.NewGuid();
                _context.Add(group);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (_context.tTagGroup == null)
                {
                    return Problem("Entity set 'Tag Group'  is null.");
                }
                if (!_context.tTagGroup.Any(e => e.xId == group.xId))
                {
                    return NotFound();
                }
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> TagEdit([Bind("xId,xExpirationDate,xComment,xTagGroupId")] tTagList tag)
        {
            try
            {
                if (tag.xExpirationDate.HasValue) tag.xExpirationDate = tag.xExpirationDate.Value.AddMinutes(1439);
                if (_context.tTagList == null)
                {
                    return Problem("Entity set 'Tag List'  is null.");
                }
                if (_context.tTagList.Any(e => e.xId == tag.xId))
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                    return Ok("UPDATE");
                }
                else
                {
                    _context.Add(tag);
                    await _context.SaveChangesAsync();
                    return Ok("ADD");
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PriceEdit([Bind("xId,xParkingId,xTitle,xEntryPrice,xDayHourPrice,xNightHourPrice,xWholeDayPrice,xNightStartHour,xNightEndHour,xDescription")] tTariff tariff)
        {
            if (tariff.xId == Guid.Empty)
            {
                tariff.xId = Guid.NewGuid();
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (_context.tTariffs == null)
                {
                    return Problem("Entity set 'PMSContext.Price'  is null.");
                }
                if (_context.tTariffs.Any(e => e.xId == tariff.xId))
                {
                    try
                    {
                        _context.Update(tariff);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> GateEdit([Bind("xId,xParkingId,xName,xDirection,xVideoInUrl,xVideoOutUrl,xImageInUrl,xImageOutUrl,xBarrierInUrl,xBarrierOutUrl,xDescription")] tGate gate)
        {
            if (gate.xId == Guid.Empty)
            {
                gate.xId = Guid.NewGuid();
                _context.Add(gate);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (_context.tGates == null)
                {
                    return Problem("Entity set 'PMSContext.Gate'  is null.");
                }
                if (_context.tGates.Any(e => e.xId == gate.xId))
                {
                    try
                    {
                        _context.Update(gate);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParkingDelete(Guid xId)
        {
            if (_context.tParkings == null)
            {
                return Problem("Entity set 'PMSContext.Parkings'  is null.");
            }
            var parking = await _context.tParkings.FindAsync(xId);
            if (parking != null)
            {
                _context.tParkings.Remove(parking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Parkings");
            }
            return NotFound(xId);
        }
        [HttpPost]
        public async Task<IActionResult> PriceDelete(Guid xId)
        {
            if (_context.tTariffs == null)
            {
                return Problem("Entity set 'PMSContext.Price'  is null.");
            }
            var tariff = await _context.tTariffs.FindAsync(xId);
            if (tariff != null)
            {
                _context.tTariffs.Remove(tariff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Price");
            }
            return NotFound(xId);
        }
        [HttpPost]
        public async Task<IActionResult> GateDelete(Guid xId)
        {
            if (_context.tGates == null)
            {
                return Problem("Entity set 'PMSContext.Gate'  is null.");
            }
            var gate = await _context.tGates.FindAsync(xId);
            if (gate != null)
            {
                _context.tGates.Remove(gate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Gates");
            }
            return NotFound(xId);
        }
        [HttpPost]
        public async Task<IActionResult> TagGroupDelete(Guid xId)
        {
            if (_context.tTagGroup == null)
            {
                return Problem("Entity set 'Tag Group' is null.");
            }
            var group = await _context.tTagGroup.FindAsync(xId);
            if (group != null)
            {
                try
                {
                    _context.tTagGroup.Remove(group);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }
            }
            return NotFound(xId);
        }
        [HttpPost]
        public async Task<IActionResult> TagDelete(string id)
        {
            if (_context.tTagList == null)
            {
                return Problem("Entity set 'Tag List' is null.");
            }
            var tag = await _context.tTagList.FindAsync(id);
            if (tag != null)
            {
                try
                {
                    _context.tTagList.Remove(tag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CardGroup");
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }
            }
            return NotFound(id);
        }
        public JsonResult getTagGroup(Guid? id)
        {
            return id.HasValue
                ? Json(_context.tTagGroup.Include(b => b.xTariff).Where(b => b.xId == id))
                : Json(_context.tTagGroup);
        }
    }
}
