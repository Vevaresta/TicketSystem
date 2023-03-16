using Ticketsystem.Enums;

namespace Ticketsystem.Data
{
    public static class TicketStatusTexts
    {
        public static Dictionary<TicketStatuses, string> Texts { get; } = new Dictionary<TicketStatuses, string>()
        {
            [TicketStatuses.Open] = "Offen",
            [TicketStatuses.InProgress] = "In Bearbeitung",
            [TicketStatuses.WaitingForResponse] = "Warte auf Rückmeldung",
            [TicketStatuses.Resolved] = "Gelöst",
            [TicketStatuses.Escalated] = "Eskaliert",
            [TicketStatuses.OnHold] = "Zurückgestellt",
            [TicketStatuses.Closed] = "Geschlossen"
        };
    }
}
