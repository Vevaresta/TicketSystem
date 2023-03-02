using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Models;
using Ticketsystem.Areas.Identity.Services;

namespace Ticketsystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string identityConnectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

            builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlite(identityConnectionString));

            builder.Services.AddScoped<ChangeRolePermissionsService>();
            builder.Services.AddScoped<CheckRolePermissionsService>();
            builder.Services.AddScoped<GetRolesToDisplayService>();
            builder.Services.AddScoped<GetRolesService>();
            builder.Services.AddScoped<ContextSeed>();

            builder.Services.AddDefaultIdentity<TicketsystemUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<EnhancedIdentityRole>()
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // add custom tables to the identity db and seed with default values:
            using IServiceScope scope = app.Services.CreateScope();
            ContextSeed contextSeed = scope.ServiceProvider.GetService<ContextSeed>();
            contextSeed.Seed().Wait();

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