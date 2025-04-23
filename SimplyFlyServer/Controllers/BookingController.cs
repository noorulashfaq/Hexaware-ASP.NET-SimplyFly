using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
       
            private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logging;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logging)
            {
                _bookingService = bookingService;
                _logging = logging;
        }

            [HttpPost]
        //[Route("AddBooking")]
        [Authorize(Roles = "Passenger")]

        public async Task<ActionResult<BookingResponse>> AddBooking([FromBody] BookingRequest request)
            {
                try
                {
                if (request == null)
                {
                    _logging.LogError("BookingRequest is null.");
                    return BadRequest("Request body cannot be null.");
                }

                var result = await _bookingService.AddBooking(request);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            }

            [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAllBookings()
            {
                var result = await _bookingService.GetAllBookings();
                return Ok(result);
            }

            [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookingResponse>> GetBookingById(int id)
            {
                var result = await _bookingService.GetBookingById(id);
                return Ok(result);
            }
    }
}
