using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using MainService.Hardware;
using MainService.DB;

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
        public DevicesData Get()
        {
            //_hubContext.Clients.All.SendMyEvent("hi");
            //_db.query("INSERT INTO datalog (deviceid, rawvalue) VALUES ('550e8400-e29b-11d4-a716-446655440000', 255);");
            //_db.query("SELECT * FROM datalog;");
            return MainHardware._data;
        }
        [HttpGet("ledToggle")]
        public OkResult ledToggle()
        {
            WebInput.toggleLed();
            return Ok();
        }
    }
}
