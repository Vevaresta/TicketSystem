using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class TicketTypesService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketTypesService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task<TicketType> GetTicketTypeByName(string ticketType) => await _ticketsystemContext.TicketTypes.FirstOrDefaultAsync(t => t.Name == ticketType);
    }
}
