using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ticketsystem.Areas.Identity.Enums;
using Ticketsystem.Areas.Identity.Models;
using Ticketsystem.Areas.Identity.Services;

namespace Ticketsystem.Areas.Identity.Data
{
    public class ContextSeed
    {
        private readonly UsersContext _identityContext;
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;
        private readonly UserManager<TicketsystemUser> _userManager;
        private readonly GetRolesService _getRolesService;
        private readonly ChangeRolePermissionsService _changeRolePermissionsService;

        public ContextSeed(
            UsersContext identityContext,
            RoleManager<EnhancedIdentityRole> roleManager,
            UserManager<TicketsystemUser> userManager,
            GetRolesService getRolesService,
            ChangeRolePermissionsService changeRolePermissionsService
            )
        {
            _identityContext = identityContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _getRolesService = getRolesService;
            _changeRolePermissionsService = changeRolePermissionsService;
        }

        public async Task Seed()
        {
            await SeedUserRolesAsync();
            await SeedDefaultAdmin();
            await SeedPermissionsAsync();
            await SeedRolePermissions();
        }

        public async Task SeedUserRolesAsync()
        {
            var query = from role in _roleManager.Roles
                        select role.Name;

            foreach (var role in Enum.GetNames<DefaultRoles>())
            {
                if (!query.Contains(role.ToString()))
                {
                    await _roleManager.CreateAsync(new EnhancedIdentityRole(role.ToString()));
                }
            }
        }

        public async Task SeedDefaultAdmin()
        {
            TicketsystemUser admin = new()
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

        public async Task SeedPermissionsAsync()
        {
            var permissionsEnumList = Enum.GetValues<RolePermissions>();
            List<string> permissions = new();

            foreach (RolePermissions pEnum in permissionsEnumList)
            {
                permissions.Add(pEnum.ToString());
            }

            List<Permission> tempList = new(_identityContext.Permissions);

            foreach (var p in tempList)
            {
                if (!permissions.Contains(p.Name))
                {
                    _identityContext.Permissions.Remove(p);
                }
            }

            foreach (string permission in permissions)
            {
                if (!_identityContext.Permissions.Where(p => p.Name == permission).Any())
                {
                    Permission perm = new()
                    {
                        Name = permission.ToString()
                    };
                    _identityContext.Permissions.Add(perm);
                }
            }

            await _identityContext.SaveChangesAsync();
        }

        public async Task SeedRolePermissions()
        {
            var administrator = await _getRolesService.GetRoleByNameAsync(DefaultRoles.Administrator.ToString());
            var fallback = await _getRolesService.GetRoleByNameAsync(DefaultRoles.Fallback.ToString());

            foreach (var permission in Enum.GetValues<RolePermissions>())
            { 
                await _changeRolePermissionsService.AddPermissionToRole(administrator, permission);
            }

            await _changeRolePermissionsService.RemoveAllPermissionsFromRole(fallback);
        }
    }
}
