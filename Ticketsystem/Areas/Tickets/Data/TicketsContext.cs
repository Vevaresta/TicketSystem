using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Models;
using Ticketsystem.Areas.Tickets.Models;

namespace Ticketsystem.Areas.Tickets.Data
{
    public class TicketsContext : DbContext
    {
        public DbSet<Device> Device { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Software> Software { get; set; }

        public DbSet<TicketStatus> TicketStatus { get; set; }
        
        public DbSet<TicketUpdate> TicketUpdate { get; set; }
        public DbSet<TicketUpdateType> TicketUpdateType { get; set; }



        public TicketsContext(DbContextOptions<TicketsContext> options)
            : base(options)
        {
        }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
