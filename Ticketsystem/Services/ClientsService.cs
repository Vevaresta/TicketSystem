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
                query = query.Where(t => t.LastName == clientData.FilterByLastName);
            }
            if (!string.IsNullOrEmpty(clientData.FilterByFirstName))
            {
                query = query.Where(t => t.FirstName == clientData.FilterByFirstName);
            }
            if (!string.IsNullOrEmpty(clientData.FilterByEmail))
            {
                query = query.Where(t => t.Email == clientData.FilterByEmail);
            }

            return query;
        }

        public async Task<List<Client>> GetAllClients(ClientData clientData)
        {
            IQueryable<Client> query = GetClientsShared(clientData);
            return await query.Take(clientData.Take).Skip(clientData.Skip).ToListAsync();
        }
    }
}
