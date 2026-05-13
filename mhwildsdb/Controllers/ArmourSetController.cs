using Asp.Versioning;
using mhwildsdb.Services;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Armour/sets")]
public class ArmourSetController(IArmourSetService _armourSetService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllArmourSets()
    {
        return Ok();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetArmourSetById(Guid id)
    {
        return Ok(id);
    }

    // TODO: additional crud..

    // POST

    // PUT

    // DELETE

}
