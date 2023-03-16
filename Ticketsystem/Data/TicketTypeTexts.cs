using Ticketsystem.Enums;

namespace Ticketsystem.Data
{
    public static class TicketTypeTexts
    {
        public static Dictionary<TicketTypes, string> Texts { get; } = new Dictionary<TicketTypes, string>
        {
            [TicketTypes.Repair] = "Reparatur",
            [TicketTypes.DataRecovery] = "Datenrettung",
            [TicketTypes.Consultation] = "Beratung",
            [TicketTypes.Special] = "Spezialauftrag",
        };
    }
}
