namespace Ticketsystem.Areas.Tickets.Models
{
    public class Device
    {
        public Device(string id)
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
