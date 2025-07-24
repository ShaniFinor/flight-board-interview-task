using FlightBoard.Application.Services;
using FlightBoard.Domain.Enums;
using FlightBoard.Infrastructure.Repositories;
using Xunit;

namespace FlightBoard.Tests.Application.Services
{
    public class FlightServiceTests
    {
       private class FakeRepository : FlightRepository
        {
            public FakeRepository() : base(null!) { }
        }

        private readonly FlightService _service;

        public FlightServiceTests()
        {
            _service = new FlightService(new FakeRepository());
        }

        [Fact]
        public void GetStatus_ReturnsBoarding_WhenWithin30MinutesOfDeparture()
        {
            var departure = DateTime.UtcNow.AddMinutes(15);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Boarding.ToString(), status);
        }

        [Fact]
        public void GetStatus_ReturnsDeparted_WhenWithin1HourAfterDeparture()
        {
            var departure = DateTime.UtcNow.AddMinutes(-30);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Departed.ToString(), status);
        }

        [Fact]
        public void GetStatus_ReturnsLanded_WhenMoreThan1HourAfterDeparture()
        {
            var departure = DateTime.UtcNow.AddHours(-2);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Landed.ToString(), status);
        }

        [Fact]
        public void GetStatus_ReturnsScheduled_WhenMoreThan30MinutesToDeparture()
        {
            var departure = DateTime.UtcNow.AddHours(2);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Scheduled.ToString(), status);
        }
    }
}