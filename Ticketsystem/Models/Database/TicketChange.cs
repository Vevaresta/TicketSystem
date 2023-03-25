using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models.Database
{
    public class TicketChange
    {
        public TicketChange()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }


        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }


        [ForeignKey(nameof(OldTicketStatus))]
        public string OldTicketStatusId { get; set; }


        [ForeignKey(nameof(NewTicketStatus))]
        public string NewTicketStatusId { get; set; }

        public DateTime ChangeDate { get; set; }
        public string Comment { get; set; }

        public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual TicketStatus OldTicketStatus { get; set; }
        public virtual TicketStatus NewTicketStatus { get; set; }

        public static implicit operator TicketChangeViewModel(TicketChange ticketChange)
        {
            return new TicketChangeViewModel
            {
                Id = ticketChange.Id,
                ChangeDate = ticketChange.ChangeDate.ToLocalTime(),
                Comment = ticketChange.Comment,
                User = ticketChange.User,
                Ticket = ticketChange.Ticket,
                OldTicketStatus = ticketChange.OldTicketStatus,
                NewTicketStatus = ticketChange.NewTicketStatus
            };
        }
    }
}
