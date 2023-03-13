using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class TicketStatusesService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketStatusesService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task<TicketStatus> GetTicketStatusByName(string ticketStatus) => await _ticketsystemContext.TicketStatuses.FirstOrDefaultAsync(t => t.Name == ticketStatus);
    }
}
