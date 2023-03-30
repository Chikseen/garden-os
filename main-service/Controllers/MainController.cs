using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;

namespace main_service.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        public MainController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
        }

        [HttpGet()]
        public String Get()
        {
            _hubContext.Clients.All.SendMyEvent("hi");
            return "This is the main-service";
        }
    }
}
