using Ticketsystem.Data;
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

        public List<Client> GetAllClients()
        {
            return _ticketsystemContext.Clients.ToList();
        }
    }
}
