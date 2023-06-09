﻿using Microsoft.AspNetCore.Identity;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");

        builder.Entity<Ticket>().ToTable("Tickets");
        builder.Entity<Device>().ToTable("Devices");
        builder.Entity<Client>().ToTable("Clients");
        builder.Entity<Permission>().ToTable("Permissions");
        builder.Entity<Software>().ToTable("Software");
        builder.Entity<TicketStatus>().ToTable("TicketStatuses");
        builder.Entity<TicketType>().ToTable("TicketTypes");
        builder.Entity<TicketChange>().ToTable("TicketChanges");

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
