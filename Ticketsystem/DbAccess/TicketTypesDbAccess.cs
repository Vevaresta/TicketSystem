using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class TicketTypesDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketTypesDbAccess(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task<TicketType> GetTicketTypeByName(string ticketType)
        {
            //TicketTypes ticketTypeEnum = TicketTypeTexts.Texts.Keys.FirstOrDefault(k => TicketTypeTexts.Texts[k] == ticketType);
            return await _ticketsystemContext.TicketTypes.FirstOrDefaultAsync(t => t.Name == ticketType.ToString());
        }
    }
}
