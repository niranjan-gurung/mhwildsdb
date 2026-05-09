using Asp.Versioning;
using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.Filters;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SkillController(ISkillService _skillService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSkills()
    {
        var skills = await _skillService.GetAllSkillsAsync();
        return Ok(skills);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetSkillById(Guid id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);
        return Ok(skill);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateSkillDto>))]
    public async Task<IActionResult> CreateSkill(CreateSkillDto request)
    {
        var skill = await _skillService.CreateSkillAsync(request);
        return CreatedAtAction(nameof(GetSkillById), new { id = skill.Id }, skill);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateSkillDto>>))]
    public async Task<IActionResult> CreateSkillRange(ICollection<CreateSkillDto> requests)
    {
        var skills = await _skillService.CreateSkillRangeAsync(requests);
        return CreatedAtAction(nameof(GetSkills), skills);
    }

    [HttpPut("{id:Guid}")]
    //[ServiceFilter(typeof(ValidateFilter<UpdateSkillDto>))]
    public async Task<IActionResult> UpdateSkill(Guid id, UpdateSkillDto request)
    {
        await _skillService.UpdateSkillAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteSkill(Guid id)
    {
        await _skillService.DeleteSkillAsync(id);
        return NoContent();
    }
}
