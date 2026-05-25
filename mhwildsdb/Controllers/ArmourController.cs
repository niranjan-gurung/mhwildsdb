using Asp.Versioning;
using mhwildsdb.DTOs.Armours;
using mhwildsdb.Filters;
using mhwildsdb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ArmourController(IArmourService _armourService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetArmours()
    {
        var armours = await _armourService.GetAllArmoursAsync();
        return Ok(armours);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetArmourById(Guid id)
    {
        var armour = await _armourService.GetArmourByIdAsync(id);
        return Ok(armour);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateArmourDto>))]
    public async Task<IActionResult> CreateArmour(CreateArmourDto request)
    {
        var armour = await _armourService.CreateArmourAsync(request);
        return CreatedAtAction(nameof(GetArmourById), new { id = armour.Id }, armour);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateArmourDto>>))]
    public async Task<IActionResult> CreateArmourRange(ICollection<CreateArmourDto> requests)
    {
        var armours = await _armourService.CreateArmourRangeAsync(requests);
        return CreatedAtAction(nameof(GetArmours), null, armours);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateArmour(Guid id, UpdateArmourDto request)
    {
        await _armourService.UpdateArmourAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteArmour(Guid id)
    {
        await _armourService.DeleteArmourAsync(id);
        return NoContent();
    }
}
