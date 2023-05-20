using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;

namespace MainService.Controllers
{
    [ApiController]
    [Route("devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;

        public DeviceController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
        }

        [HttpGet("{rpiid}")]
        public ActionResult<RPIdata> GetRpiData(String rpiid)
        {
            Console.WriteLine(rpiid);
            RPIdata data = _deviceService.GetRPI(rpiid);
            return Ok(data);
        }
    }
}
