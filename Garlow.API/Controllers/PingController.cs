using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Garlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        public async Task<IActionResult> Ping()
        {
            return Ok(await Task.FromResult("pong"));
        }
    }
}
