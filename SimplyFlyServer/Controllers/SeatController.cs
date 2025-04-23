using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        [Authorize(Roles = "FlightOwner")]
        public async Task<ActionResult<SeatResponse>> AddSeat([FromBody] SeatRequest request)
        {
            var response = await _seatService.AddSeat(request);
            return CreatedAtAction(nameof(GetSeatById), new { seatId = response.SeatId }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "FlightOwner")]
        public async Task<ActionResult<SeatResponse>> UpdateSeat(int id, [FromBody] SeatRequest request)
        {
            var response = await _seatService.UpdateSeat(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "FlightOwner")]
        public async Task<ActionResult<SeatResponse>> DeleteSeat(int id)
        {
            var response = await _seatService.DeleteSeat(id);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAllSeats()
        {
            var response = await _seatService.GetAllSeats();
            return Ok(response);
        }

        [HttpGet("{seatId}")]
        [Authorize]
        public async Task<ActionResult<SeatResponse>> GetSeatById(int seatId)
        {
            var response = await _seatService.GetSeatById(seatId);
            return Ok(response);
        }

        [HttpPost("release-seats")]
        [Authorize(Roles = "FlightOwner")]
        public async Task<IActionResult> ReleaseSeatsForArrivedFlights()
        {
            await _seatService.ReleaseSeatsForArrivedFlights();
            return Ok("Seats released for arrived flights.");
        }

    }
}
