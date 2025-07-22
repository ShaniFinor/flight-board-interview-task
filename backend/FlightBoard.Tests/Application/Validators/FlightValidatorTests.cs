using FlightBoard.Application.Validators;
using FlightBoard.Domain.Entities;
using FluentValidation.TestHelper;
using Xunit;


namespace FlightBoard.Tests.Application.Validators
{
    public class FlightValidatorTests
    {
        private readonly FlightValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_FlightNumber_Is_Empty()
        {
            var flight = new Flight { FlightNumber = "" };
            var result = _validator.TestValidate(flight);
            result.ShouldHaveValidationErrorFor(f => f.FlightNumber);
        }

        [Fact]
        public void Should_Have_Error_When_DepartureTime_In_Past()
        {
            var flight = new Flight
            {
                FlightNumber = "AB123",
                Destination = "NYC",
                DepartureTime = DateTime.UtcNow.AddHours(-1),
                Gate = "A1"
            };
            var result = _validator.TestValidate(flight);
            result.ShouldHaveValidationErrorFor(f => f.DepartureTime);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Valid()
        {
            var flight = new Flight
            {
                FlightNumber = "AB123",
                Destination = "NYC",
                DepartureTime = DateTime.UtcNow.AddHours(1),
                Gate = "A1"
            };
            var result = _validator.TestValidate(flight);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}