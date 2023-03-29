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

        [Description("Neue Tickets erstellen")]
        CreateTickets,

        [Description("Tickets suchen")]
        SearchTickets,

        [Description("Tickets ändern")]
        UpdateTickets,

        [Description("Ticketdetails anzeigen")]
        ViewTicketDetails,

        [Description("Tickets löschen")]
        DeleteTickets,

        [Description("Ticketänderungen anzeigen")]
        ViewTicketChanges,

        [Description("Kundenliste anzeigen")]
        ViewClients,

        [Description("Kunden suchen")]
        SearchClients,

        [Description("Kundendaten ändern")]
        UpdateClients,

        [Description("Kundendaten anzeigen")]
        ViewClientDetails,

        [Description("Kunden löschen")]
        DeleteClients
    }
}
