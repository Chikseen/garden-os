using Microsoft.AspNetCore.Mvc;

namespace web_proxy.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
     
    [HttpGet()]
    public String GetS()
    {
      return "This is the web-proxy";
    }
}
