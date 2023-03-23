using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Services
{
    public class TicketsService
    {
        private readonly TicketsystemContext _ticketsystemContext;

        public TicketsService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        private IQueryable<Ticket> GetTicketsShared(TicketData ticketData)
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType);

            if (!string.IsNullOrEmpty(ticketData.FilterByTicketId))
            {
                query = query.Where(t => t.Id == int.Parse(ticketData.FilterByTicketId));
            }
            if (!string.IsNullOrEmpty(ticketData.FilterByTicketName))
            {
                query = query.Where(t => t.Name.ToLower().Contains(ticketData.FilterByTicketName.ToLower()));
            }
            if (!string.IsNullOrEmpty(ticketData.FilterByClientName))
            {
                query = query.Where(t => t.Client.LastName.ToLower().Contains(ticketData.FilterByClientName.ToLower()));
            }
            if (!string.IsNullOrEmpty(ticketData.FilterByTicketStatus))
            {
                query = query.Where(t => t.TicketStatus.Name == ticketData.FilterByTicketStatus);
            }
            if (!string.IsNullOrEmpty(ticketData.FilterByTicketType))
            {
                query = query.Where(t => t.TicketType.Name == ticketData.FilterByTicketType);
            }

            if (!string.IsNullOrEmpty(ticketData.FilterByStartDate) && string.IsNullOrEmpty(ticketData.FilterByEndDate))
            {
                ticketData.FilterByEndDate = ticketData.FilterByStartDate;
            }
            else if (string.IsNullOrEmpty(ticketData.FilterByStartDate) && !string.IsNullOrEmpty(ticketData.FilterByEndDate))
            {
                ticketData.FilterByStartDate = ticketData.FilterByEndDate;
            }

            if (!string.IsNullOrEmpty(ticketData.FilterByStartDate) && !string.IsNullOrEmpty(ticketData.FilterByEndDate))
            {
                bool areDatesValid = true;
                areDatesValid = DateTime.TryParse(ticketData.FilterByStartDate, out DateTime startDate);
                areDatesValid = DateTime.TryParse(ticketData.FilterByEndDate, out DateTime endDate);
                if (areDatesValid)
                {
                    query = query.Where(t => t.OrderDate >= startDate).Where(t => t.OrderDate <= endDate);
                }
            }

            return query;
        }

        public int GetTicketsCount(TicketData ticketData)
        {
            return GetTicketsShared(ticketData).Count();
        }

        public async Task<IList<Ticket>> GetAllTickets(TicketData ticketData)
        {
            IQueryable<Ticket> query = GetTicketsShared(ticketData);

            query = ticketData.SortBy switch
            {
                "Id" => query.OrderBy(t => t.Id),
                "Name" => query.OrderBy(t => t.Name),
                "LastName" => query.OrderBy(t => t.Client.LastName),
                "OrderDate" => query.OrderBy(t => t.OrderDate),
                "TicketType" => query.OrderBy(t => t.TicketType.Name),
                "TicketStatus" => query.OrderBy(t => t.TicketStatus.Name),
                _ => query.OrderBy(t => t.OrderDate),
            };

            if (ticketData.DoReverse)
            {
                query = query.Reverse();
            }

            return await query.Skip(ticketData.Skip).Take(ticketData.Take).ToListAsync();
        }

        public async Task<Ticket> GetTicketById(int id)
        {
            var ticket = await _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.TicketChanges).ThenInclude(tc => tc.User)
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
            var devices = ticket.Devices;

            if (devices != null)
            {
                foreach (Device device in devices)
                {
                    if (device.Software != null)
                    {
                        foreach (Software software in device.Software)
                        {
                            if (string.IsNullOrEmpty(software.Id))
                            {
                                software.Id = Guid.NewGuid().ToString();
                                _ticketsystemContext.Add(software);
                            }
                            else
                            {
                                _ticketsystemContext.Update(software);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(device.Id))
                    {
                        device.Id = Guid.NewGuid().ToString();
                        _ticketsystemContext.Add(device);
                    }
                    else
                    {
                        _ticketsystemContext.Update(device);
                    }
                }
            }

            List<Device> devicesInDb = await _ticketsystemContext.Devices.Where(d => d.TicketId == ticket.Id).AsNoTracking().ToListAsync();
            foreach (var deviceInDb in devicesInDb)
            {
                if (!ticket.Devices.Any(d => d.Id == deviceInDb.Id))
                {
                    _ticketsystemContext.Remove(deviceInDb);
                }
            }

            if (ticket.Devices != null)
            {
                foreach (var device in ticket.Devices)
                {
                    List<Software> softwarePerDeviceInDb = await _ticketsystemContext.Software.Where(s => s.DeviceId == device.Id).AsNoTracking().ToListAsync();
                    foreach (var software in softwarePerDeviceInDb)
                    {
                        if (!device.Software.Any(s => s.Id == software.Id))
                        {
                            _ticketsystemContext.Remove(software);
                        }
                    }
                }
            }

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
