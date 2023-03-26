using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Data;

public class TicketsystemContext : IdentityDbContext<User, Role, string>
{
    public TicketsystemContext(DbContextOptions<TicketsystemContext> options)
        : base(options)
    {
    }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketStatus> TicketStatuses { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<TicketChange> TicketChanges { get; set; }
    public virtual DbSet<TicketView> MyView { get; set; }

    public virtual DbSet<TicketView> TicketViewQuery(string selectColumns, int limit, int offset, string orderByColumn, string orderByDirection)
    {
        var query = @"
            CREATE OR REPLACE VIEW my_view AS
              SELECT {0} -- the columns to be selected will be replaced here
              FROM (
                SELECT t.""Id"", t.""ClientId"", t.""DataBackupByClient"", t.""DataBackupByStaff"", t.""DataBackupDone"", t.""DoBackup"", t.""Name"", t.""OrderDate"", t.""TicketStatusId"", t.""TicketTypeId"", t.""WorkOrder""
                FROM ""Tickets"" AS t
                ORDER BY t.""OrderDate"" DESC
                LIMIT {1} OFFSET {2}
              ) AS t0
              LEFT JOIN ""Clients"" AS c ON t0.""ClientId"" = c.""Id""
              LEFT JOIN ""TicketStatuses"" AS t1 ON t0.""TicketStatusId"" = t1.""Id""
              LEFT JOIN ""TicketTypes"" AS t2 ON t0.""TicketTypeId"" = t2.""Id""
              ORDER BY {3} {4}, t0.""Id"", c.""Id"", t1.""Id"", t2.""Id""";

        var sql = string.Format(query, selectColumns, limit, offset, orderByColumn, orderByDirection);
        return (DbSet<TicketView>)Set<TicketView>().FromSqlRaw(sql);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");

        builder.Entity<Ticket>()
            .HasMany(t => t.Devices)
            .WithOne(d => d.Ticket)
            .HasForeignKey(d => d.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Device>()
            .HasMany(d => d.Software)
            .WithOne(s => s.Device)
            .HasForeignKey(s => s.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Ticket>()
            .HasMany(t => t.TicketChanges)
            .WithOne(tc => tc.Ticket)
            .HasForeignKey(tc => tc.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(t => t.TicketChanges)
            .WithOne(tc => tc.User)
            .HasForeignKey(tc => tc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("RolePermissions"));
    }
}
