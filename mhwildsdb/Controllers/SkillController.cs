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

        [HttpGet("skills")]
        public async Task<IActionResult> GetAllSkillsAsync()
        {
            var skills = await _skillService.GetAllSkillsAsync();
            return Ok(skills);
        }

        [HttpGet("skills/{id:Guid}")]
        public async Task<IActionResult> GetSkillByIdAsync(Guid id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            
            if (skill is null) 
                return NotFound();

            return Ok(skill);
        }

        [HttpPost("skills")]
        public async Task<IActionResult> CreateSkillAsync(CreateSkillDto command)
        {
            var skill = await _skillService.CreateSkillAsync(command);
            return CreatedAtAction(nameof(GetSkillByIdAsync), new { id = skill.Id }, skill);
        }

        [HttpPut("skills/{id:Guid}")]
        public async Task<IActionResult> UpdateSkillAsync(Guid id, UpdateSkillDto command)
        {
            await _skillService.UpdateSkillAsync(id, command);
            return NoContent();
        }

        [HttpDelete("skills/{id:Guid}")]
        public async Task<IActionResult> DeleteSkillAsync(Guid id)
        {
            await _skillService.DeleteSkillAsync(id);
            return NoContent();
        }
    }
}
