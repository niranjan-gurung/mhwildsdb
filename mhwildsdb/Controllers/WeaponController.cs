using Asp.Versioning;
using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Filters;
using mhwildsdb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeaponController(IWeaponService _weaponService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWeapons()
    {
        var weapons = await _weaponService.GetAllWeaponsAsync();
        return Ok(weapons);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetWeaponById(Guid id)
    {
        var weapon = await _weaponService.GetWeaponByIdAsync(id);
        return Ok(weapon);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateFilter<CreateWeaponDto>))]
    public async Task<IActionResult> CreateWeapon(CreateWeaponDto request)
    {
        var weapon = await _weaponService.CreateWeaponAsync(request);
        return CreatedAtAction(nameof(GetWeaponById), new { id = weapon.Id }, weapon);
    }

    [HttpPost("range")]
    [ServiceFilter(typeof(ValidateFilter<ICollection<CreateWeaponDto>>))]
    public async Task<IActionResult> CreateWeaponRange(ICollection<CreateWeaponDto> requests)
    {
        var weapons = await _weaponService.CreateWeaponRangeAsync(requests);
        return CreatedAtAction(nameof(GetWeapons), null, weapons);
    }
}
