using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Context
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> Create([FromBody] NotificatinRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message is required.");

            var result = await _notificationService.AddNotification(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetAll()
        {
            var result = await _notificationService.GetAllNotifications();
            return Ok(result);
        }

        [HttpPut("read/{id}")]
        public async Task<ActionResult<NotificationResponse>> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsRead(id);
            return Ok(result);
        }
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _notificationService.DeleteNotification(id);
            if (!success)
                return NotFound(new { Message = "Notification not found" });

            return NoContent();
        }
    }
}
