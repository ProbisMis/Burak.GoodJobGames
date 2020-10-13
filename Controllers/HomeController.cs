using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace GoodJobGames.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("health-check")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            return StatusCode((int)HttpStatusCode.OK, Environment.MachineName);
        }

        [HttpGet("")]
        public RedirectResult Home()
        {
            return Redirect($"{Request.Scheme}://{Request.Host.ToUriComponent()}/swagger");
        }
    }
}