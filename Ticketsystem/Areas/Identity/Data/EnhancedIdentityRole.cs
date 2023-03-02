using Microsoft.AspNetCore.Identity;
using Ticketsystem.Areas.Identity.Models;

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
