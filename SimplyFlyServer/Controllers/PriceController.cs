using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }
        
        [HttpPost]
        [Authorize(Roles = "FlightOwner")]
        public async Task<ActionResult<PriceResponse>> AddPrice([FromBody] PriceRequest priceRequest)
        {
            var result = await _priceService.AddPrice(priceRequest);
            return CreatedAtAction(nameof(GetPriceById), new { id = result.PriceId }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "FlightOwner")]

        public async Task<ActionResult<PriceResponse>> UpdatePrice(int id, [FromBody] PriceRequest priceRequest)
        {
            var result = await _priceService.UpdatePrice(id, priceRequest);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "FlightOwner")]
        public async Task<ActionResult> DeletePrice(int id)
        {
            var success = await _priceService.DeletePrice(id);
            if (!success)
                return NotFound("Price not found");

            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PriceResponse>> GetPriceById(int id)
        {
            var result = await _priceService.GetPriceById(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<PriceResponse>>> GetAllPrices()
        {
            var result = await _priceService.GetAllPrices();
            return Ok(result);
        }
    }
}
