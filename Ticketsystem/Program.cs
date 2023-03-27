using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Ticketsystem.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;
using Ticketsystem.Models.Data;
using Ticketsystem.Utilities;

namespace Ticketsystem
{
    public class Program
    {
        //[RequiresUnreferencedCode()]
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string dbms = builder.Configuration.GetValue<string>("DBMS");
            bool seedTestData = builder.Configuration.GetValue<bool>("SeedTestData");
            bool enableRedisCache = builder.Configuration.GetValue<bool>("EnableRedisCache");
            string redisServer = builder.Configuration.GetSection("RedisCacheOptions")["Configuration"];
            string redisTicketsCache = builder.Configuration.GetSection("RedisCacheOptions")["InstanceName"];

            if (dbms == "sqlite")
            {
                string dbConnectionString = builder.Configuration.GetConnectionString("SQLiteContextConnection") ?? throw new InvalidOperationException("Connection string 'SQLiteContextConnection' not found.");
                builder.Services.AddDbContext<TicketsystemContext>(contextOptions =>
                {
                    contextOptions.UseSqlite(
                        dbConnectionString,
                        sqlOptions =>
                        {
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                });
            }
            else if (dbms == "postgres")
            {
                string dbConnectionString = builder.Configuration.GetConnectionString("PostgreSQLContextConnection") ?? throw new InvalidOperationException("Connection string 'PostgreSQLContextConnection' not found.");
                builder.Services.AddDbContext<TicketsystemContext>(contextOptions =>
                {
                    contextOptions.UseNpgsql(
                        dbConnectionString,
                        sqlOptions =>
                        {
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                });
            }

            builder.Services.AddScoped<ContextSeed>();
            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TicketsystemContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<Globals>(new Globals()
            {
                EnableRedisCache = enableRedisCache,
                RedisServer = redisServer,
                RedisTicketsCache = redisTicketsCache,
            });
            builder.Services.AddScoped<IDbAccessFactory, DbAccessFactory>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Elmah Core
            builder.Services.AddElmah(options =>
            {
                options.OnPermissionCheck = context => context.User.IsInRole("Administrator");
                options.Path = "/logging";
            });

            builder.Services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.LogPath = "~/log"; 
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisCacheOptions:Configuration").Value;
                options.InstanceName = builder.Configuration.GetSection("RedisCacheOptions:InstanceName").Value;
            });

            WebApplication app = builder.Build();

            // add custom tables to the identity db and seed with default values:
            using IServiceScope scope = app.Services.CreateScope();
            ContextSeed contextSeed = scope.ServiceProvider.GetService<ContextSeed>();

            // Auf true setzen, um die Datenbank mit 250 Zufallstickets zu f�llen:
            contextSeed.Seed(seedTestData).Wait();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // app.UseExceptionHandler("/Home/Error");
                app.UseElmahExceptionPage();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();                           
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            RequestLocalizationOptions localizationOptions = new()
            {
                SupportedCultures = new List<CultureInfo> { new CultureInfo("de-DE") },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo("de-DE") },
                DefaultRequestCulture = new RequestCulture("de-DE")
            };
            app.UseRequestLocalization(localizationOptions);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();

            app.Run();

        }
    }
}