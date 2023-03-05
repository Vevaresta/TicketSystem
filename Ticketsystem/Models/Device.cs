using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketsystem.Models;

public class Device
{
    public Device()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    [ForeignKey(nameof(Ticket))]
    public string TicketId { get; set; }

    public string DeviceType { get; set; }
    public string PriorDamage { get; set; }
    public string Accessories { get; set; }
    public string Comments { get; set; }

    public Ticket Ticket { get; set; }
}
