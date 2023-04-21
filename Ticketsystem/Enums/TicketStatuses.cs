using System.ComponentModel;

namespace Ticketsystem.Enums;

public enum TicketStatuses
{
    [Description("Offen")]
    Open,

    [Description("In Bearbeitung")]
    InProgress,

    [Description("Geschlossen")]
    Closed

}
