
using Microsoft.AspNetCore.Mvc;
using MS.Domain.Entities;
using MS.Service;

namespace MS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyEntityController : ControllerBase
    {
        private readonly MyService _service;

        public MyEntityController(MyService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEntities()
        {
            try
            {
                var entities = await _service.GetAllEntitiesAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] MyEntity entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity is null.");
            }

            try
            {
                await _service.CreateEntityAsync(entity);
                return CreatedAtAction(nameof(GetAllEntities), new { id = entity._id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}