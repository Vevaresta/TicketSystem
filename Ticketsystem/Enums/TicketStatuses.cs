using System.ComponentModel;

namespace Ticketsystem.Enums;

public enum TicketStatuses
{
    [Description("Offen")]
    Open,

    [Description("In Bearbeitung")]
    InProgress,

    [Description("Warte auf R�ckmeldung")]
    WaitingForResponse,

    [Description("Gel�st")]
    Resolved,

    [Description("Geschlossen")]
    Closed,

    [Description("Eskaliert")]
    Escalated,

    [Description("Zur�ckgestellt")]
    OnHold
}
