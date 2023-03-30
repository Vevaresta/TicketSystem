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

        public T GetDbAccess<T>() where T : class
        {
            if (typeof(T) == typeof(ClientsDbAccess))
            {
                return new ClientsDbAccess(_ticketsystemContext, _cache, _globals) as T;
            }
            else if (typeof(T) == typeof(TicketsDbAccess))
            {
                return new TicketsDbAccess(_ticketsystemContext, _cache, _globals) as T;
            }
            else if (typeof(T) == typeof(RolePermissionsDbAccess))
            {
                return new RolePermissionsDbAccess(_ticketsystemContext, _userManager, _roleManager) as T;
            }
            else if (typeof(T) == typeof(RolesToDisplayDbAccess))
            {
                return new RolesToDisplayDbAccess(_roleManager) as T;
            }
            else if (typeof(T) == typeof(RolesDbAccess))
            {
                return new RolesDbAccess(_userManager, _roleManager) as T;
            }
            else if (typeof(T) == typeof(TicketTypesDbAccess))
            {
                return new TicketTypesDbAccess(_ticketsystemContext) as T;
            }
            else if (typeof(T) == typeof(TicketStatusesDbAccess))
            {
                return new TicketStatusesDbAccess(_ticketsystemContext) as T;
            }
            else if (typeof(T) == typeof(TicketChangesDbAccess))
            {
                return new TicketChangesDbAccess(_ticketsystemContext) as T;
            }
            else
            {
                throw new Exception("Wrong DbAccess type!");
            }
        }
    }
}
