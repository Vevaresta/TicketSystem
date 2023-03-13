using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ticketsystem.Enums;
using Ticketsystem.Models;
using Ticketsystem.Services;

namespace Ticketsystem.Data
{
    public class ContextSeed
    {
        private readonly TicketsystemContext _ticketSystemContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        private readonly IServiceFactory _serviceFactory;

        public ContextSeed(
            TicketsystemContext ticketsystemContext,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IServiceFactory serviceFactory
            )
        {
            _ticketSystemContext = ticketsystemContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _serviceFactory = serviceFactory;
        }

        public async Task SeedAsync()
        {
            await SeedUserRoles();
            await SeedDefaultAdmin();
            await SeedPermissions();
            await SeedRolePermissions();
            await SeedTicketStatuses();
            await SeedTicketTypes();
        }

        public async Task SeedUserRoles()
        {
            var query = from role in _roleManager.Roles
                        select role.Name;

            foreach (var role in Enum.GetNames<DefaultRoles>())
            {
                if (!query.Contains(role.ToString()))
                {
                    await _roleManager.CreateAsync(new Role(role.ToString()));
                }
            }
        }

        public async Task SeedDefaultAdmin()
        {
            User admin = new()
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "User",
                Email = "admin@localhost",
            };

            if (_userManager.Users.All(user => user.Id != admin.Id))
            {
                var adminInDb = await _userManager.FindByNameAsync(admin.UserName);
                if (adminInDb == null)
                {
                    await _userManager.CreateAsync(admin, "Service1234!");
                    await _userManager.AddToRoleAsync(admin, DefaultRoles.Administrator.ToString());
                }
            }
        }

        public async Task SeedPermissions()
        {
            var permissionsEnumList = Enum.GetValues<RolePermissions>();
            List<string> permissions = new();

            foreach (RolePermissions pEnum in permissionsEnumList)
            {
                permissions.Add(pEnum.ToString());
            }

            List<Permission> tempList = new(_ticketSystemContext.Permissions);

            foreach (var p in tempList)
            {
                if (!permissions.Contains(p.Name))
                {
                    _ticketSystemContext.Permissions.Remove(p);
                }
            }

            foreach (string permission in permissions)
            {
                if (!_ticketSystemContext.Permissions.Where(p => p.Name == permission).Any())
                {
                    Permission perm = new()
                    {
                        Name = permission.ToString()
                    };
                    _ticketSystemContext.Permissions.Add(perm);
                }
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        public async Task SeedRolePermissions()
        {
            var administrator = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Administrator.ToString());
            var fallback = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Fallback.ToString());

            foreach (var permission in Enum.GetValues<RolePermissions>())
            {
                await _serviceFactory.GetRolePermissionsService().AddPermissionToRole(administrator, permission);
            }

            await _serviceFactory.GetRolePermissionsService().RemoveAllPermissionsFromRole(fallback);
        }

        public async Task SeedTicketTypes()
        {
            foreach (var type in Enum.GetValues<TicketTypes>())
            {
                if (!await _ticketSystemContext.TicketTypes.AnyAsync(t => t.Name == type.ToString()))
                {
                    await _ticketSystemContext.TicketTypes.AddAsync(new TicketType() { Name = type.ToString() });
                }
            }
            await _ticketSystemContext.SaveChangesAsync();
        }

        public async Task SeedTicketStatuses()
        {
            foreach (var status in Enum.GetValues(typeof(TicketStatuses)))
            {
                if (!await _ticketSystemContext.TicketStatuses.AnyAsync(t => t.Name == status.ToString()))
                {
                    await _ticketSystemContext.TicketStatuses.AddAsync(new TicketStatus { Name = status.ToString() });
                }
            }
            await _ticketSystemContext.SaveChangesAsync();
        }
    }
}
