using FlightBoard.Api.Controllers;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FlightBoard.Tests.Api.Controllers
{
    public class FlightsControllerTests
    {
        private class FakeFlightService : FlightService
        {
            public FakeFlightService() : base(null!) { }

            public override Task<List<Flight>> GetFlightsWithStatusAsync()
            {
                return Task.FromResult(new List<Flight>
                {
                    new Flight { FlightNumber = "AA123", Destination = "Paris", DepartureTime = DateTime.UtcNow.AddHours(1), Gate = "A2", Status = "Boarding" }
                });
            }

            public override Task<Flight?> GetFlightAsync(string flightNumber)
            {
                if (flightNumber == "AA123")
                {
                    return Task.FromResult<Flight?>(new Flight { FlightNumber = "AA123", Destination = "Paris", DepartureTime = DateTime.UtcNow, Gate = "A2", Status = "Scheduled" });
                }
                return Task.FromResult<Flight?>(null);
            }

            public override Task AddFlightAsync(Flight flight) => Task.CompletedTask;
            public override Task DeleteFlightAsync(string flightNumber) => Task.CompletedTask;
        }

        [Fact]
        public async Task GetFlights_ReturnsListOfFlights()
        {
            var controller = new FlightsController(new FakeFlightService());

            var result = await controller.GetFlights();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<List<Flight>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task GetFlight_ReturnsCorrectFlight()
        {
            var controller = new FlightsController(new FakeFlightService());

            var result = await controller.GetFlight("AA123");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var flight = Assert.IsType<Flight>(ok.Value);
            Assert.Equal("AA123", flight.FlightNumber);
        }

        [Fact]
        public async Task GetFlight_ReturnsNotFound_WhenMissing()
        {
            var controller = new FlightsController(new FakeFlightService());

            var result = await controller.GetFlight("ZZ999");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddFlight_ReturnsCreatedAt()
        {
            var controller = new FlightsController(new FakeFlightService());

            var result = await controller.AddFlight(new Flight
            {
                FlightNumber = "BB456",
                Destination = "Berlin",
                DepartureTime = DateTime.UtcNow.AddMinutes(20),
                Gate = "B1"
            });

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var added = Assert.IsType<Flight>(created.Value);
            Assert.Equal("BB456", added.FlightNumber);
        }

        [Fact]
        public async Task DeleteFlight_ReturnsNoContent()
        {
            var controller = new FlightsController(new FakeFlightService());

            var result = await controller.DeleteFlight("AA123");

            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task SearchFlights_ReturnsMatchingFlights()
        {
            // Arrange
            var fakeService = new FakeFlightServiceWithSearch();
            var controller = new FlightsController(fakeService);
        
            // Act
            var result = await controller.SearchFlights("Boarding", "Paris");
        
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
            Assert.Single(flights);
            Assert.Equal("AA123", flights[0].FlightNumber);
        }
        
        private class FakeFlightServiceWithSearch : FlightService
        {
            public FakeFlightServiceWithSearch() : base(null!) { }
        
            public override Task<List<Flight>> SearchFlightsAsync(string? status, string? destination)
            {
                var flights = new List<Flight>
                {
                    new Flight { FlightNumber = "AA123", Destination = "Paris", DepartureTime = DateTime.UtcNow.AddMinutes(20), Gate = "A2", Status = "Boarding" },
                    new Flight { FlightNumber = "BB234", Destination = "Berlin", DepartureTime = DateTime.UtcNow.AddHours(2), Gate = "B1", Status = "Scheduled" }
                };
        
                return Task.FromResult(
                    flights.Where(f =>
                        (string.IsNullOrEmpty(status) || f.Status == status) &&
                        (string.IsNullOrEmpty(destination) || f.Destination == destination)
                    ).ToList()
                );
            }
        }
    }
}