namespace Ticketsystem.Models.Database
{
    public class PermissionsTriggered
    {
        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}
