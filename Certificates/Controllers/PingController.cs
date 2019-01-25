using Certificates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [Route("api/ping")]
    [Route("api/status")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET ping
        [HttpGet]
        public ActionResult<Pong> Get()
        {
            return new Pong("Service is Up and Running!");
        }
    }
}
