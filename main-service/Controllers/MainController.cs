using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using MainService.Hardware;

namespace MainService.Controllers
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
        public HardwareData Get()
        {
            //_hubContext.Clients.All.SendMyEvent("hi");
            return MainHardware._data;
        }
    }
}
