using Microsoft.AspNetCore.Identity;
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

            string dbms = builder.Configuration.GetValue<string>("DBMS");

            if (dbms == "sqlite")
            {
                string dbConnectionString = builder.Configuration.GetConnectionString("SQLiteContextConnection") ?? throw new InvalidOperationException("Connection string 'SQLiteContextConnection' not found.");
                builder.Services.AddDbContext<TicketsystemContext>(options => options.UseSqlite(dbConnectionString));
            }
            else if (dbms == "postgres")
            {
                string dbConnectionString = builder.Configuration.GetConnectionString("PostgreSQLContextConnection") ?? throw new InvalidOperationException("Connection string 'PostgreSQLContextConnection' not found.");
                builder.Services.AddDbContext<TicketsystemContext>(options => options.UseNpgsql(dbConnectionString));
            }

            builder.Services.AddScoped<ContextSeed>();
            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TicketsystemContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IServiceFactory, ServiceFactory>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // add custom tables to the identity db and seed with default values:
            using IServiceScope scope = app.Services.CreateScope();
            ContextSeed contextSeed = scope.ServiceProvider.GetService<ContextSeed>();

            // Auf true setzen, um die Datenbank mit 250 Zufallstickets zu f�llen:
            contextSeed.Seed(true).Wait();

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