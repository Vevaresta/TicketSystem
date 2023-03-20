 using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Models;

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
    public DbSet<TicketUsers> TicketUsers { get; set; }
    public DbSet<TicketChanges> TicketChanges { get; set; }
    public DbSet<PermissionsTriggered> PermissionsTriggered { get; set; }

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

        builder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("RolePermissions"));

        builder.Entity<TicketUsers>()
            .HasKey(tu => new { tu.UserId, tu.TicketId });

        builder.Entity<TicketChanges>()
            .HasKey(ut => new { ut.UserId, ut.TicketId });

        builder.Entity<PermissionsTriggered>()
            .HasKey(up => new { up.UserId, up.PermissionId });
    }
}
