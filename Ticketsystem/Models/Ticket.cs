using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Ticketsystem.Models;

public class Ticket
{
    public Ticket()
    {
    }

    public int Id { get; set; }

    [ForeignKey(nameof(TicketType))]
    public string TicketTypeId { get; set; }

    [ForeignKey(nameof(TicketStatus))]
    public string TicketStatusId { get; set; }

    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; }

    public DateTime OrderDate { get; set; }
    public string WorkOrder { get; set; }
    public bool DataBackupByClient { get; set; }
    public bool DataBackupByStaff { get; set; }
    public bool DataBackupDone { get; set; }
    public string Comments { get; set; }

    public TicketType TicketType { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public Client Client { get; set; }

    public virtual IList<Device> Devices { get; set; }
    public virtual IList<TicketUsers> TicketUsers { get; set; }
    public virtual IList<TicketChanges> TicketChanges { get; set; }
}
