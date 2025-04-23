using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<IActionResult> AddFlight([FromBody] FlightRequest request)
        {
            try
            {
                var result = await _flightService.AddFlight(request);
                return CreatedAtAction(nameof(GetFlightById), new { id = result.FlightId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightRequest request)
        {
            try
            {
                var result = await _flightService.UpdateFlight(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var success = await _flightService.DeleteFlight(id);
            if (!success)
                return NotFound(new { message = "Flight not found" });

            return NoContent();
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFlightById(int id)
        {
            try
            {
                var result = await _flightService.GetFlightById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllFlights()
        {
            var result = await _flightService.GetAllFlights();
            return Ok(result);
        }
        
        [HttpPost("filter")]
        [Authorize]
        public async Task<IActionResult> GetFlightsByFilter([FromBody] FlightFilterRequest request)
        {
            try
            {
                var result = await _flightService.GetFlightsByFilter(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
