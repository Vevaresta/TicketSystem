using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.ViewModels;
using Ticketsystem.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Ticketsystem.DbAccess
{
    public class ClientsDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;
        private readonly IDistributedCache _cache;
        private readonly Globals _globals;

        public ClientsDbAccess(
            TicketsystemContext ticketsystemContext,
            IDistributedCache cache,
            Globals globals)
        {
            _ticketsystemContext = ticketsystemContext;
            _cache = cache;
            _globals = globals;
        }

        private IQueryable<Client> GetClientsShared(ClientFilterData clientData)
        {
            IQueryable<Client> query = _ticketsystemContext.Clients
            .Where(c => c.Id != -1);

            if (clientData.FilterById != null)
            {
                query = query.Where(t => t.Id == clientData.FilterById);
            }
            if (!string.IsNullOrEmpty(clientData.FilterByLastName))
            {
                query = query.Where(t => t.LastName.ToLower().Contains(clientData.FilterByLastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(clientData.FilterByEmail))
            {
                query = query.Where(t => t.Email.ToLower().Contains(clientData.FilterByEmail.ToLower()));
            }

            return query;
        }

        public async Task<List<T>> GetAll<T>(IFilterData data) where T : class
        {
            var clientData = data as ClientFilterData;

            string cacheKey = $"clients_{clientData.FilterById}_{clientData.FilterByLastName}_{clientData.FilterByEmail}_{clientData.SortBy}_{clientData.Take}_{clientData.Skip}_{clientData.DoReverse}";
            if (_globals.EnableRedisCache)
            {

                var cachedClients = await _cache.GetAsync(cacheKey);
                if (cachedClients != null)
                {
                    return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(cachedClients));
                }
            }

            IQueryable<Client> query = GetClientsShared(clientData);

            query = clientData.SortBy switch
            {
                "Id" => query.OrderBy(t => t.Id),
                "LastName" => query.OrderBy(t => t.LastName),
                "FirstName" => query.OrderBy(t => t.FirstName),
                "Email" => query.OrderBy(t => t.Email),
                _ => query.OrderBy(t => t.LastName),
            };

            if (clientData.DoReverse)
            {
                query = query.Reverse();
            }

            query = query.Skip(clientData.Skip).Take(clientData.Take);

            var clients = await query.ToListAsync();

            List<T> list = new();

            foreach (var client in clients)
            {
                ClientIndexViewModel item = new()
                {
                    Id = client.Id,
                    LastName = client.LastName,
                    FirstName = client.FirstName,
                    Email = client.Email
                };
                list.Add(item as T);
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
            int clientId = int.Parse(id as string);

            Client client = await _ticketsystemContext.Clients.FirstOrDefaultAsync(m => m.Id == clientId);

            return client as T;
        }

        public int GetCount(IFilterData data)
        {
            return GetClientsShared(data as ClientFilterData).Count();
        }

        public Task Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task Update<T>(T entity) where T : class
        {
            _ticketsystemContext.Update(entity as Client);
            await _ticketsystemContext.SaveChangesAsync();

            if (_globals.EnableRedisCache)
            {
                await RedisCacheUtility.FlushDb(_globals.RedisServer);
            }
        }

        public async Task Delete<T>(T entity) where T : class
        {
            var client = entity as Client;

            if (client != null)
            {
                _ticketsystemContext.Clients.Remove(client);
            }

            var clientTickets = _ticketsystemContext.Tickets.Where(t => t.Client.Id == client.Id);

            if (clientTickets != null)
            {
                var fallbackClient = await _ticketsystemContext.Clients.FirstOrDefaultAsync(c => c.Id == -1);

                foreach (var ticket in clientTickets)
                {
                    ticket.Client = fallbackClient;
                }
            }

            await _ticketsystemContext.SaveChangesAsync();

            if (_globals.EnableRedisCache)
            {
                await RedisCacheUtility.FlushDb(_globals.RedisServer);
            }
        }
    }
}
