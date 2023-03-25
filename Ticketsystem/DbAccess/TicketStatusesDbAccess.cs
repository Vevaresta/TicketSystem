using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class TicketStatusesDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketStatusesDbAccess(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task<TicketStatus> GetTicketStatusByName(string ticketStatus) => await _ticketsystemContext.TicketStatuses.FirstOrDefaultAsync(t => t.Name == ticketStatus);
    }
}
