 
using Microsoft.AspNetCore.Mvc;
using MS.Service;

namespace MS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RabbitMqController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;

        public RabbitMqController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;

        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            if(string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }
            _rabbitMqService.SendMessageAsync(message);
            return Ok(new { Message = message });
        }
    }
}