using FluentValidation;
using FlightBoard.Domain.Entities;

namespace FlightBoard.Application.Validators
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RoleFor(f => f.FlightNumber).NoEmpty().WithMessage("Flight number is required.");
            RoleFor(f => f.Destination).NoEmpty().WithMessage("Destination is required.");
            RoleFor(f => f.DepartureTime).GreaterThan(DateTime.UtcNow).WithMessage("Departure time must be in the future.");
            RoleFor(f => f.Gate).NoEmpty().WithMessage("Gate is required.");
        }
    }
}