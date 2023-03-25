using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels
{
    public class TicketChangeViewModel
    {
        public string Id { get; set; }

        public DateTime ChangeDate { get; set; }

        [DisplayName("Änderungsbeschreibung")]
        [Required(ErrorMessage = "Pflichtfeld")]
        public string Comment { get; set; }

        public virtual UserViewModel User { get; set; }
        public virtual TicketViewModel Ticket { get; set; }
        public virtual TicketStatus OldTicketStatus { get; set; }
        public virtual TicketStatus NewTicketStatus { get; set; }

        public static implicit operator TicketChange(TicketChangeViewModel viewModel)
        {
            return new TicketChange
            {
                Comment = viewModel.Comment,
                UserId = viewModel.User.Id,
                TicketId = viewModel.Ticket.Id,
                ChangeDate = viewModel.ChangeDate,
                OldTicketStatus = viewModel.OldTicketStatus,
                NewTicketStatus = viewModel.NewTicketStatus
            };
        }
    }
}
