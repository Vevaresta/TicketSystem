using Microsoft.AspNetCore.Identity;
using Ticketsystem.Data;

namespace Ticketsystem.Services
{
    public interface IServiceFactory
    {
        public RolePermissionsService GetRolePermissionsService();
        public RolesService GetRolesService();
        public RolesToDisplayService GetRolesToDisplayService();
        public TicketsService GetTicketsService();
        public TicketTypesService GetTicketTypesService();
        public TicketStatusesService GetTicketStatusesService();
        public TicketChangesService GetTicketChangesService();
        public ClientsService GetClientsService();
    }
}
