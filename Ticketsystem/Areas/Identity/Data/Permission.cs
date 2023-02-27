namespace Ticketsystem.Areas.Identity.Data
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EnhancedIdentityRole> Roles { get; set; }
    }
}
