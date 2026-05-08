using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.Filters;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SkillController(ISkillService _skillService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllSkillsAsync()
    {
        var skills = await _skillService.GetAllSkillsAsync();
        return Ok(skills);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetSkillByIdAsync(Guid id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);
        return Ok(skill);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateSkillDto>))]
    public async Task<IActionResult> CreateSkillAsync(CreateSkillDto request)
    {
        var skill = await _skillService.CreateSkillAsync(request);
        return CreatedAtAction(nameof(GetSkillByIdAsync), new { id = skill.Id }, skill);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateSkillDto>>))]
    public async Task<IActionResult> CreateSkillRangeAsync(ICollection<CreateSkillDto> requests)
    {
        var skills = await _skillService.CreateSkillRangeAsync(requests);
        return CreatedAtAction(nameof(GetAllSkillsAsync), skills);
    }

    [HttpPut("{id:Guid}")]
    //[ServiceFilter(typeof(ValidateFilter<UpdateSkillDto>))]
    public async Task<IActionResult> UpdateSkillAsync(Guid id, UpdateSkillDto request)
    {
        await _skillService.UpdateSkillAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteSkillAsync(Guid id)
    {
        await _skillService.DeleteSkillAsync(id);
        return NoContent();
    }
}
