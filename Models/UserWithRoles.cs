using Microsoft.AspNetCore.Identity;

namespace mYPMS.Models
{
    public class UsersAndRoles
    {
        public List<UserWithRoles>? Users { get; set; }
        public List<IdentityRole>? Roles { get; set; }
    }
    public class UserWithRoles
    {
        public IdentityUser? User { get; set; }
        public string? RolesName { get; set; }
    }
}
