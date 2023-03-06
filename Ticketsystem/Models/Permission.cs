namespace Ticketsystem.Models
{
    public class Permission
    {
        public Permission()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<PermissionsTriggered> PermissionsTriggered { get; set; }
    }
}
