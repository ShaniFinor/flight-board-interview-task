using FlightBoard.Domain.Entities;
using FlightBoard.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FlightBoard.Api.Hubs;

namespace FlightBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;

        private readonly IHubContext<FlightsHub> _hubContext;

        public FlightsController(FlightService flightService, IHubContext<FlightsHub> hubContext)
        {
            _flightService = flightService;
            _hubContext = hubContext;

        }

        // GET /api/flights
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
            //signalR broadcast to frontend
            await _hubContext.Clients.All.SendAsync("FlightAdded", flight);
            return CreatedAtAction(nameof(GetFlight), new { flightNumber = flight.FlightNumber }, flight);
        }

        [HttpDelete("{flightNumber}")]
        public async Task<IActionResult> DeleteFlight(string flightNumber)
        {
            await _flightService.DeleteFlightAsync(flightNumber);
            //signalR broadcast to frontend
            await _hubContext.Clients.All.SendAsync("FlightDeleted", flightNumber);
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<Flight>>> SearchFlights([FromQuery] string? status, [FromQuery] string? destination)
        {
            var result = await _flightService.SearchFlightsAsync(status, destination);
            return Ok(result);
        }
    }
}