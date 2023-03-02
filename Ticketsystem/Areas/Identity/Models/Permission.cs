using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Models
{
    public class Permission
    {
        public Permission()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<EnhancedIdentityRole> Roles { get; set; }
    }
}
