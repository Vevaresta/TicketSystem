using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Ticketsystem.ViewModels;

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

        private IQueryable<Client> GetClientsShared(ClientData clientData)
        {
            IQueryable<Client> query = _ticketsystemContext.Clients
            .Where(c => c.Id != "Fallback");

            if (!string.IsNullOrEmpty(clientData.FilterByLastName))
            {
                query = query.Where(t => t.LastName.ToLower().Contains(clientData.FilterByLastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(clientData.FilterByFirstName))
            {
                query = query.Where(t => t.FirstName.ToLower().Contains(clientData.FilterByFirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(clientData.FilterByEmail))
            {
                query = query.Where(t => t.Email.ToLower().Contains(clientData.FilterByEmail.ToLower()));
            }

            return query;
        }

        public int GetClientsCount(ClientData clientData)
        {
            return GetClientsShared(clientData).Count();
        }

        public async Task<List<ClientIndexViewModel>> GetAllClients(ClientData clientData)
        {
            string cacheKey = "";
            if (_globals.EnableRedisCache)
            {
                cacheKey = $"tickets_{clientData.FilterByFirstName}_{clientData.FilterByLastName}_{clientData.FilterByEmail}_{clientData.SortBy}_{clientData.Take}_{clientData.Skip}_{clientData.DoReverse}";
                var cachedClients = await _cache.GetAsync(cacheKey);
                if (cachedClients != null)
                {
                    return JsonConvert.DeserializeObject<List<ClientIndexViewModel>>(Encoding.UTF8.GetString(cachedClients));
                }
            }

            IQueryable<Client> query = GetClientsShared(clientData);

            query = clientData.SortBy switch
            {
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

            List<ClientIndexViewModel> list = new();

            foreach (var client in clients)
            {
                list.Add(new ClientIndexViewModel
                {
                    Id = client.Id,
                    LastName = client.LastName,
                    FirstName = client.FirstName,
                    Email = client.Email
                });
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

        public async Task<Client> GetClientById(string id)
        {
            return await _ticketsystemContext.Clients.FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task DeleteClient(Client client)
        {
            if (client != null)
            {
                _ticketsystemContext.Clients.Remove(client);
            }

            var clientTickets = _ticketsystemContext.Tickets.Where(t => t.Client.Id == client.Id);

            if (clientTickets != null)
            {
                var fallbackClient = await _ticketsystemContext.Clients.FirstOrDefaultAsync(c => c.Id == "Fallback");
                foreach (var ticket in clientTickets)
                {
                    ticket.Client = fallbackClient;
                }
            }

            await _ticketsystemContext.SaveChangesAsync();
        }

        public async Task UpdateClient(Client client)
        {
            _ticketsystemContext.Update(client);
            await _ticketsystemContext.SaveChangesAsync();
        }
    }
}
