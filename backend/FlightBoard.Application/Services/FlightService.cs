using FlightBoard.Domain.Enums;

namespace FlightBoard.Application.Services
{
    public class FlightService
    {
        public FlightStatus GetStatus(DateTime departureTime)
        {
            var now = DateTime.UtcNow;
            if (now >= departureTime.AddMinutes(-30) && now < departureTime) return FlightStatus.Boarding;
            if (now >= departureTime && now <= departureTime.AddHours(1)) return FlightStatus.Departed;
            if (now > departureTime.AddHours(1)) return FlightStatus.Landed;
            return FlightStatus.Landed;
        }
    }
}