namespace Ticketsystem.Areas.Tickets.Models
{
    public class Software
    {
        public Software(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
