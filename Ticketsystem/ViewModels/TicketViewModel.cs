using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ticketsystem.Enums;
using Ticketsystem.Models;

namespace Ticketsystem.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [DisplayName("Ticketname")]
        public string Name { get; set; }

        [DisplayName("Auftragsdatum")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [DisplayName("Arbeitsanweisung")]
        public string WorkOrder { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        public bool DoBackup { get; set; }

        [Display(Name = "Bereits erledigt durch Auftraggeber")]
        public bool DataBackupByClient { get; set; }

        [Display(Name = "Durch Mitarbeiter durchzuführen")]
        public bool DataBackupByStaff { get; set; }

        [DisplayName("Backup erledigt?")]
        public bool DataBackupDone { get; set; }
        public ClientViewModel Client { get; set; }

        [DisplayName("Auftragsart")]
        public TicketTypes TicketType { get; set; }

        [DisplayName("Status")]
        public TicketStatuses TicketStatus { get; set; }

        public IList<DeviceViewModel> Devices { get; set; }

        public DeviceViewModel Device { get; set; }

        public static implicit operator Ticket(TicketViewModel viewModel)
        {
            var ticket = new Ticket
            {
                Client = viewModel.Client,
                Name = viewModel.Name,
                WorkOrder = viewModel.WorkOrder,
                DoBackup = viewModel.DoBackup,
                DataBackupByClient = viewModel.DataBackupByClient,
                DataBackupByStaff = viewModel.DataBackupByStaff,
                DataBackupDone = viewModel.DataBackupDone,
                Devices = new List<Device>()
            };

            if (viewModel.Devices != null)
            {
                foreach (var device in viewModel.Devices)
                {
                    ticket.Devices.Add(device);
                }
            }

            return ticket;
        }

        public Ticket CopyForUpdate()
        {
            var ticket = new Ticket
            {
                Id = this.Id,
                Client = this.Client.CopyForUpdate(),
                Name = this.Name,
                WorkOrder = this.WorkOrder,
                DoBackup = this.DoBackup,
                DataBackupByClient = this.DataBackupByClient,
                DataBackupByStaff = this.DataBackupByStaff,
                DataBackupDone = this.DataBackupDone,
                Devices = new List<Device>()
            };

            if (this.Devices != null)
            {
                foreach (var deviceViewModel in this.Devices)
                {
                    Device device = deviceViewModel.CopyForUpdate();
                    ticket.Devices.Add(device);
                }
            }

            return ticket;
        }

        public string BackupChoices { get; set; }
    }
}
