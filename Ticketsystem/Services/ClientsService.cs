using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Services
{
    public class ClientsService
    {
        private TicketsystemContext _ticketsystemContext;

        public ClientsService(TicketsystemContext ticketsystemContext)
        {
            _ticketsystemContext = ticketsystemContext;
        }

        private IQueryable<Client> GetClientsShared(ClientData clientData)
        {
            IQueryable<Client> query = _ticketsystemContext.Clients;

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

        public async Task<List<Client>> GetAllClients(ClientData clientData)
        {
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

            return await query.Skip(clientData.Skip).Take(clientData.Take).ToListAsync();
        }
    }
}
