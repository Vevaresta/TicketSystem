using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.Enums;
using Ticketsystem.Extensions;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models;

public class Ticket
{
    public Ticket()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public bool DoBackup { get; set; } = false;
    public bool DataBackupByClient { get; set; } = false;
    public bool DataBackupByStaff { get; set; } = false;
    public bool DataBackupDone { get; set; }

    public virtual TicketType TicketType { get; set; }
    public virtual TicketStatus TicketStatus { get; set; }
    public virtual Client Client { get; set; }


    public virtual IList<Device> Devices { get; set; }
    public virtual IList<TicketUsers> TicketUsers { get; set; }
    public virtual IList<TicketChanges> TicketChanges { get; set; }

    public static implicit operator TicketViewModel(Ticket ticket)
    {
        TicketViewModel viewModel = new()
        {
            Id = ticket.Id,
            Name = ticket.Name,
            WorkOrder = ticket.WorkOrder,
            OrderDate = ticket.OrderDate,
            Client = ticket.Client,
            DoBackup = ticket.DoBackup,
            DataBackupByClient = ticket.DataBackupByClient,
            DataBackupByStaff = ticket.DataBackupByStaff,
            DataBackupDone = ticket.DataBackupDone,
            TicketType = Enum.GetValues<TicketTypes>().FirstOrDefault(tt => tt.ToString() == ticket.TicketType.Name),
            TicketStatus = Enum.GetValues<TicketStatuses>().FirstOrDefault(ts => ts.ToString() == ticket.TicketStatus.Name),
            Devices = new List<DeviceViewModel>(),
        };

        foreach (var device in ticket.Devices)
        {
            viewModel.Devices.Add(device);
        }

        return viewModel;
    }

    [NotMapped]
    public int TicketId { get; set; }
}
