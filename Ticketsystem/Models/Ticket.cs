using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketsystem.Models;

public class Ticket
{
    public Ticket()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    [ForeignKey(nameof(TicketType))]
    public string TicketTypeId { get; set; }

    [ForeignKey(nameof(TicketStatus))]
    public string TicketStatusId { get; set; }

    public string WorkOrder { get; set; }
    public bool DataBackupByClient { get; set; }
    public bool DataBackupByStaff { get; set; }
    public bool DataBackupDone { get; set; }
    public string Comments { get; set; }

    public TicketType TicketType { get; set; }
    public TicketStatus TicketStatus { get; set; }

    public virtual ICollection<Device> Devices { get; set; }
    public virtual ICollection<User> Users { get; set; }
}
