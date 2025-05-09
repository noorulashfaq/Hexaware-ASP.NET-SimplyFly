﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin,FlightOwner")]

        public async Task<IActionResult> AddAircraft([FromBody] AircraftRequest request)
        {
            try
            {
                var result = await _aircraftService.AddAircraft(request);
                return CreatedAtAction(nameof(GetAircraftById), new { id = result.AircraftId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

     
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<IActionResult> UpdateAircraft(int id, [FromBody] AircraftRequest request)
        {
            try
            {
                var result = await _aircraftService.UpdateAircraft(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,FlightOwner")]
        public async Task<IActionResult> DeleteAircraft(int id)
        {
            var success = await _aircraftService.DeleteAircraft(id);
            if (!success)
                return NotFound(new { message = "Aircraft not found" });

            return NoContent();
        }

     
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAircraftById(int id)
        {
            try
            {
                var result = await _aircraftService.GetAircraftById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

   
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAircrafts()
        {
            var result = await _aircraftService.GetAllAircrafts();
            return Ok(result);
        }
    }
}
