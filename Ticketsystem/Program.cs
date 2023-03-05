using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models;
using Ticketsystem.Services;

namespace Ticketsystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string dbConnectionString = builder.Configuration.GetConnectionString("TicketsystemContextConnection") ?? throw new InvalidOperationException("Connection string 'TicketsystemContextConnection' not found.");

            builder.Services.AddDbContext<TicketsystemContext>(options => options.UseSqlite(dbConnectionString));

            builder.Services.AddScoped<ChangeRolePermissionsService>();
            builder.Services.AddScoped<CheckRolePermissionsService>();
            builder.Services.AddScoped<GetRolesToDisplayService>();
            builder.Services.AddScoped<GetRolesService>();
            builder.Services.AddScoped<ContextSeed>();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TicketsystemContext>()
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