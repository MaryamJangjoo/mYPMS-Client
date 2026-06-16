using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using mYPMS.Data;
using mYPMS.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace mYPMS.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly mYPMSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ReportController(mYPMSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index(Guid? parking, Guid? gate, int? status,
            string? dateFrom, string? dateTo, string? license, string? tag)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                DateTime datetimeFrom = new(), datetimeTo = new();
                if (DateTime.TryParse(dateFrom, out datetimeFrom) == false) datetimeFrom = DateTime.Now.AddDays(-1);
                datetimeTo = DateTime.TryParse(dateTo, out datetimeTo) ? datetimeTo.AddDays(1) : DateTime.Now;
                string query = "SELECT * FROM tTraffics";
                if (parking.HasValue || gate.HasValue || status.HasValue || tag != null || license != null)
                {
                    query += " WHERE xId IS NOT NULL";
                    query += parking.HasValue ? $" AND xParkingId = '{parking}'" : "";
                    query += gate.HasValue ? $" AND (xEntryGateId = '{gate}' OR xDepartureGateId = '{gate}')" : "";
                    query += status.HasValue ? $" AND xStatusCode = {status}" : "";
                    query += tag != null ? $" AND xTagId LIKE '%{tag}%'" : "";
                    //query += license != null ? $" AND (xLicencePlateEn LIKE '%{license}%' OR xLicencePlateEx LIKE '%{license}%')" : "";
                }
                var traffics = _context.tTraffics
                    .FromSqlRaw(query)
                    .Where(b => EF.Functions.Like(b.xLicencePlateEn, $"%{license}%") || EF.Functions.Like(b.xLicencePlateEx, $"%{license}%"))
                    .Where(b => (b.xEntryDateTime >= datetimeFrom && b.xEntryDateTime <= datetimeTo) || (b.xDepartureDateTime >= datetimeFrom && b.xDepartureDateTime <= datetimeTo))
                    .Include(b => b.xParking)
                    .Include(b => b.xEntryGate)
                    .Include(b => b.xDepartureGate)
                    .OrderByDescending(t => t.xEntryDateTime)
                    .Take(1000);
                return PartialView("_IndexGrid", traffics);
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public FileContentResult Export(Guid? parking, Guid? gate, int? status,
            string? dateFrom, string? dateTo, string? license, string? tag)
        {
            DateTime datetimeFrom = new(), datetimeTo = new();
            if (DateTime.TryParse(dateFrom, out datetimeFrom) == false) datetimeFrom = DateTime.Now.AddDays(-1);
            datetimeTo = DateTime.TryParse(dateTo, out datetimeTo) ? datetimeTo.AddDays(1) : DateTime.Now;
            string query = "SELECT * FROM tTraffics";
            if (parking.HasValue || gate.HasValue || status.HasValue || tag != null || license != null)
            {
                query += " WHERE xId IS NOT NULL";
                query += parking.HasValue ? $" AND xParkingId = '{parking}'" : "";
                query += gate.HasValue ? $" AND (xEntryGateId = '{gate}' OR xDepartureGateId = '{gate}')" : "";
                query += status.HasValue ? $" AND xStatusCode = {status}" : "";
                query += tag != null ? $" AND xTagId LIKE '%{tag}%'" : "";
                //query += license != null ? $" AND (xLicencePlateEn LIKE '%{license}%' OR xLicencePlateEx LIKE '%{license}%')" : "";
            }
            var traffics = _context.tTraffics
                .FromSqlRaw(query)
                .Where(b => EF.Functions.Like(b.xLicencePlateEn, $"%{license}%") || EF.Functions.Like(b.xLicencePlateEx, $"%{license}%"))
                .Where(b => (b.xEntryDateTime >= datetimeFrom && b.xEntryDateTime <= datetimeTo) || (b.xDepartureDateTime >= datetimeFrom && b.xDepartureDateTime <= datetimeTo))
                .Include(b => b.xParking)
                .Include(b => b.xEntryGate)
                .Include(b => b.xDepartureGate)
                .Include(b => b.xTariff)
                .OrderByDescending(t => t.xEntryDateTime)
                .ToList();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage package = new();

            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("تردد");
            sheet.View.RightToLeft = true;

            ExcelWorksheet pivot = package.Workbook.Worksheets.Add("تحلیل");
            pivot.View.RightToLeft = true;

            sheet.Cells["A1:M1"].Merge = true;
            sheet.Cells["A2:C2"].Merge = true;
            sheet.Cells["D2:F2"].Merge = true;
            sheet.Cells["H2:I2"].Merge = true;
            sheet.Cells["K2:L2"].Merge = true;
            sheet.Cells["A1"].Value = "گزارش تردد پارکینگ";
            sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A2"].Value = $"از تاریخ: {datetimeFrom}";
            sheet.Cells["D2"].Value = $"تا تاریخ: {datetimeTo}";
            sheet.Cells["G2"].Value = "تعداد ترددها:";
            sheet.Cells["J2"].Value = "مبلغ کل:";
            sheet.Cells["A3"].Value = "پارکینگ";
            sheet.Cells["B3"].Value = "شماره کارت";
            sheet.Cells["C3"].Value = "تاریخ ورود";
            sheet.Cells["D3"].Value = "تاریخ خروج";
            sheet.Cells["E3"].Value = "دروازه ورود";
            sheet.Cells["F3"].Value = "دروازه خروج";
            sheet.Cells["G3"].Value = "کاربر ورود";
            sheet.Cells["H3"].Value = "کاربر خروج";
            sheet.Cells["I3"].Value = "پلاک ورود";
            sheet.Cells["J3"].Value = "پلاک خروج";
            sheet.Cells["K3"].Value = "وضعیت";
            sheet.Cells["L3"].Value = "مبلغ";
            sheet.Cells["M3"].Value = "تعرفه";
            //sheet.Cells["N3"].Value = "شیوه پرداخت";
            int i = 4;
            traffics.ForEach(t =>
            {
                sheet.Cells[i, 1].Value = t.xParking == null ? "" : t.xParking.xTitle;
                sheet.Cells[i, 2].Value = t.xTagId ?? "";
                sheet.Cells[i, 3].Value = t.xEntryDateTime!.Value.ToPeString();
                sheet.Cells[i, 4].Value = t.xDepartureDateTime == null ? "" : t.xDepartureDateTime.Value.ToPeString();
                sheet.Cells[i, 5].Value = t.xEntryGate == null ? "" : t.xEntryGate.xName;
                sheet.Cells[i, 6].Value = t.xDepartureGate == null ? "" : t.xDepartureGate.xName;
                sheet.Cells[i, 7].Value = t.xEntryOperator ?? "";
                sheet.Cells[i, 8].Value = t.xDepartureOperator ?? "";
                sheet.Cells[i, 9].Value = t.xLicencePlateEn ?? "";
                sheet.Cells[i, 10].Value = t.xLicencePlateEx ?? "";
                if (t.xStatusCode == 1) sheet.Cells[i, 11].Value = "وارد شده";
                else if (t.xStatusCode == 2) sheet.Cells[i, 11].Value = "خارج شده";
                else if (t.xStatusCode == 4) sheet.Cells[i, 11].Value = "حذف شده";
                else sheet.Cells[i, 11].Value = "نامعلوم";
                sheet.Cells[i, 12].Value = t.xPaid ?? 0;
                sheet.Cells[i, 13].Value = t.xTariff == null ? "" : t.xTariff.xTitle;
                //sheet.Cells[i, 14].Value = t.xPaidMethod == null ? "" : t.xPaidMethod.xPaymentMethod;
                sheet.Cells[i, 1, i, 13].Style.Fill.SetBackground((i % 2) == 0 ? Color.LightGray : Color.WhiteSmoke);
                if (t.xProperties != null) sheet.Cells[i, 1, i, 13].Style.Fill.SetBackground(Color.Red);
                i++;
            });
            if (i < 5) i = 5;
            sheet.Cells["H2"].Formula = $"COUNT(L4:L{i - 1})";
            sheet.Cells["K2"].Formula = $"SUM(L4:L{i - 1})";
            sheet.Cells[$"B4:B{i - 1}"].Style.Numberformat.Format = "@";
            sheet.Cells["K2"].Style.Numberformat.Format = "#,##0";
            sheet.Cells[$"L4:L{i}"].Style.Numberformat.Format = "#,##0";
            sheet.Cells[$"C4:D{i}"].Style.Numberformat.Format = "YYYY/MM/DD HH:MM:SS";
            sheet.Calculate();

            // Create the Pivot Table
            ExcelRange dataRange = sheet.Cells[$"A3:M{i - 1}"];
            var pivotTable = pivot.PivotTables.Add(pivot.Cells["A7"], dataRange, "PivotTable");

            var pageField = pivotTable.PageFields.Add(pivotTable.Fields[2]);    // تاریخ ورود
            pageField.Format = "YYYY/MM/DD HH:MM:SS";
            pageField = pivotTable.PageFields.Add(pivotTable.Fields[3]);    // تاریخ خروج
            pageField.Format = "YYYY/MM/DD HH:MM:SS";
            pageField = pivotTable.PageFields.Add(pivotTable.Fields[4]);    // دروازه ورود
            pageField = pivotTable.PageFields.Add(pivotTable.Fields[5]);    // دروازه خروج
            pageField = pivotTable.PageFields.Add(pivotTable.Fields[12]);    // تعرفه

            var rowField = pivotTable.RowFields.Add(pivotTable.Fields[0]);  // پارکینگ
            rowField.Outline = false;
            rowField.Compact = false;
            rowField.ShowAll = false;
            rowField.SubtotalTop = false;
            rowField = pivotTable.RowFields.Add(pivotTable.Fields[10]);     // وضعیت
            rowField.Items.ShowDetails(false);
            rowField = pivotTable.RowFields.Add(pivotTable.Fields[6]);      // کاربر ورود
            rowField.Items.ShowDetails(false);
            rowField = pivotTable.RowFields.Add(pivotTable.Fields[7]);      // کاربر خروج
            rowField.Items.ShowDetails(false);

            var dataField = pivotTable.DataFields.Add(pivotTable.Fields[11]);   // مبلغ
            dataField.Name = "تعداد تردد";
            dataField.Function = OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Count;
            dataField.Format = "#,##0";
            dataField = pivotTable.DataFields.Add(pivotTable.Fields[11]); // مبلغ
            dataField.Name = "جمع مبلغ";
            dataField.Function = OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum;
            dataField.Format = "#,##0";

            pivotTable.GrandTotalCaption = "جمع کل";
            pivotTable.RowHeaderCaption = "پارکینگ";
            pivotTable.DataCaption = "داده";
            pivotTable.ColumnHeaderCaption = "عنوان ستون";
            pivotTable.GridDropZones = true;

            // Traffic sheet style
            sheet.Cells["A2:M2"].Style.Fill.SetBackground(Color.Black);
            sheet.Cells["A2:M2"].Style.Font.Color.SetColor(Color.White);
            sheet.Cells["A3:M3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A3:M3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            sheet.Cells[$"A1:M{i - 1}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            sheet.Cells.Style.Font.SetFromFont("B Nazanin", 10);
            sheet.Cells["A1"].Style.Font.Size = 24;
            sheet.Cells["A2:M2"].Style.Font.Size = 12;
            sheet.Cells["A1:M3"].Style.Font.Bold = true;
            sheet.Columns.AutoFit();

            // Pivot table style
            var styleWholeTable = pivotTable.Styles.AddWholeTable();
            styleWholeTable.Style.Font.Name = "B Nazanin";
            styleWholeTable.Style.Font.Bold = true;
            styleWholeTable.Style.Font.Size = 12;

            //ExcelPicture picture = sheet.Drawings.AddPicture("Sample", @"C:\Users\hmosa\source\repos\mYPMS\wwwroot\imgs\3-plate.jpg");
            //picture.SetPosition(3, 0, 3, 0);
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToPeString("yyyyMMdd")}-Report.xlsx");
        }
        public IActionResult Today()
        {
            string? pid = HttpContext.Session.GetString("pid");
            if (!Guid.TryParse(pid, out Guid parkingId)) return Redirect("~/Home/Welcome");
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                DateTime datetimeFrom = DateTime.Today;
                var traffics = _context.tTraffics
                    .Where(b => b.xParkingId == parkingId && (b.xEntryDateTime >= datetimeFrom || b.xDepartureDateTime >= datetimeFrom))
                    .Include(b => b.xParking)
                    .Include(b => b.xEntryGate)
                    .Include(b => b.xDepartureGate)
                    .OrderByDescending(t => t.xEntryDateTime);
                return PartialView("_TodayGrid", traffics);
            }
            return View();
        }
        public JsonResult GetTraffic(string guid)
        {
            HttpContext.Response.StatusCode = 500;
            if (!Guid.TryParse(guid, out Guid id)) return new JsonResult("شناسه تردد معتبر نیست");
            tTraffic? traffic = _context.tTraffics
                .Include(b => b.xTariff)
                .FirstOrDefault(b => b.xId == id);
            if (traffic == null) return new JsonResult("شناسه تردد یافت نشد");
            HttpContext.Response.StatusCode = 200;
            return Json(traffic);
        }
        public IStatusCodeActionResult ChangeStatus(string id, int newStatus = 0)
        {
            if (!Guid.TryParse(id, out Guid _id)) return Problem("شناسه تردد معتبر نیست");
            if (!(newStatus == 1 || newStatus == 2 || newStatus == 4)) return Problem("کد وضعیت جدید صحیح نیست");

            tTraffic? traffic = _context.tTraffics.FirstOrDefault(b => b.xId == _id);
            if (traffic == null) return Problem("شناسه تردد یافت نشد");

            Guid pid = new(HttpContext.Session.GetString("pid") ?? Guid.Empty.ToString());
            Guid gid = new(HttpContext.Session.GetString("gid") ?? Guid.Empty.ToString());
            if (pid == Guid.Empty || gid == Guid.Empty) return Problem("پارکینگ و دروازه را مجدد انتخاب کنید");

            tParking? parking = _context.tParkings.Include(b => b.xTariff).SingleOrDefault(b => b.xId == pid);
            if (parking == null) return Problem("پارکینگ مربوطه یافت نشد");

            tGate? gate = _context.tGates.SingleOrDefault(b => b.xId == gid);
            if (gate == null) return Problem("دروازه مربوطه یافت نشد");

            tTariff? tariff = null;
            tariff = GetTariff(traffic.xTagId, ref parking);
            if (tariff == null) return Problem("تعرفه مربوطه یافت نشد");

            IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (newStatus == 1)
            {
                traffic.xDepartureDateTime = null;
                traffic.xDepartureGate = null;
                traffic.xDepartureOperator = null;
            }
            if (newStatus == 2)
            {
                traffic.xDepartureDateTime = DateTime.Now;
                traffic.xDepartureGate = gate;
                traffic.xDepartureOperator = user.UserName;
            }
            GetPrice(ref traffic);
            traffic.xStatusCode = newStatus;
            traffic.xProperties = "تغییر دستی وضعیت";
            _context.tTraffics.Update(traffic);
            _context.SaveChanges();
            return Ok();
        }
        public JsonResult getParkingWithGates()
        {
            return Json(_context.tParkings.Include(t => t.xGates));
        }
        private tTariff? GetTariff(string? tag, ref tParking parking)
        {
            tTagList? tagFromList = null;
            tTariff? tariff = null;
            tagFromList = _context.tTagList.SingleOrDefault(b => b.xId == tag);
            if (tagFromList != null) tariff = _context.tTagList
                    .Where(b => b.xId == tag)
                    .Include(b => b.xTagGroup)
                    .ThenInclude(b => b.xTariff)
                    .First().xTagGroup.xTariff;
            tariff ??= parking.xTariff;
            return tariff;
        }
        private static void GetPrice(ref tTraffic t)
        {
            tTariff tariff = t.xTariff;
            bool entrancePayment = IsEntrancePayment(ref tariff);
            if (t.xStatusCode == 1 && entrancePayment)
            {
                t.xPaid = tariff.xEntryPrice;
                return;
            }
            else if (t.xStatusCode == 2 && !entrancePayment)
            {
                if (t.xDepartureDateTime == null || t.xEntryDateTime == null) return;
                double parked_total_minutes;
                try
                {
                    parked_total_minutes = t.xDepartureDateTime.Value.Subtract(t.xEntryDateTime.Value).TotalMinutes;
                }
                catch (Exception)
                {
                    return;
                }

                int ONE_DAY_MINUTES = 1440;
                int ONE_HOUR_MINUTES = 60;
                int WHOLE_DAY_PRICE = tariff.xWholeDayPrice ?? 0;
                int ENTRANCE_PRICE = tariff.xEntryPrice ?? 0;
                int NORMAL_START_MINUTE = tariff.xNightStartHour ?? 0 * 60;
                int NORMAL_END_MINUTE = tariff.xNightEndHour ?? 0 * 60;
                int NORMAL_HOUR_PRICE = tariff.xDayHourPrice ?? 0;
                int NIGHT_HOUR_PRICE = tariff.xNightHourPrice ?? 0;

                int parked_entry_hours = t.xEntryDateTime.Value.Hour;
                int parked_entry_minutes = t.xEntryDateTime.Value.Minute;
                int parked_exit_hours = t.xDepartureDateTime.Value.Hour;
                int parked_exit_minutes = t.xDepartureDateTime.Value.Minute;

                int total_price;
                int normal_minutes;
                int night_minutes;
                int normal_hours;
                int night_hours;
                int entry_minute = parked_entry_hours * ONE_HOUR_MINUTES + parked_entry_minutes;
                int exit_minute = parked_exit_hours * ONE_HOUR_MINUTES + parked_exit_minutes;

                int whole_day = (int)Math.Floor(parked_total_minutes / ONE_DAY_MINUTES);
                bool enter_exit_same_day = (whole_day == 0 && parked_entry_hours <= parked_exit_hours);
                bool enter_in_night;

                if (enter_exit_same_day || (exit_minute >= entry_minute))
                {
                    night_minutes = (entry_minute < NORMAL_START_MINUTE) ? NORMAL_START_MINUTE - entry_minute : 0;
                    night_minutes = (exit_minute < NORMAL_START_MINUTE) ? exit_minute - entry_minute : night_minutes;
                    night_minutes = (entry_minute > NORMAL_END_MINUTE) ? exit_minute - entry_minute : night_minutes;
                    enter_in_night = (night_minutes > 0);
                    normal_minutes = (int)(parked_total_minutes % ONE_DAY_MINUTES) - night_minutes;
                }
                else
                {
                    normal_minutes = (entry_minute < NORMAL_END_MINUTE) ? NORMAL_END_MINUTE - entry_minute : 0;
                    normal_minutes -= (entry_minute < NORMAL_START_MINUTE) ? NORMAL_START_MINUTE - entry_minute : 0;
                    enter_in_night = normal_minutes <= 0;
                    normal_minutes += (exit_minute > NORMAL_START_MINUTE) ? exit_minute - NORMAL_START_MINUTE : 0;
                    normal_minutes += (exit_minute > NORMAL_END_MINUTE) ? exit_minute - NORMAL_END_MINUTE : 0;
                    night_minutes = (int)(parked_total_minutes % ONE_DAY_MINUTES) - normal_minutes;
                }

                if (parked_total_minutes >= ONE_HOUR_MINUTES && enter_in_night)
                {
                    if (night_minutes >= ONE_HOUR_MINUTES) night_minutes -= ONE_HOUR_MINUTES;
                    else
                    {
                        normal_minutes -= (ONE_HOUR_MINUTES - night_minutes);
                        night_minutes = 0;
                    }
                }
                else if (parked_total_minutes >= ONE_HOUR_MINUTES && !enter_in_night)
                    normal_minutes -= ONE_HOUR_MINUTES;

                normal_hours = (int)normal_minutes / 60;
                night_hours = (int)night_minutes / 60;
                normal_hours = (normal_minutes % 60 > 0 && parked_total_minutes >= 60) ? ++normal_hours : normal_hours;
                night_hours = (night_minutes % 60 > 0 && parked_total_minutes >= 60) ? ++night_hours : night_hours;

                if (whole_day == 0)
                {
                    total_price = (
                            night_hours * NIGHT_HOUR_PRICE +
                                    normal_hours * NORMAL_HOUR_PRICE +
                                    ENTRANCE_PRICE);
                }
                else
                {
                    total_price = (
                            night_hours * NIGHT_HOUR_PRICE +
                                    normal_hours * NORMAL_HOUR_PRICE +
                                    whole_day * WHOLE_DAY_PRICE);
                }
                t.xPaid = total_price;
            }
        }
        private static bool IsEntrancePayment(ref tTariff t)
        {
            return t.xDayHourPrice == 0 || t.xWholeDayPrice == 0 || t.xNightHourPrice == 0;
        }
    }
}

/*
public IActionResult GetExcel()
{
    using(var package = new ExcelPackage())
    {
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var excelData = package.GetAsByteArray();
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = "MyWorkbook.xlsx";
        return File(excelData, contentType, fileName);
    }
}

https://epplussoftware.com/en/Developers
https://epplussoftware.com/en/Developers/PivotTables
https://riptutorial.com/epplus/example/27333/creating-a-pivot-table
*/