using FlightBoard.Domain.Entities;
using FlightBoard.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;
        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Flight>>> GetFlights()
        {
            var flights = await _flightService.GetFlightsWithStatusAsync();
            return Ok(flights);
        }

        [HttpGet("{flightNumber}")]
        public async Task<ActionResult<Flight>> GetFlight(string flightNumber)
        {
            var flight = await _flightService.GetFlightAsync(flightNumber);
            if (flight == null)
                return NotFound();
            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight([FromBody] Flight flight)
        {
            await _flightService.AddFlightAsync(flight);
            return CreatedAtAction(nameof(GetFlight), new { flightNumber = flight.FlightNumber }, flight);
        }

        [HttpDelete("{flightNumber}")]
        public async Task<IActionResult> DeleteFlight(string flightNumber)
        {
            await _flightService.DeleteFlightAsync(flightNumber);
            return NoContent();
        }
    }
}