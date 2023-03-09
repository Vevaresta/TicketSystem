using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ticketsystem.Models;

namespace Ticketsystem.ViewModels
{
    public class TicketViewModel
    {
        [Required(ErrorMessage = "Dies ist ein Plichtfeld")]
        [DisplayName("Ticketname")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dies ist ein Plichtfeld")]
        [DisplayName("Arbeitsanweisung")]
        public string WorkOrder { get; set; }

        [DisplayName("Notizen")]
        public string Comments { get; set; }

        public bool DataBackupByClient { get; set; }

        public bool DataBackupByStaff { get; set; }

        public bool DataBackupDone { get; set; }
        public ClientViewModel Client { get; set; }

        public IList<DeviceViewModel> Devices { get; set; }

        // Defining an implicit conversion between the ticket model and and ticket viewmodel
        // This enables the following:
        // TicketViewModel vm = new();
        // Ticket ticket = vm;
        // Uses the implicit operator keyword:
        public static implicit operator Ticket(TicketViewModel viewModel)
        {
            var ticket = new Ticket
            {
                // Properties are alligned with theire counterparts
                Client = viewModel.Client,
                Name = viewModel.Name,
                WorkOrder = viewModel.WorkOrder,
                Comments = viewModel.Comments,
                DataBackupByClient = viewModel.DataBackupByClient,
                DataBackupByStaff = viewModel.DataBackupByStaff,
                DataBackupDone = viewModel.DataBackupDone,
                Devices = new List<Device>()
            };

            if (viewModel.Devices != null )
            {
                foreach ( var device in viewModel.Devices )
                {
                    ticket.Devices.Add(device);
                }
            }

            return ticket;
        }
    }
}
