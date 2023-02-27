using Microsoft.AspNetCore.Identity;

namespace Ticketsystem.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedUserRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Mitarbeiter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Abteilungsleiter.ToString()));
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
                    await userManager.AddToRoleAsync(admin, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(admin, Roles.Abteilungsleiter.ToString());
                    await userManager.AddToRoleAsync(admin, Roles.Mitarbeiter.ToString());
                }
            }
        }
    }
}
