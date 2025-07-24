using FlightBoard.Domain.Entities;
using FlightBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightBoard.Infrastructure.Repositories
{
    public class FlightRepository
    {
        private readonly FlightDbContext _context;
        public FlightRepository(FlightDbContext context)
        {
            _context = context;
        }
        //return all Flights.
        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        // return Flight by FlightNumber.
        public async Task<Flight?> GetFlightAsync(string flightNumber)
        {
            return await _context.Flights.FindAsync(flightNumber);
        }

        //Add now flight
        public async Task AddFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        //Delete Flight by FlightNumber.
        public async Task DeleteFlightAsync(string flightNumber)
        {
            var flight = await GetFlightAsync(flightNumber);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }
    }
}