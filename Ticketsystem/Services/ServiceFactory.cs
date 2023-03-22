using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly TicketsystemContext _ticketsystemContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ServiceFactory(
            TicketsystemContext ticketsystemContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
            )
        {
            _ticketsystemContext = ticketsystemContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public RolesService GetRolesService() => new(_userManager, _roleManager);
        public RolePermissionsService GetRolePermissionsService() => new(_ticketsystemContext, _userManager, _roleManager);
        public RolesToDisplayService GetRolesToDisplayService() => new(_roleManager);
        public TicketsService GetTicketsService() => new(_ticketsystemContext);
        public TicketTypesService GetTicketTypesService() => new(_ticketsystemContext);
        public TicketStatusesService GetTicketStatusesService() => new(_ticketsystemContext);
        public ClientsService GetClientsService() => new(_ticketsystemContext);
    }
}
