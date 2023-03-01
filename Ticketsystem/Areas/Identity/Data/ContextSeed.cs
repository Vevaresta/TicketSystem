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

            foreach (var role in Enum.GetNames(typeof(DefaultRoles)))
            {
                if (!query.Contains(role.ToString()))
                {
                    await roleManager.CreateAsync(new EnhancedIdentityRole(role.ToString()));
                }
            }
        }

        public static async Task SeedDefaultAdmin(UserManager<TicketsystemUser> userManager)
        {
            TicketsystemUser admin = new TicketsystemUser
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "User",
                Email = "admin@localhost",
            };

            if (userManager.Users.All(user => user.Id != admin.Id))
            {
                TicketsystemUser adminInDb = await userManager.FindByNameAsync(admin.UserName);
                if (adminInDb == null)
                {
                    await userManager.CreateAsync(admin, "Service1234!");
                    await userManager.AddToRoleAsync(admin, DefaultRoles.Administrator.ToString());
                }
            }
        }

        public static async Task SeedPermissionsAsync(IdentityContext identityContext)
        {
            var permissionsEnumList = Enum.GetValues(typeof(PermissionsEnum));
            List<string> permissions = new List<string>();
            foreach (var pEnum in permissionsEnumList)
            {
                permissions.Add(pEnum.ToString());
            }

            List<Permission> tempList = new List<Permission>(identityContext.Permissions);

            foreach (Permission p in tempList)
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
                    var perm = new Permission
                    {
                        Name = permission.ToString()
                    };
                    identityContext.Permissions.Add(perm);
                }
            }

            await identityContext.SaveChangesAsync();
        }

        public static async Task SeedRolePermissions(IServiceProvider serviceProvider)
        {
            var rolesService = serviceProvider.GetRequiredService<RolesService>();
            var rolePermissionsService = serviceProvider.GetRequiredService<RolePermissionsService>();

            EnhancedIdentityRole administrator = await rolesService.GetRoleByNameAsync(DefaultRoles.Administrator.ToString());
            EnhancedIdentityRole fallback = await rolesService.GetRoleByNameAsync(DefaultRoles.Fallback.ToString());

            foreach (PermissionsEnum permission in Enum.GetValues(typeof(PermissionsEnum)))
            { 
                await rolePermissionsService.AddPermissionToRole(administrator, permission);
            }

            await rolePermissionsService.RemoveAllPermissionsFromRole(fallback);
        }
    }
}
