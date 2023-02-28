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

            foreach (var role in Enum.GetNames(typeof(RolesEnum)))
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
                    await userManager.AddToRoleAsync(admin, RolesEnum.Administrator.ToString());
                }
            }
        }

        public static async Task SeedPermissionsAsync(IdentityContext identityContext)
        {
            if (identityContext.Permissions.Any())
            {
                return;   // Permissions already seeded
            }

            foreach (PermissionsEnum permission in Enum.GetValues(typeof(PermissionsEnum)))
            {
                var perm = new Permission
                {
                    Name = permission.ToString()
                };
                identityContext.Permissions.Add(perm);
            }

            await identityContext.SaveChangesAsync();
        }

        public static async Task SeedRolePermissions(RoleManager<EnhancedIdentityRole> roleManager, IdentityContext identityContext)
        {
            RolePermissionsService rolePermissionsService = new RolePermissionsService(identityContext, roleManager);

            EnhancedIdentityRole administrator = await rolePermissionsService.GetRole(RolesEnum.Administrator);

            foreach (PermissionsEnum permission in Enum.GetValues(typeof(PermissionsEnum)))
            { 
                await rolePermissionsService.AddPermissionToRole(administrator, permission);
            }
        }
    }
}
