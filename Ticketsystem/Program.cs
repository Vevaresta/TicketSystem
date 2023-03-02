using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Services;

namespace Ticketsystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string identityConnectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

            builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlite(identityConnectionString));

            builder.Services.AddScoped<ChangeRolePermissionsService>();
            builder.Services.AddScoped<GetRolesService>();
            builder.Services.AddScoped<CheckRolePermissionsService>();

            builder.Services.AddDefaultIdentity<TicketsystemUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<EnhancedIdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // add custom tables to the identity db and seed with default values:
            using IServiceScope scope = app.Services.CreateScope();
            RoleManager<EnhancedIdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<EnhancedIdentityRole>>();
            UserManager<TicketsystemUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<TicketsystemUser>>();
            IdentityContext identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            GetRolesService getRolesService = scope.ServiceProvider.GetRequiredService<GetRolesService>();
            ChangeRolePermissionsService changeRolePermissionsService = scope.ServiceProvider.GetService<ChangeRolePermissionsService>();

            ContextSeed.SeedUserRolesAsync(roleManager).Wait();
            ContextSeed.SeedDefaultAdmin(userManager).Wait();
            ContextSeed.SeedPermissionsAsync(identityContext).Wait();
            ContextSeed.SeedRolePermissions(getRolesService, changeRolePermissionsService).Wait();

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