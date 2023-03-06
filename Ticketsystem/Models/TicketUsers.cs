namespace Ticketsystem.Models
{
    public class TicketUsers
    {
        public string TicketId { get; set; }
        public string UserId { get; set; }

        public Ticket Ticket { get; set; }
        public User User { get; set; }
    }
}
