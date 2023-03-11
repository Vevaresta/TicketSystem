using System.ComponentModel;
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

    [Required(ErrorMessage = "Dies ist ein Plichtfeld")]
    [DisplayName("Ticketname")]
    public string Name { get; set; }

    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Dies ist ein Plichtfeld")]
    [DisplayName("Arbeitsanweisung")]
    public string WorkOrder { get; set; }

    public bool DataBackupByClient { get; set; }
    public bool DataBackupByStaff { get; set; }
    public bool DataBackupDone { get; set; }

    [DisplayName("Notizen")]
    public string Comments { get; set; }

    public TicketType TicketType { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public Client Client { get; set; }

    public virtual IList<Device> Devices { get; set; }
    public virtual IList<TicketUsers> TicketUsers { get; set; }
    public virtual IList<TicketChanges> TicketChanges { get; set; }
}
