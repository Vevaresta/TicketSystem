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

    public static class Permissions
    {
        public static Dictionary<PermissionsEnum, string> PermissionNames { get; set; } = new Dictionary<PermissionsEnum, string>
        {
            [PermissionsEnum.ManageUsers] = "Benutzer verwalten",
            [PermissionsEnum.AddUsers] = "Neue Benutzer hinzufügen",
            [PermissionsEnum.ChangeUserRole] = "Benutzerrolle ändern",
            [PermissionsEnum.DeleteUsers] = "Benutzer löschen",

            [PermissionsEnum.ManageRoles] = "Rollen verwalten",
            [PermissionsEnum.AddRole] = "Neue Rollen hinzufügen",
            [PermissionsEnum.DeleteRole] = "Rolle löschen",
            [PermissionsEnum.ChangeRolePermissions] = "Berechtigungen von Rollen ändern",

            [PermissionsEnum.CreateTickets] = "Neue Tickets erstellen",
            [PermissionsEnum.UpdateTickets] = "Tickets updaten",
            [PermissionsEnum.DeleteTickets] = "Tickets löschen",

            [PermissionsEnum.ViewHistory] = "Ticketverlauf anzeigen",
        };
    }
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
