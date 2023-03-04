using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;
using Ticketsystem.Areas.Identity.Models;

namespace Ticketsystem.Areas.Identity.Data;

public class UsersContext : IdentityDbContext<TicketsystemUser, EnhancedIdentityRole, string>
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<EnhancedIdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<TicketsystemUser>().ToTable("Users");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.Entity<EnhancedIdentityRole>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("RolePermissions"));
    }
}
