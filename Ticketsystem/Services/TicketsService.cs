using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ticketsystem.Services
{
    public class TicketsService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketsService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        public async Task<int> GetTicketsCount(string id = "", string lastName = "")
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType).AsSplitQuery();

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(t => t.Id == int.Parse(id));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(t => t.Client.LastName == lastName);
            }

            return (await query.ToListAsync()).Count();
        }

        public async Task<IList<Ticket>> GetAllTickets()
        {
            return await _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .AsSplitQuery().ToListAsync();
        }

        public async Task<IList<Ticket>> GetAllTickets(string id, string lastName, int take, int skip, string sortByAttribute, bool doReverse)
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType);

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(t => t.Id == int.Parse(id));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(t => t.Client.LastName == lastName);
            }

            query = sortByAttribute switch
            {
                "Id" => query.OrderBy(t => t.Id),
                "Name" => query.OrderBy(t => t.Name),
                "LastName" => query.OrderBy(t => t.Client.LastName),
                "OrderDate" => query.OrderBy(t => t.OrderDate),
                "TicketType" => query.OrderBy(t => t.TicketType.Name),
                "TicketStatus" => query.OrderBy(t => t.TicketStatus.Name),
                _ => query.OrderBy(t => t.OrderDate),
            };

            if (doReverse)
            {
                query = query.Reverse();
            }

            return await query.Skip(skip).Take(take).AsSplitQuery().ToListAsync();
        }


        public async Task<Ticket> GetTicketById(int id)
        {
            var ticket = await _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);

            return ticket;
        }

        public async Task AddTicket(Ticket ticket)
        {
            _ticketsystemContext.Add(ticket);
            await _ticketsystemContext.SaveChangesAsync();
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            _ticketsystemContext.Update(ticket);
            await _ticketsystemContext.SaveChangesAsync();
        }

        public async Task DeleteTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                _ticketsystemContext.Tickets.Remove(ticket);
            }

            await _ticketsystemContext.SaveChangesAsync();
        }
    }
}
