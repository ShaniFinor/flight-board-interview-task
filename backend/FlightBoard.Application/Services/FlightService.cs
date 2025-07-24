using FlightBoard.Domain.Entities;
using FlightBoard.Infrastructure.Repositories;

namespace FlightBoard.Application.Services
{
    public class FlightService
    {
        private readonly FlightRepository _repository;
         public FlightService(FlightRepository repository)
        {
            _repository = repository;
        }

        public virtual async Task<List<Flight>> GetFlightsWithStatusAsync()
        {
            var flights = await _repository.GetAllFlightsAsync();

            foreach (var flight in flights)
            {
                flight.Status = GetStatus(flight.DepartureTime);
            }

            return flights;
        }
          public virtual async Task<Flight?> GetFlightAsync(string flightNumber)
        {
            var flight = await _repository.GetFlightAsync(flightNumber);
            if (flight != null)
            {
                flight.Status = GetStatus(flight.DepartureTime);
            }
            return flight;
        }

        public virtual async Task AddFlightAsync(Flight flight)
        {
            await _repository.AddFlightAsync(flight);
        }

        public virtual async Task DeleteFlightAsync(string flightNumber)
        {
            await _repository.DeleteFlightAsync(flightNumber);
        }

        public string GetStatus(DateTime departureTime)
        {
            var now = DateTime.UtcNow;
            var diff = departureTime - now;

            if (diff.TotalMinutes > 30)
                return "Scheduled";
            if (diff.TotalMinutes <= 30 && diff.TotalMinutes > 0)
                return "Boarding";
            if (diff.TotalMinutes <= 0 && diff.TotalMinutes > -60)
                return "Departed";

            return "Landed";
        }
        
    }
}