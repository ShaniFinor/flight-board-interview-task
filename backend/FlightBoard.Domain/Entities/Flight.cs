using System.ComponentModel.DataAnnotations;
namespace FlightBoard.Domain.Entities
{
    public class Flight
    {
        [Key]
        public string FlightNumber { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public string Gate { get; set; } = string.Empty;

        public string? Status { get; set; } // Calculated in service
    }
}