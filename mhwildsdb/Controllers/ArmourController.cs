using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ArmourController : ControllerBase
{
    public async Task<IActionResult> GetArmours()
    {
        return Ok();
    }
}
