using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Ticketsystem.Enums;
using Ticketsystem.Extensions;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models.Database;

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
    public int ClientId { get; set; }

    public string Name { get; set; }

    public DateTime OrderDate { get; set; }

    public string WorkOrder { get; set; }

    public bool DoBackup { get; set; } = false;
    public bool DataBackupByClient { get; set; } = false;
    public bool DataBackupByStaff { get; set; } = false;
    public bool DataBackupDone { get; set; }
    public bool IsVirus { get; set; }

    public virtual TicketType TicketType { get; set; }
    public virtual TicketStatus TicketStatus { get; set; }
    public virtual Client Client { get; set; }

    public byte[] PdfSigned { get; set; }

    public virtual IList<Device> Devices { get; set; }
    public virtual IList<TicketChange> TicketChanges { get; set; }

    public static implicit operator TicketViewModel(Ticket ticket)
    {
        TicketViewModel viewModel = new()
        {
            Id = ticket.Id,
            Name = ticket.Name,
            WorkOrder = ticket.WorkOrder,
            OrderDate = ticket.OrderDate.ToLocalTime(),
            Client = ticket.Client,
            DoBackup = ticket.DoBackup,
            IsVirus = ticket.IsVirus,
            DataBackupByClient = ticket.DataBackupByClient,
            DataBackupByStaff = ticket.DataBackupByStaff,
            DataBackupDone = ticket.DataBackupDone,
            TicketType = Enum.GetValues<TicketTypes>().FirstOrDefault(tt => tt.ToString() == ticket.TicketType.Name),
            TicketStatus = Enum.GetValues<TicketStatuses>().FirstOrDefault(ts => ts.ToString() == ticket.TicketStatus.Name),
            Devices = new List<DeviceViewModel>(),
        };

        if (ticket.Devices != null )
        {
            foreach (var device in ticket.Devices)
            {
                viewModel.Devices.Add(device);
            }
        }

        return viewModel;
    }
}
