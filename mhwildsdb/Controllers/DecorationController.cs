using Asp.Versioning;
using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Filters;
using mhwildsdb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DecorationController(IDecorationService _decorationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDecorations()
    {
        var decorations = await _decorationService.GetAllDecorationsAsync();
        return Ok(decorations);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetDecorationById(Guid id)
    {
        var decoration = await _decorationService.GetDecorationByIdAsync(id);
        return Ok(decoration);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateDecorationDto>))]
    public async Task<IActionResult> CreateDecoration(CreateDecorationDto request)
    {
        var decoration = await _decorationService.CreateDecorationAsync(request);
        return CreatedAtAction(nameof(GetDecorationById), new { id = decoration.Id }, decoration);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateDecorationDto>>))]
    public async Task<IActionResult> CreateDecorationRange(ICollection<CreateDecorationDto> requests)
    {
        var decorations = await _decorationService.CreateDecorationRangeAsync(requests);
        return CreatedAtAction(nameof(GetDecorations), null, decorations);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateDecoration(Guid id, UpdateDecorationDto request)
    {
        await _decorationService.UpdateDecorationAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteDecoration(Guid id)
    {
        await _decorationService.DeleteDecorationAsync(id);
        return NoContent();
    }
}