using Certificates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("api/ping")]
    [Route("api/status")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET ping
        [HttpGet]
        public ActionResult<Pong> Get()
        {
            return new Pong(ControllerContext.ToString());
        }
    }
}
