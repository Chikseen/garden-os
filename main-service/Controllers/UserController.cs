using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Services.User;

namespace MainService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;
        private UserService _userService;

        public UserController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpPost("{userid}/datalog")]
        public ActionResult<ResponseDevices> GetDataLog(String userid, TimeFrame? timeFrame = null)
        {
            String? apiKey = _userService.GetApiKey(Request);

            Console.WriteLine("NEW DATA SEND TO FE");

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            ResponseDevices? response;
            if (timeFrame == null)
                response = _deviceService.GetDataLog(
                    id: userid,
                    ApiKey: apiKey,
                    isUser: true
                );
            else
                response = _deviceService.GetDataLog(
                    id: userid,
                    ApiKey: apiKey,
                    timeframe: timeFrame,
                    isUser: true
                );

            if (response == null)
                return BadRequest();

            return Ok(response);
        }
    }
}
