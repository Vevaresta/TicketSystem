using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ticketsystem.Areas.Identity.Services;

namespace Ticketsystem.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedUserRolesAsync(RoleManager<EnhancedIdentityRole> roleManager)
        {
            var query = from role in roleManager.Roles
                        select role.Name;

            foreach (var role in Enum.GetNames<DefaultRoles>())
            {
                if (!query.Contains(role.ToString()))
                {
                    await roleManager.CreateAsync(new EnhancedIdentityRole(role.ToString()));
                }
            }
        }

        public static async Task SeedDefaultAdmin(UserManager<TicketsystemUser> userManager)
        {
            TicketsystemUser admin = new()
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "User",
                Email = "admin@localhost",
            };

            if (userManager.Users.All(user => user.Id != admin.Id))
            {
                var adminInDb = await userManager.FindByNameAsync(admin.UserName);
                if (adminInDb == null)
                {
                    await userManager.CreateAsync(admin, "Service1234!");
                    await userManager.AddToRoleAsync(admin, DefaultRoles.Administrator.ToString());
                }
            }
        }

        public static async Task SeedPermissionsAsync(IdentityContext identityContext)
        {
            var permissionsEnumList = Enum.GetValues<PermissionsEnum>();
            List<string> permissions = new();

            foreach (PermissionsEnum pEnum in permissionsEnumList)
            {
                permissions.Add(pEnum.ToString());
            }

            List<Permission> tempList = new(identityContext.Permissions);

            foreach (var p in tempList)
            {
                if (!permissions.Contains(p.Name))
                {
                    identityContext.Permissions.Remove(p);
                }
            }

            foreach (string permission in permissions)
            {
                if (!identityContext.Permissions.Where(p => p.Name == permission).Any())
                {
                    Permission perm = new()
                    {
                        Name = permission.ToString()
                    };
                    identityContext.Permissions.Add(perm);
                }
            }

            await identityContext.SaveChangesAsync();
        }

        public static async Task SeedRolePermissions(GetRolesService getRolesService, ChangeRolePermissionsService changeRolePermissionsService)
        {
            var administrator = await getRolesService.GetRoleByNameAsync(DefaultRoles.Administrator.ToString());
            var fallback = await getRolesService.GetRoleByNameAsync(DefaultRoles.Fallback.ToString());

            foreach (var permission in Enum.GetValues<PermissionsEnum>())
            { 
                await changeRolePermissionsService.AddPermissionToRole(administrator, permission);
            }

            await changeRolePermissionsService.RemoveAllPermissionsFromRole(fallback);
        }
    }
}
