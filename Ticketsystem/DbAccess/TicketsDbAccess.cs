using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Newtonsoft.Json;
using Ticketsystem.ViewModels;
using Ticketsystem.Enums;
using Ticketsystem.Extensions;
using Ticketsystem.Utilities;

namespace Ticketsystem.DbAccess
{
    public class TicketsDbAccess : ITicketsClientsDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;
        private readonly IDistributedCache _cache;
        private readonly Globals _globals;

        public TicketsDbAccess(
            TicketsystemContext ticketsystemContext,
            IDistributedCache cache,
            Globals globals)
        {
            _ticketsystemContext = ticketsystemContext;
            _cache = cache;
            _globals = globals;
        }

        private IQueryable<Ticket> GetTicketsShared(TicketFilterData ticketData)
        {
            IQueryable<Ticket> query = _ticketsystemContext.Tickets
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Select(t => new Ticket
                {
                    Id = t.Id,
                    Name = t.Name,
                    OrderDate = t.OrderDate,
                    TicketStatus = t.TicketStatus,
                    TicketType = t.TicketType,
                    Client = new Client
                    {
                        LastName = t.Client.LastName,
                    }
                });

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
                    query = query.Where(t => t.OrderDate >= startDate.ToUniversalTime()).Where(t => t.OrderDate <= endDate.ToUniversalTime());
                }
            }

            return query;
        }

        public async Task Add<T>(T ticket) where T : class
        {
            _ticketsystemContext.Add(ticket);
            await _ticketsystemContext.SaveChangesAsync();

            if (_globals.EnableRedisCache)
            {
                await RedisCacheUtility.DeleteCacheEntriesByPrefix(_globals.RedisServer, _globals.RedisTicketsCache);
            }
        }

        public async Task<List<T>> GetAll<T>(IFilterData data) where T : class
        {
            var ticketData = data as TicketFilterData;

            string cacheKey = $"tickets_{ticketData.FilterByTicketId}_{ticketData.FilterByTicketName}_{ticketData.FilterByClientName}_{ticketData.FilterByTicketStatus}_{ticketData.FilterByTicketType}_{ticketData.FilterByStartDate}_{ticketData.FilterByEndDate}_{ticketData.SortBy}_{ticketData.DoReverse}_{ticketData.Skip}_{ticketData.Take}";
            if (_globals.EnableRedisCache)
            {
                var cachedTickets = await _cache.GetAsync(cacheKey);
                if (cachedTickets != null)
                {
                    return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(cachedTickets));
                }
            }

            var query = GetTicketsShared(ticketData);

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

            query = query.Skip(ticketData.Skip).Take(ticketData.Take);

            var tickets = await query.ToListAsync();

            List<T> list = new();

            foreach (var ticket in tickets)
            {
                TicketIndexViewModel ticketIndexViewModel = new()
                {
                    Id = ticket.Id,
                    Name = ticket.Name,
                    OrderDate = ticket.OrderDate.ToLocalTime(),
                    TicketType = Enum.GetValues<TicketTypes>().FirstOrDefault(tt => tt.ToString() == ticket.TicketType.Name).GetText(),
                    TicketStatus = Enum.GetValues<TicketStatuses>().FirstOrDefault(tt => tt.ToString() == ticket.TicketStatus.Name).GetText(),
                    ClientLastName = ticket.Client.LastName,
                };

                list.Add(ticketIndexViewModel as T);
            }

            if (_globals.EnableRedisCache)
            {
                await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list)), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return list;
        }

        public async Task<T> GetById<T, TT>(TT id) where T : class
        {
            int ticketId = (int)(object)id;

            var ticket = await _ticketsystemContext.Tickets
                .Include(t => t.Client)
                .Include(t => t.Devices).ThenInclude(d => d.Software)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == ticketId);

            var ticketChanges = _ticketsystemContext.TicketChanges
                .Where(tc => tc.TicketId == ticketId)
                .Include(tc => tc.User)
                .Include(tc => tc.OldTicketStatus)
                .Include(tc => tc.NewTicketStatus)
                .OrderBy(tc => tc.ChangeDate).Reverse();

            ticket.TicketChanges = await ticketChanges.ToListAsync();

            return ticket as T;
        }

        public int GetCount(IFilterData data)
        {
            return GetTicketsShared(data as TicketFilterData).Count();
        }

        public async Task Delete<T>(T entity) where T : class
        {
            if (entity is Ticket ticket)
            {
                _ticketsystemContext.Tickets.Remove(ticket);
            }

            await _ticketsystemContext.SaveChangesAsync();

            if (_globals.EnableRedisCache)
            {
                await RedisCacheUtility.DeleteCacheEntriesByPrefix(_globals.RedisServer, _globals.RedisTicketsCache);
            }
        }

        public async Task Update<T>(T entity) where T : class
        {
            var ticket = entity as Ticket;
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

            if (_globals.EnableRedisCache)
            {
                await RedisCacheUtility.DeleteCacheEntriesByPrefix(_globals.RedisServer, _globals.RedisTicketsCache);
            }
        }
    }
}
