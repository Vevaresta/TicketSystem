using Ticketsystem.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Services
{
    public class TicketChangesService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketChangesService(TicketsystemContext ticketsystemContext)
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
