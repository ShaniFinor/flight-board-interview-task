using System.ComponentModel.DataAnnotations;
namespace FlightBoard.Domain.Entities
{
    public class Flight
    {
        [Key]
        public string FlightNumber { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public string Gate { get; set; } = string.Empty;

        public string? Status { get; set; } // Calculated in service
    }
}