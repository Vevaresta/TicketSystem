namespace Ticketsystem.Areas.Identity.Data
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EnhancedIdentityRole> Roles { get; set; }
    }

    public class Permissions
    {
        public static PermissionsDict PermissionNames { get; set; } = new PermissionsDict();
    }

    public enum PermissionsEnum
    {
        AddUsers,
        DeleteUsers,
        ManageUsers,
        AddRole,
        ChangeUserRole,

        ChangeRolePermissions,
        CreateTickets,
        UpdateTickets,
        DeleteTickets,
        ViewHistory
    }

    public class PermissionsDict : Dictionary<PermissionsEnum, string>
    {
        public PermissionsDict()
        {
            this[PermissionsEnum.AddUsers] = "Neue Benutzer hinzufügen";
            this[PermissionsEnum.DeleteUsers] = "Benutzer löschen";
            this[PermissionsEnum.ManageUsers] = "Benutzer verwalten";
            this[PermissionsEnum.CreateTickets] = "Neue Tickets erstellen";
            this[PermissionsEnum.UpdateTickets] = "Tickets updaten";

            this[PermissionsEnum.DeleteTickets] = "Tickets löschen";
            this[PermissionsEnum.AddRole] = "Neue Rollen hinzufügen";
            this[PermissionsEnum.ChangeUserRole] = "Benutzerrolle ändern";
            this[PermissionsEnum.ChangeRolePermissions] = "Berechtigungen von Rollen ändern";
            this[PermissionsEnum.ViewHistory] = "Ticketverlauf anzeigen";
        }
    }
}
