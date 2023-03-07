namespace Ticketsystem.Models
{
    public class TicketChanges
    {
        public string UserId { get; set; }
        public int TicketId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }

        public User User { get; set; }
        public Ticket Ticket { get; set; }
    }
}
