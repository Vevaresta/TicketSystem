using Ticketsystem.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class TicketChangesDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketChangesDbAccess(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task AddTicketChange(TicketChange ticketChange)
        {
            _ticketsystemContext.Add(ticketChange);
            await _ticketsystemContext.SaveChangesAsync();
        }
    }
}
