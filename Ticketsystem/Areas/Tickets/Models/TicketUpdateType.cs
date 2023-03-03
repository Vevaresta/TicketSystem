namespace Ticketsystem.Areas.Tickets.Models
{
    public class TicketUpdateType
    {
        public TicketUpdateType(string id)
        {

            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

    }
}
