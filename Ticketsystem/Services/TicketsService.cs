using Microsoft.EntityFrameworkCore;
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

        private IQueryable<Ticket> GetTicketsShared(
            string filterByTicketId,
            string filterByTicketName,
            string filterByTicketStatus,
            string filterByClientName,
            string filterByStartDate,
            string filterByEndDate,
            string filterByTicketType
            )
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType).AsSplitQuery();

            if (!string.IsNullOrEmpty(filterByTicketId))
            {
                query = query.Where(t => t.Id == int.Parse(filterByTicketId));
            }
            if (!string.IsNullOrEmpty(filterByTicketName))
            {
                query = query.Where(t => t.Name == filterByTicketName);
            }
            if (!string.IsNullOrEmpty(filterByTicketStatus))
            {
                query = query.Where(t => t.TicketStatus.Name == filterByTicketStatus);
            }
            if (!string.IsNullOrEmpty(filterByClientName))
            {
                query = query.Where(t => t.Client.LastName == filterByClientName);
            }
            if (!string.IsNullOrEmpty(filterByTicketType))
            {
                query = query.Where(t => t.TicketType.Name == filterByTicketType);
            }

            if (!string.IsNullOrEmpty(filterByStartDate) && string.IsNullOrEmpty(filterByEndDate))
            {
                filterByEndDate = filterByStartDate;
            }
            else if (string.IsNullOrEmpty(filterByStartDate) && !string.IsNullOrEmpty(filterByEndDate))
            {
                filterByStartDate = filterByEndDate;
            }

            if (!string.IsNullOrEmpty(filterByStartDate) && !string.IsNullOrEmpty(filterByEndDate))
            {
                bool areDatesValid = true;
                areDatesValid = DateTime.TryParse(filterByStartDate, out DateTime startDate);
                areDatesValid = DateTime.TryParse(filterByEndDate, out DateTime endDate);
                if (areDatesValid)
                {
                    query = query.Where(t => t.OrderDate.Date >= startDate).Where(t => t.OrderDate.Date <= endDate);
                }
            }

            return query;
        }

        public int GetTicketsCount(
            string filterByTicketId,
            string filterByTicketName,
            string filterByTicketStatus,
            string filterByClientName,
            string filterByStartDate,
            string filterByEndDate,
            string filterByTicketType
            )
        {
            return GetTicketsShared(
            filterByTicketId,
            filterByTicketName,
            filterByTicketStatus,
            filterByClientName,
            filterByStartDate,
            filterByEndDate,
            filterByTicketType
            ).Count();
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

        public async Task<IList<Ticket>> GetAllTickets(
            int take,
            int skip,
            string sortByAttribute,
            bool doReverse,
            string filterByTicketId,
            string filterByTicketName,
            string filterByTicketStatus,
            string filterByClientName,
            string filterByStartDate,
            string filterByEndDate,
            string filterByTicketType
            )
        {
            IQueryable<Ticket> query = GetTicketsShared(
                filterByTicketId,
                filterByTicketName,
                filterByTicketStatus,
                filterByClientName,
                filterByStartDate,
                filterByEndDate,
                filterByTicketType
            );

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
