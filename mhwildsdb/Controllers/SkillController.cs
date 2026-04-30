using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SkillController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Skills page!!");
        }
    }
}
