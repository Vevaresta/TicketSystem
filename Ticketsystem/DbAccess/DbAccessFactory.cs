using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Distributed;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class DbAccessFactory : IDbAccessFactory
    {
        private readonly TicketsystemContext _ticketsystemContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IDistributedCache _cache;
        private readonly Globals _globals; 

        public DbAccessFactory(
            TicketsystemContext ticketsystemContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IDistributedCache cache,
            Globals globals
            )
        {
            _ticketsystemContext = ticketsystemContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _cache = cache;
            _globals = globals;
        }

        public IDbAccess GetTicketsClientsDbAccess<T>()
        {
            if (typeof(T) == typeof(ClientsDbAccess))
            {
                return new ClientsDbAccess(_ticketsystemContext, _cache, _globals);
            }
            else if (typeof(T) == typeof(TicketsDbAccess))
            {
                return new TicketsDbAccess(_ticketsystemContext, _cache, _globals);
            }
            else
            {
                throw new Exception("Wrong DbAccess type!");
            }
        }

        public RolesDbAccess RolesDbAccess { get => new(_userManager, _roleManager); }
        public RolePermissionsDbAccess RolePermissionsDbAccess { get => new(_ticketsystemContext, _userManager, _roleManager); }
        public RolesToDisplayDbAccess RolesToDisplayDbAccess { get => new(_roleManager); }
        public TicketTypesDbAccess TicketTypesDbAccess { get => new(_ticketsystemContext); }
        public TicketStatusesDbAccess TicketStatusesDbAccess { get => new(_ticketsystemContext); }
        public TicketChangesDbAccess TicketChangesDbAccess { get => new(_ticketsystemContext); }
    }
}
