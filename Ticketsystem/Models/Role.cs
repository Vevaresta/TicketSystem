using Microsoft.AspNetCore.Identity;

namespace Ticketsystem.Models
{
    public class Role : IdentityRole
    {
        public Role() : base()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
