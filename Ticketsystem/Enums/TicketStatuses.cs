using System.ComponentModel;

namespace Ticketsystem.Enums;

public enum TicketStatuses
{
    [Description("Offen")]
    Open,

    [Description("In Bearbeitung")]
    InProgress,

    [Description("Warte auf Rückmeldung")]
    WaitingForResponse,

    [Description("Gelöst")]
    Resolved,

    [Description("Geschlossen")]
    Closed,

    [Description("Eskaliert")]
    Escalated,

    [Description("Zurückgestellt")]
    OnHold
}
