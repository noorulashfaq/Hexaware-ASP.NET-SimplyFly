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
    public class FlightRouteController : ControllerBase
    {
        private readonly IRouteservice _routeService;

        public FlightRouteController(IRouteservice routeService)
        {
            _routeService = routeService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<ActionResult<FlightRouteResponse>> AddRoute(FlightRouteRequest route)
        {
            try
            {
                var result = await _routeService.AddRoute(route);
                return CreatedAtAction(nameof(GetRouteById), new { id = result.RouteId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FlightRouteResponse>>> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutes();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FlightRouteResponse>> GetRouteById(int id)
        {
            try
            {
                var route = await _routeService.GetRouteById(id);
                return Ok(route);
            }
            catch (Exception ex)
            {
                return NotFound($"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<ActionResult<FlightRouteResponse>> UpdateRoute(int id, FlightRouteRequest route)
        {
            try
            {
                var result = await _routeService.UpdateRoute(id, route);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<ActionResult> DeleteRoute(int id)
        {
            var success = await _routeService.DeleteRoute(id);
            if (!success)
                return NotFound("Route not found");

            return NoContent();
        }
    }
}
