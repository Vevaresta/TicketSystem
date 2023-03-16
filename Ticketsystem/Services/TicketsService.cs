using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ticketsystem.Data;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class TicketsService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketsService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        private IQueryable<Ticket> GetTicketsShared(TicketQuery queryModel)
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType).AsSplitQuery();

            if (!string.IsNullOrEmpty(queryModel.FilterByTicketId))
            {
                query = query.Where(t => t.Id == int.Parse(queryModel.FilterByTicketId));
            }
            if (!string.IsNullOrEmpty(queryModel.FilterByTicketName))
            {
                query = query.Where(t => t.Name == queryModel.FilterByTicketName);
            }
            if (!string.IsNullOrEmpty(queryModel.FilterByTicketStatus))
            {
                query = query.Where(t => t.TicketStatus.Name == queryModel.FilterByTicketStatus);
            }
            if (!string.IsNullOrEmpty(queryModel.FilterByClientName))
            {
                query = query.Where(t => t.Client.LastName == queryModel.FilterByClientName);
            }
            if (!string.IsNullOrEmpty(queryModel.FilterByTicketType))
            {
                query = query.Where(t => t.TicketType.Name == queryModel.FilterByTicketType);
            }

            if (!string.IsNullOrEmpty(queryModel.FilterByStartDate) && string.IsNullOrEmpty(queryModel.FilterByEndDate))
            {
                queryModel.FilterByEndDate = queryModel.FilterByStartDate;
            }
            else if (string.IsNullOrEmpty(queryModel.FilterByStartDate) && !string.IsNullOrEmpty(queryModel.FilterByEndDate))
            {
                queryModel.FilterByStartDate = queryModel.FilterByEndDate;
            }

            if (!string.IsNullOrEmpty(queryModel.FilterByStartDate) && !string.IsNullOrEmpty(queryModel.FilterByEndDate))
            {
                bool areDatesValid = true;
                areDatesValid = DateTime.TryParse(queryModel.FilterByStartDate, out DateTime startDate);
                areDatesValid = DateTime.TryParse(queryModel.FilterByEndDate, out DateTime endDate);
                if (areDatesValid)
                {
                    query = query.Where(t => t.OrderDate.Date >= startDate).Where(t => t.OrderDate.Date <= endDate);
                }
            }

            return query;
        }

        public int GetTicketsCount(TicketQuery queryModel)
        {
            return GetTicketsShared(queryModel).Count();
        }

        public async Task<IList<Ticket>> GetAllTickets(TicketQuery queryModel)
        {
            IQueryable<Ticket> query = GetTicketsShared(queryModel);

            query = queryModel.SortByAttribute switch
            {
                "Id" => query.OrderBy(t => t.Id),
                "Name" => query.OrderBy(t => t.Name),
                "LastName" => query.OrderBy(t => t.Client.LastName),
                "OrderDate" => query.OrderBy(t => t.OrderDate),
                "FilterByTicketType" => query.OrderBy(t => t.TicketType.Name),
                "FilterByTicketStatus" => query.OrderBy(t => t.TicketStatus.Name),
                _ => query.OrderBy(t => t.OrderDate),
            };

            if (queryModel.DoReverse)
            {
                query = query.Reverse();
            }

            return await query.Skip(queryModel.Skip).Take(queryModel.Take).AsSplitQuery().ToListAsync();
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
