using FluentValidation;
using FlightBoard.Domain.Entities;

namespace FlightBoard.Application.Validators
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RuleFor(f => f.FlightNumber).NotEmpty().WithMessage("Flight number is required.");
            RuleFor(f => f.Destination).NotEmpty().WithMessage("Destination is required.");
            RuleFor(f => f.DepartureTime).Must(date => date > DateTime.UtcNow).WithMessage("Departure time must be in the future.");
            RuleFor(f => f.Gate).NotEmpty().WithMessage("Gate is required.");
        }
    }
}