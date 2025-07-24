using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FlightBoard.Infrastructure.Data;

namespace FlightBoard.Infrastructure.Data
{
    public class FlightDbContextFactory : IDesignTimeDbContextFactory<FlightDbContext>
    {
        public FlightDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightDbContext>();
            optionsBuilder.UseSqlite("Data Source=flightboard.db");

            return new FlightDbContext(optionsBuilder.Options);
        }
    }
}