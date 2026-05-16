using Asp.Versioning;
using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Filters;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Armour/sets")]
public class ArmourSetController(IArmourSetService _armourSetService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetArmourSets()
    {
        var armourSets = await _armourSetService.GetAllArmourSetsAsync();
        return Ok(armourSets);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetArmourSetById(Guid id)
    {
        var armourSet = await _armourSetService.GetArmourSetByIdAsync(id);
        return Ok(armourSet);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateArmourSetDto>))]
    public async Task<IActionResult> CreateArmourSet(CreateArmourSetDto request)
    {
        var armourSet = await _armourSetService.CreateArmourSetAsync(request);
        return CreatedAtAction(nameof(GetArmourSetById), new { id = armourSet.Id }, armourSet);
    }

    //[HttpPost("range")]
    //[ServiceFilter(typeof(ValidateFilter<ICollection<CreateArmourSetDto>>))]
    //public async Task<IActionResult> CreateArmourRange(ICollection<CreateArmourSetDto> requests)
    //{
    //    var armourSets = await _armourSetService.CreateArmourRangeAsync(requests);
    //    return CreatedAtAction(nameof(GetArmourSets), null, armourSets);
    //}

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateArmourSet(Guid id, UpdateArmourSetDto request)
    {
        await _armourSetService.UpdateArmourSetAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteArmourSet(Guid id)
    {
        await _armourSetService.DeleteArmourSetAsync(id);
        return NoContent();
    }
}
