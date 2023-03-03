namespace Ticketsystem.Areas.Tickets.Models
{
    public class Client
    {
        public Client(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

    }
}
