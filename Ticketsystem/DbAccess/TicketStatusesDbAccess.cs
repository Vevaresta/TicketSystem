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

        public async Task<int[]> GetTicketStatusPercentages()
        {
            int[] statuses = new int[3];

            statuses[0] = await _ticketsystemContext.Tickets.Include(t => t.TicketStatus).Where(t => t.TicketStatus.Name == "Open").CountAsync();
            statuses[1] = await _ticketsystemContext.Tickets.Include(t => t.TicketStatus).Where(t => t.TicketStatus.Name == "InProgress").CountAsync();
            statuses[2] = await _ticketsystemContext.Tickets.Include(t => t.TicketStatus).Where(t => t.TicketStatus.Name == "Closed").CountAsync();

            int statsum = statuses.Sum();

            int[] statpersent = new int[3];

            if (statsum == 0)
            {
                statpersent[0] = 100;
                statpersent[1] = 0;
                statpersent[2] = 0;
            }
            else
            {
                statpersent[0] = statuses[0] * 100 / statsum;
                statpersent[1] = statuses[1] * 100 / statsum;
                statpersent[2] = statuses[2] * 100 / statsum;
            }

            if (statpersent.Sum() != 100)
            {
                statpersent[2] += 1;
            }


            return statpersent;
        }
    }
}
