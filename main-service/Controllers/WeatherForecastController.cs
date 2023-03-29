using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;

namespace main_service.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IHubContext<MainHub> hubContext;
    public WeatherForecastController(IHubContext<MainHub> questionHub)
    {
        this.hubContext = questionHub;
    }

    [HttpGet()]
    public String GetS()
    {
        this.hubContext
            .Clients
            .All
            .SendAsync("QuestionScoreChange", "hi", "sub");
        return "This is the main-service";
    }
}
