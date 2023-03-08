using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketsystem.Models;

public class Device
{
    public Device()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }

    public string Name { get; set; }
    public string DeviceType { get; set; }
    public string Manufacturer { get; set; }
    public string SerialNumber { get; set; }
    public string Accessories { get; set; }
    public string Comments { get; set; }

    public Ticket Ticket { get; set; }

    public virtual IList<Software> Software { get; set; }
}
