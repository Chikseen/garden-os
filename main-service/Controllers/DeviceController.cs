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
        private DeviceService _deviceService;
        private UserService _userService;

        public DeviceController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpGet("{rpiid}/metadata")]
        public ActionResult<RPIData> GetRpiMeta(String rpiid)
        {
            String? apiKey = _userService.GetApiKey(Request);

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIData? data = _deviceService.GetRpiMeta(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpGet("{rpiid}")]
        public ActionResult<RPIDevices> GetRpiDevices(String rpiid)
        {
            String? apiKey = _userService.GetApiKey(Request);

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIDevices? data = _deviceService.GetRPIDevices(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpPost("{rpiid}/save")]
        public ActionResult<ResponseDevices> SaveDataToDB(String rpiid, SaveDataRequest data)
        {
            String apiKey = _userService.GetApiKey(Request);
            Garden garden = new Garden();
            garden.SetGardenIdByRPI(rpiid);

            ResponseDevices? response = _deviceService.SaveDataToDB(data, rpiid, apiKey!);

            if (response == null)
                return BadRequest();

            _hubContext.Clients.Group(garden.Id).SendCurrentDeviceData(response);

            return Ok(response);
        }

        [HttpPost("status")]
        public ActionResult<ResponseDevices> SetStatus(DeveiceStatus status)
        {
            String apiKey = _userService.GetApiKey(Request);
            Garden garden = new Garden();
            garden.SetGardenIdByRPI(status.RpiId);

            DeveiceStatus response = _deviceService.SetStatus(status);

            _hubContext.Clients.Group(garden.Id).NewDeviceStatus(response);

            return Ok(response);
        }

        [HttpGet("status/{gardenId}")]
        public ActionResult<DeveiceStatus> GetStatus(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            DeveiceStatus response = _deviceService.GetStatus(gardenId);

            return Ok(response);
        }
        [HttpGet("status/{gardenId}/log")]
        public ActionResult<List<DeveiceStatus>> GetStatusLog(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            List<DeveiceStatus> response = _deviceService.GetStatusLog(gardenId);

            return Ok(response);
        }
    }
}
