using FlightBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightBoard.Infrastructure.Data
{
    public class FlightDbContext : DbContext
    {
       public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasKey(f => f.FlightNumber); // FlightNumber as PK
            base.OnModelCreating(modelBuilder);
        }

    }
}