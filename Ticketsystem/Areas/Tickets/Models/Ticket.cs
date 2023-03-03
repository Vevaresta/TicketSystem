namespace Ticketsystem.Areas.Tickets.Models
{
    public class Ticket
    {
        public Ticket(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
