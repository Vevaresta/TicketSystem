using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;
using Ticketsystem.Models;

namespace Ticketsystem.Data;

public class TicketsystemContext : IdentityDbContext<User, Role, string>
{
    public TicketsystemContext(DbContextOptions<TicketsystemContext> options)
        : base(options)
    {
    }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<Device> Device { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<TicketStatus> TicketStatus { get; set; }
    public DbSet<TicketType> TicketType { get; set; }

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
    }
}
