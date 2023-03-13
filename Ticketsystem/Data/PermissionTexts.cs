using Ticketsystem.Enums;

namespace Ticketsystem.Data
{
    public static class PermissionTexts
    {
        public static Dictionary<RolePermissions, string> Texts { get; set; } = new Dictionary<RolePermissions, string>
        {
            [RolePermissions.ManageUsers] = "Benutzer verwalten",
            [RolePermissions.AddUsers] = "Neue Benutzer hinzufügen",
            [RolePermissions.ChangeUserRole] = "Benutzerrolle ändern",
            [RolePermissions.DeleteUsers] = "Benutzer löschen",

            [RolePermissions.ManageRoles] = "Rollen verwalten",
            [RolePermissions.AddRoles] = "Neue Rollen hinzufügen",
            [RolePermissions.DeleteRoles] = "Rolle löschen",
            [RolePermissions.ChangeRolePermissions] = "Berechtigungen von Rollen ändern",

            [RolePermissions.ViewTickets] = "Tickets anzeigen",
            [RolePermissions.CreateTickets] = "Neue Tickets erstellen",
            [RolePermissions.UpdateTickets] = "Tickets updaten",
            [RolePermissions.DeleteTickets] = "Tickets löschen",

            [RolePermissions.ViewHistory] = "Ticketverlauf anzeigen",
        };
    }
}
