using Ticketsystem.Data;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class ChangeTicketStatus
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public ChangeTicketStatus(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task Change(Ticket ticket, TicketStatus ticketStatus)
        {

        }
    }
}
