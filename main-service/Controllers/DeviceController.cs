using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Services.User;

namespace MainService.Controllers
{
    [ApiController]
    [Route("devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly DeviceService _deviceService;
        private readonly UserService _userService;

        public DeviceController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpGet("{rpiid}/metadata")]
        public ActionResult<RPIData> GetRpiMeta(string rpiid)
        {
            string? apiKey = _userService.GetApiKey(Request);

            if (string.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIData? data = _deviceService.GetRpiMeta(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpGet("{rpiid}")]
        public ActionResult<RPIDevices> GetRpiDevices(string rpiid)
        {
            string? apiKey = _userService.GetApiKey(Request);

            if (string.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIDevices? data = _deviceService.GetRPIDevices(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpPost("{rpiid}/save")]
        public ActionResult<ResponseDevices> SaveDataToDB(string rpiid, SaveDataRequest data)
        {
            string apiKey = _userService.GetApiKey(Request);
            Garden garden = new ();
            garden.SetGardenIdByRPI(rpiid);

            ReponseDevice? response = _deviceService.SaveDataToDB(data, rpiid, apiKey!);

            if (response == null)
                return BadRequest();

            _hubContext.Clients.Group(garden.Id).SendCurrentDeviceData(response);

            return Ok(response);
        }

        [HttpPost("status")]
        public ActionResult<ResponseDevices> SetStatus(DeveiceStatus status)
        {
            Garden garden = new();
            garden.SetGardenIdByRPI(status.RpiId);

            DeveiceStatus response = _deviceService.SetStatus(status);

            _hubContext.Clients.Group(garden.Id).NewDeviceStatus(response);

            return Ok(response);
        }
    }
}
