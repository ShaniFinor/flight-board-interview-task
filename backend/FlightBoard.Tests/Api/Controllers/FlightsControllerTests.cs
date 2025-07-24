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
                            var flights = new List<Flight>
                {
                    new Flight
                    {
                        FlightNumber = "AA123",
                        Destination = "Paris",
                        DepartureTime = DateTime.UtcNow.AddHours(1),
                        Gate = "A2",
                        Status = "Boarding"
                    }
                };

                return Task.FromResult(flights);
            }
        }

        [Fact]
        public async Task GetFlights_ReturnsFlightsFromService()
        {
            // Arrange
            var fakeService = new FakeFlightService();
            var controller = new FlightsController(fakeService);

            // Act
            var result = await controller.GetFlights();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
            Assert.Single(flights);
            Assert.Equal("AA123", flights[0].FlightNumber);
            Assert.Equal("Paris", flights[0].Destination);
        }
    }
}