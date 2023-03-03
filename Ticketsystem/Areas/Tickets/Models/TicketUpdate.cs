namespace Ticketsystem.Areas.Tickets.Models
{
    public class TicketUpdate
    {
        public TicketUpdate(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

    }
}
