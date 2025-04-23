using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancellationController : ControllerBase
    {
        private readonly ICancellationService _cancellationService;

        public CancellationController(ICancellationService cancellationService)
        {
            _cancellationService = cancellationService;
        }

        [HttpPost("cancel")]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult<CancellationResponse>> CancelBooking([FromBody] CancellationRequest request)
        {
            if (request == null || request.BookingId <= 0)
                return BadRequest("Invalid cancellation request.");

            try
            {
                var result = await _cancellationService.CancelBooking(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CancellationResponse>>> GetAll()
        {
            var result = await _cancellationService.GetAllCancellations();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CancellationResponse>> GetById(int id)
        {
            var result = await _cancellationService.GetCancellationById(id);
            return Ok(result);
        }

        [HttpPut("{id}/refund-status")]
        [Authorize]
        public async Task<ActionResult<CancellationResponse>> UpdateRefundStatus(int id, [FromQuery] string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                return BadRequest("Refund status cannot be empty.");

            try
            {
                var result = await _cancellationService.UpdateRefundStatus(id, newStatus);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

    }
}
