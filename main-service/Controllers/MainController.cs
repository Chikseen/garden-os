using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.User;

namespace MainService.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly UserService _userService;

        public MainController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _userService = new();
        }

        [HttpGet()]
        public string Get()
        {
            return "If your reading this your cool";
        }

        [HttpGet("reboot/{rpiId}")]
        public ActionResult<string> Reboot(string rpiId)
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
        public OkResult LedToggle()
        {
            //WebInput.toggleLed();
            return Ok();
        }
    }
}
