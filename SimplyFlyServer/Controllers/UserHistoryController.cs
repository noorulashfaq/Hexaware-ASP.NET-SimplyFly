using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService _historyService;

        public UserHistoryController(IUserHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserHistoryResponse>>> GetUserHistory(int userId)
        {
            var history = await _historyService.GetUserHistory(userId);
            return Ok(history);
        }
    }
}
