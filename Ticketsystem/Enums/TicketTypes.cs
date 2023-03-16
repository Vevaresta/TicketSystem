using System.ComponentModel;

namespace Ticketsystem.Enums;

public enum TicketTypes
{
    [Description("Reparatur")]
    Repair,

    [Description("Datenrettung")]
    DataRecovery,

    [Description("Beratung")]
    Consultation,

    [Description("Spezialauftrag")]
    Special
}