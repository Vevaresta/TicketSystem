using Microsoft.AspNetCore.Identity;

namespace Ticketsystem.Areas.Identity.Data
{
    public class EnhancedIdentityRole : IdentityRole
    {
        public EnhancedIdentityRole() : base()
        {
        }

        public EnhancedIdentityRole(string roleName) : base(roleName)
        {
        }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
