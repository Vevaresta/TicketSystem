namespace Ticketsystem.Areas.Identity.Data
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

    public class Permissions
    {
        public static PermissionsDict PermissionNames { get; set; } = new PermissionsDict();
    }

    public enum PermissionsEnum
    {
        ManageUsers,
        AddUsers,
        ChangeUserRole,
        DeleteUsers,

        ManageRoles,
        AddRole,
        DeleteRole,
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
            this[PermissionsEnum.ManageUsers] = "Benutzer verwalten";
            this[PermissionsEnum.AddUsers] = "Neue Benutzer hinzufügen";
            this[PermissionsEnum.ChangeUserRole] = "Benutzerrolle ändern";
            this[PermissionsEnum.DeleteUsers] = "Benutzer löschen";

            this[PermissionsEnum.ManageRoles] = "Rollen verwalten";
            this[PermissionsEnum.AddRole] = "Neue Rollen hinzufügen";
            this[PermissionsEnum.DeleteRole] = "Rolle löschen";
            this[PermissionsEnum.ChangeRolePermissions] = "Berechtigungen von Rollen ändern";

            this[PermissionsEnum.CreateTickets] = "Neue Tickets erstellen";
            this[PermissionsEnum.UpdateTickets] = "Tickets updaten";
            this[PermissionsEnum.DeleteTickets] = "Tickets löschen";

            this[PermissionsEnum.ViewHistory] = "Ticketverlauf anzeigen";
        }
    }
}
