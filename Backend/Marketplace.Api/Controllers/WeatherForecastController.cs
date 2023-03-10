using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public ActionResult<Health> Health()
    {
        var health = new Health(
            $"{Request.Scheme}://{Request.Host}{Request.PathBase}",
            Directory.GetCurrentDirectory(),
            Directory.GetCurrentDirectory() + @"\auth.json",
            DateTime.Now.ToShortDateString().Split("/")
        );
        return Ok(health);
    }
}