using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;
namespace Ticketsystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var identityConnectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

            builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlite(identityConnectionString));

            builder.Services.AddDefaultIdentity<TicketsystemUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<EnhancedIdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<EnhancedIdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TicketsystemUser>>();
            var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            ContextSeed.SeedUserRolesAsync(roleManager).Wait();
            ContextSeed.SeedDefaultAdmin(userManager).Wait();
            ContextSeed.SeedPermissionsAsync(identityContext).Wait();
            ContextSeed.SeedRolePermissions(roleManager, identityContext).Wait();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            
            app.Run();
        }
    }
}