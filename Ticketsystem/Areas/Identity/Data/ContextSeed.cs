using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ticketsystem.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedUserRolesAsync(RoleManager<EnhancedIdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new EnhancedIdentityRole(RolesEnum.Administrator.ToString()));
            await roleManager.CreateAsync(new EnhancedIdentityRole(RolesEnum.Mitarbeiter.ToString()));
            await roleManager.CreateAsync(new EnhancedIdentityRole(RolesEnum.Abteilungsleiter.ToString()));
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
            EnhancedIdentityRole administrator = await roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == RolesEnum.Administrator.ToString());
            EnhancedIdentityRole abteilungsleiter = await roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == RolesEnum.Abteilungsleiter.ToString());
            EnhancedIdentityRole mitarbeiter = await roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == RolesEnum.Mitarbeiter.ToString());

            await AddPermissionToRole(identityContext, roleManager, administrator, PermissionsEnum.ManageUsers);
            await AddPermissionToRole(identityContext, roleManager, administrator, PermissionsEnum.CreateTickets);
            await AddPermissionToRole(identityContext, roleManager, administrator, PermissionsEnum.UpdateTickets);
            await AddPermissionToRole(identityContext, roleManager, administrator, PermissionsEnum.DeleteTickets);
        }

        private static async Task AddPermissionToRole(
            IdentityContext identityContext,
            RoleManager<EnhancedIdentityRole> roleManager,
            EnhancedIdentityRole role,
            PermissionsEnum permission
            )
        {
            var permObject = await identityContext.Permissions.FirstOrDefaultAsync(p => p.Name == permission.ToString());

            // Check if the permission is already in the administrator's permission list
            if (!role.Permissions.Contains(permObject))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Add(permObject);
                await roleManager.UpdateAsync(role);
            }
        }
    }
}
