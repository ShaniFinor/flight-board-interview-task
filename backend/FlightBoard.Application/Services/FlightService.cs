using FlightBoard.Domain.Entities;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FlightBoard.Application.Services
{
    public class FlightService
    {
        private readonly FlightRepository _repository;
        private readonly ILogger<FlightService> _logger;
        private readonly IMemoryCache _cache;
        public FlightService(FlightRepository repository, ILogger<FlightService> logger, IMemoryCache cache)
        {
            _repository = repository;
            _logger = logger;
            _cache = cache;
        }

        public virtual async Task<List<Flight>> GetFlightsWithStatusAsync()
        {
            _logger.LogInformation("Checking cache for flights");

            var flights = await _cache.GetOrCreateAsync("flights_cache", async entry =>
            {
                _logger.LogInformation("Cache miss – loading from repository");
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);

                var freshFlights = await _repository.GetAllFlightsAsync();
                return freshFlights;
            });

            foreach (var flight in flights)
            {
                flight.Status = GetStatus(flight.DepartureTime);
            }

            return flights;
        }
        public virtual async Task<Flight?> GetFlightAsync(string flightNumber)
        {
            _logger.LogInformation("Fetching flight with number {FlightNumber}", flightNumber);
            var flight = await _repository.GetFlightAsync(flightNumber);
            if (flight != null)
            {
                flight.Status = GetStatus(flight.DepartureTime);
                _logger.LogInformation("Flight found: {@Flight}", flight);
            }
            else
            {
                _logger.LogWarning("Flight with number {FlightNumber} not found", flightNumber);
            }
            return flight;
        }

        public virtual async Task AddFlightAsync(Flight flight)
        {
            _logger.LogInformation("Adding new flight: {@Flight}", flight);
            await _repository.AddFlightAsync(flight);
            _logger.LogInformation("Flight added successfully: {FlightNumber}", flight.FlightNumber);
            _cache.Remove("flights_cache"); // refresh cache.
        }

        public virtual async Task DeleteFlightAsync(string flightNumber)
        {
            _logger.LogInformation("Attempting to delete flight with number {FlightNumber}", flightNumber);
            var flight = await _repository.GetFlightAsync(flightNumber);
            if (flight == null)
            {
                _logger.LogWarning("Cannot delete flight. Flight with number {FlightNumber} not found", flightNumber);
                return;
            }
            await _repository.DeleteFlightAsync(flightNumber);
            _logger.LogInformation("Flight with number {FlightNumber} deleted successfully", flightNumber);
            _cache.Remove("flights_cache"); // refresh cache.
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

        public virtual async Task<List<Flight>> SearchFlightsAsync(string? status, string? destination)
        {
            _logger.LogInformation("Searching flights with filters: Status: {Status}, Destination: {Destination}", status, destination);
            var flights = await _repository.GetAllFlightsAsync();

            foreach (var flight in flights)
            {
                flight.Status = GetStatus(flight.DepartureTime);
            }

            if (!string.IsNullOrEmpty(status))
                flights = flights.Where(f => f.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (!string.IsNullOrEmpty(destination))
                flights = flights.Where(f => f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).ToList();

            _logger.LogInformation("Search returned {Count} flights", flights.Count);
            return flights;
        }

    }
}