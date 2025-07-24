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
    }
}