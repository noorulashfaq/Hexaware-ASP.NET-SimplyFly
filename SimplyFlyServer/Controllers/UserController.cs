using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult<UserResponse>> AddUser(UserRequest request)
        {
            
            try
            {
                var result = await _userService.AddUser(request);
                return Created("", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); 
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<UserResponse>> UpdateUser(int id, UserRequest request)
        {
            try
            {
                var result = await _userService.UpdateUser(id, request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Update Error: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUser(id);
                if (deleted)
                    return NoContent();
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Delete Error: {e.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }


    }
}
