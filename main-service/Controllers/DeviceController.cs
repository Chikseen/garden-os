using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Microsoft.Extensions.Primitives;
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
        public ActionResult<RPIdata> GetRpiMeta(String rpiid)
        {
            String? apiKey = _userService.GetApiKey(Request);

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIdata? data = _deviceService.GetRpiMeta(rpiid, apiKey!);

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
            String? apiKey = _userService.GetApiKey(Request);

            Console.WriteLine("NEW DATA SEND TO FE");


            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            ResponseDevices? response = _deviceService.SaveDataToDB(data, rpiid, apiKey!);

            if (response == null)
                return BadRequest();

            _hubContext.Clients.All.SendMyEvent(response);

            return Ok(response);
        }
    }
}
