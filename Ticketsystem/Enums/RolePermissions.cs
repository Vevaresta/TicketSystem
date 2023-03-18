using System.ComponentModel;

namespace Ticketsystem.Enums
{
    public enum RolePermissions
    {
        [Description("Benutzer verwalten")]        
        ManageUsers,

        [Description("Neue Benutzer hinzufügen")]
        AddUsers,

        [Description("Benutzerrolle ändern")]
        ChangeUserRole,

        [Description("Benutzer löschen")]
        DeleteUsers,

        [Description("Rollen verwalten")]
        ManageRoles,

        [Description("Neue Rollen hinzufügen")]
        AddRoles,

        [Description("Rolle löschen")]
        DeleteRoles,

        [Description("Berechtigungen von Rollen ändern")]
        ChangeRolePermissions,

        [Description("Ticketliste anzeigen")]
        ViewTickets,

        [Description("Ticketdetails anzeigen")]
        ViewTicketDetails,

        [Description("Tickets suchen")]
        SearchTickets,

        [Description("Neue Tickets erstellen")]
        CreateTickets,

        [Description("Tickets ändern")]
        UpdateTickets,

        [Description("Tickets löschen")]
        DeleteTickets,

        [Description("Ticketverlauf anzeigen")]
        ViewHistory
    }
}
