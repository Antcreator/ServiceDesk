using Microsoft.EntityFrameworkCore;
using ServiceDesk.Data.Config;
using ServiceDesk.Data.Model;

namespace ServiceDesk.Data.Context
{
    public class PersistenceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new TicketConfig());
        }
    }
}
