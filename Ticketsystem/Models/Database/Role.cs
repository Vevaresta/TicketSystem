using Microsoft.AspNetCore.Identity;

namespace Ticketsystem.Models.Database
{
    public class Role : IdentityRole
    {
        public Role() : base()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public virtual IList<Permission> Permissions { get; set; }
    }
}
