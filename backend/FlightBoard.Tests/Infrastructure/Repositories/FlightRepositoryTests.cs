using FlightBoard.Domain.Entities;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FlightBoard.Tests.Infrastructure.Repositories
{
    public class FlightRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly FlightDbContext _context;
        private readonly FlightRepository _repository;

        public FlightRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new FlightDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new FlightRepository(_context);
        }

        [Fact]
        public async Task AddFlightAsync_AddsFlightSuccessfully()
        {
            var flight = new Flight
            {
                FlightNumber = "LY15",
                Destination = "Paris",
                DepartureTime = DateTime.UtcNow.AddHours(2),
                Gate = "A2"
            };

            await _repository.AddFlightAsync(flight);
            var saved = await _repository.GetFlightAsync("LY15");

            Assert.NotNull(saved);
            Assert.Equal("Paris", saved?.Destination);
        }

        [Fact]
        public async Task GetAllFlightsAsync_ReturnsAllFlights()
        {
            await _repository.AddFlightAsync(new Flight { FlightNumber = "B1", Destination = "Rome", DepartureTime = DateTime.UtcNow, Gate = "C1" });
            await _repository.AddFlightAsync(new Flight { FlightNumber = "B2", Destination = "Berlin", DepartureTime = DateTime.UtcNow, Gate = "C2" });

            var flights = await _repository.GetAllFlightsAsync();

            Assert.Equal(2, flights.Count);
        }

        [Fact]
        public async Task GetFlightAsync_ReturnsCorrectFlight()
        {
            var flight = new Flight
            {
                FlightNumber = "LY08",
                Destination = "London",
                DepartureTime = DateTime.UtcNow,
                Gate = "C3"
            };

            await _repository.AddFlightAsync(flight);
            var result = await _repository.GetFlightAsync("LY08");

            Assert.NotNull(result);
            Assert.Equal("London", result?.Destination);
        }

        [Fact]
        public async Task DeleteFlightAsync_RemovesFlight()
        {
            var flight = new Flight
            {
                FlightNumber = "LY44",
                Destination = "NYC",
                DepartureTime = DateTime.UtcNow,
                Gate = "D4"
            };

            await _repository.AddFlightAsync(flight);
            await _repository.DeleteFlightAsync("LY44");

            var deleted = await _repository.GetFlightAsync("LY44");
            Assert.Null(deleted);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
