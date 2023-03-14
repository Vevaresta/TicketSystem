using Microsoft.CodeAnalysis.Editing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Ticketsystem.ViewModels;

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

    public TicketType TicketType { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public Client Client { get; set; }

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
            DataBackupByClient = ticket.DataBackupByClient,
            DataBackupByStaff = ticket.DataBackupByStaff,
            DataBackupDone = ticket.DataBackupDone,
            TicketType = ticket.TicketType.Name,
            TicketStatus = ticket.TicketStatus.Name,
            Devices = new List<DeviceViewModel>()
        };

        foreach (var device in ticket.Devices)
        {
            viewModel.Devices.Add(device);
        }

        return viewModel;
    }
}
