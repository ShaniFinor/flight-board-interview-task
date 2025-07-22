using FlightBoard.Application.Services;
using FlightBoard.Domain.Enums;
using Xunit;

namespace FlightBoard.Tests.Application.Services
{
    public class FlightServiceTests
    {
        private readonly FlightService _service = new();

        [Fact]
        public void GetStatus_ReturnsBoarding_WhenWithin30MinutesOfDeparture()
        {
            var departure = DateTime.UtcNow.AddMinutes(15);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Boarding, status);
        }

        [Fact]
        public void GetStatus_ReturnsDeparted_WhenWithin1HourAfterDeparture()
        {
            var departure = DateTime.UtcNow.AddMinutes(-30);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Departed, status);
        }

        [Fact]
        public void GetStatus_ReturnsLanded_WhenMoreThan1HourAfterDeparture()
        {
            var departure = DateTime.UtcNow.AddHours(-2);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Landed, status);
        }

        [Fact]
        public void GetStatus_ReturnsScheduled_WhenMoreThan30MinutesToDeparture()
        {
            var departure = DateTime.UtcNow.AddHours(2);
            var status = _service.GetStatus(departure);
            Assert.Equal(FlightStatus.Scheduled, status);
        }
    }
}