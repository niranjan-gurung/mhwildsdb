using mhwildsdb.DTOs;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SkillController(
        ISkillService _skillService,
        ILogger<SkillController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSkillsAsync()
        {
            var skills = await _skillService.GetAllSkillsAsync();

            if (skills is null)
                throw new Exception("test exception");

            return Ok(skills);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetSkillByIdAsync(Guid id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            
            if (skill is null) 
                return NotFound();

            return Ok(skill);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkillAsync(CreateSkillDto command)
        {
            var skill = await _skillService.CreateSkillAsync(command);
            return CreatedAtAction(nameof(GetSkillByIdAsync), new { id = skill.Id }, skill);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateSkillAsync(Guid id, UpdateSkillDto command)
        {
            await _skillService.UpdateSkillAsync(id, command);
            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteSkillAsync(Guid id)
        {
            await _skillService.DeleteSkillAsync(id);
            return NoContent();
        }
    }
}
