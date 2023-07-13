using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using Services.User;

namespace MainService.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;
        private UserService _userService;

        public MainController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpGet()]
        public String Get()
        {
            return "If your reading this your cool";
        }

        [HttpGet("reboot/{rpiId}")]
        public ActionResult<String> Reboot(String rpiId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();

            if (!MainHub._rpiList.ContainsKey(rpiId))
                return BadRequest("The Hub is not registerd");

            _hubContext.Clients.Client(MainHub._rpiList[rpiId]).SendRebootRequest();
            return Ok("RebootRequest send");
        }

        [HttpGet("ledToggle")]
        public OkResult ledToggle()
        {
            //WebInput.toggleLed();
            return Ok();
        }
    }
}
