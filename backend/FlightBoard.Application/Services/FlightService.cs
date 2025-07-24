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

        public async Task<List<Flight>> GetFlightsWithStatusAsync()
        {
            var flights = await _repository.GetAllFlightsAsync();

            foreach (var flight in flights)
            {
                flight.Status = GetStatus(flight.DepartureTime);
            }

            return flights;
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