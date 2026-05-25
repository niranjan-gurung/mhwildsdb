using Asp.Versioning;
using mhwildsdb.DTOs.Charms;
using mhwildsdb.Filters;
using mhwildsdb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CharmController(ICharmService _charmService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCharms()
    {
        var charms = await _charmService.GetAllCharmsAsync();
        return Ok(charms);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCharmById(Guid id)
    {
        var charm = await _charmService.GetCharmByIdAsync(id);
        return Ok(charm);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateCharmDto>))]
    public async Task<IActionResult> CreateCharm(CreateCharmDto request)
    {
        var charm = await _charmService.CreateCharmAsync(request);
        return CreatedAtAction(nameof(GetCharmById), new { id = charm.Id }, charm);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateCharmDto>>))]
    public async Task<IActionResult> CreateCharmRange(ICollection<CreateCharmDto> requests)
    {
        var charms = await _charmService.CreateCharmRangeAsync(requests);
        return CreatedAtAction(nameof(GetCharms), null, charms);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateCharm(Guid id, UpdateCharmDto request)
    {
        await _charmService.UpdateCharmAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteCharm(Guid id)
    {
        await _charmService.DeleteCharmAsync(id);
        return NoContent();
    }
}
