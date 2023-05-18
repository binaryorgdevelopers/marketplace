using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    private readonly DateTime _startTime;

    public HomeController()
    {
        _startTime = Process.GetCurrentProcess().StartTime;
    }

    [HttpGet]
    public ActionResult<Health> Health()
    {
        var uptime = DateTime.Now - _startTime;
        var health = new Health(
            $"{Request.Scheme}://{Request.Host}{Request.PathBase}",
            Directory.GetCurrentDirectory(),
            DateTime.Now.ToShortDateString().Split("/"),
            uptime
        );
        return Ok(health);
    }
}