namespace Ticketsystem.Areas.Tickets.Models
{
    public class TicketStatus
    {
        public TicketStatus(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

    }
}
