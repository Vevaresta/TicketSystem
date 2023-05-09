using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [DisplayName("Tickettitel")]
        public string Name { get; set; }

        [DisplayName("Auftragsdatum")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [DisplayName("Arbeitsanweisung*")]
        public string WorkOrder { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        public bool DoBackup { get; set; }

        [DisplayName("Virus-Quarantäne?")]
        public bool IsVirus { get; set; }

        [Display(Name = "Bereits erledigt durch Kunde")]
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
        public IList<TicketChangeViewModel> TicketChanges { get; set; }

        public DeviceViewModel Device { get; set; }
        public TicketChangeViewModel TicketChange { get; set; }

        public bool DoSendEmail { get; set; }

        public bool IsPdfSigned { get; set; }

        public static implicit operator Ticket(TicketViewModel viewModel)
        {
            var ticket = new Ticket
            {
                Client = viewModel.Client,
                Name = viewModel.Name,
                OrderDate = viewModel.OrderDate.ToUniversalTime(),
                WorkOrder = viewModel.WorkOrder,
                DoBackup = viewModel.DoBackup,
                IsVirus = viewModel.IsVirus,
                DataBackupByClient = viewModel.DataBackupByClient,
                DataBackupByStaff = viewModel.DataBackupByStaff,
                DataBackupDone = viewModel.DataBackupDone,
                Devices = new List<Device>(),
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
                Id = Id,
                Client = Client.CopyForUpdate(),
                Name = Name,
                OrderDate = OrderDate.ToUniversalTime(),
                WorkOrder = WorkOrder,
                DoBackup = DoBackup,
                IsVirus = IsVirus,
                DataBackupByClient = DataBackupByClient,
                DataBackupByStaff = DataBackupByStaff,
                DataBackupDone = DataBackupDone,
                Devices = new List<Device>(),
                TicketChanges = new List<TicketChange>()
            };

            if (Devices != null)
            {
                foreach (var deviceViewModel in Devices)
                {
                    Device device = deviceViewModel.CopyForUpdate();
                    ticket.Devices.Add(device);
                }
            }

            if (TicketChanges != null)
            {
                foreach (var ticketChange in TicketChanges)
                {
                    ticket.TicketChanges.Add(ticketChange);
                }
            }

            return ticket;
        }

        public string BackupChoices { get; set; }
    }
}
