﻿using Microsoft.AspNetCore.Identity;
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

        public RolesDbAccess RolesDbAccess { get => new(_userManager, _roleManager); }
        public RolePermissionsDbAccess RolePermissionsDbAccess { get => new(_ticketsystemContext, _userManager, _roleManager); }
        public RolesToDisplayDbAccess RolesToDisplayDbAccess { get => new(_roleManager); }
        public TicketsDbAccess TicketsDbAccess { get => new(_ticketsystemContext, _cache, _globals); }
        public TicketTypesDbAccess TicketTypesDbAccess { get => new(_ticketsystemContext); }
        public TicketStatusesDbAccess TicketStatusesDbAccess { get => new(_ticketsystemContext); }
        public TicketChangesDbAccess TicketChangesDbAccess { get => new(_ticketsystemContext); }
        public ClientsDbAccess ClientsDbAccess { get => new(_ticketsystemContext, _cache, _globals); }
    }
}
