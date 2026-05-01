using FluentValidation;
using mhwildsdb.DTOs;
using mhwildsdb.Exceptions;
using mhwildsdb.Extensions;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace mhwildsdb.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SkillController(
    ISkillService _skillService,
    ILogger<SkillController> _logger,
    IValidator<CreateSkillDto> _validator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllSkillsAsync()
    {
        var skills = await _skillService.GetAllSkillsAsync();

        if (skills is null)
            throw new NotFoundException("Skills", null);

        return Ok(skills);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetSkillByIdAsync(Guid id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        return Ok(skill);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkillAsync(CreateSkillDto command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateSkillDto: {@Errors}",
                validationResult.Errors.Select(e => e.ErrorMessage));

            validationResult.AddToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        var skill = await _skillService.CreateSkillAsync(command);
        
        if (skill is null)
            throw new BadRequestException("Failed to create skill.");

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
