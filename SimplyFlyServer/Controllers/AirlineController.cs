using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddAirline([FromBody] AirlineRequest request)
        {
            try
            {
                var result = await _airlineService.AddAirline(request);
                return CreatedAtAction(nameof(GetAirlineById), new { id = result.AirlineId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateAirline(int id, [FromBody] AirlineRequest request)
        {
            try
            {
                var result = await _airlineService.UpdateAirline(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteAirline(int id)
        {
            var success = await _airlineService.DeleteAirline(id);
            if (!success)
                return NotFound(new { message = "Airline not found" });

            return NoContent();
        }

        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAirlineById(int id)
        {
            try
            {
                var result = await _airlineService.GetAirlineById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

      
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAirlines()
        {
            var result = await _airlineService.GetAllAirlines();
            return Ok(result);
        }
    }
}
