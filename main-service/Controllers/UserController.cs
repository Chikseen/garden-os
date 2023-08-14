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
        private readonly DeviceService _deviceService;
        private readonly UserService _userService;

        public UserController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpGet("garden")]
        public ActionResult<GardenResponseModel> GetGardenData()
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            GardenResponseModel response = _userService.GetGardenData(userData);
            return Ok(response);
        }

        [HttpPost("register")]
        public ActionResult<UserData> CreateNewUser()
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            _userService.SaveNewUser(userData);
            return Ok();
        }

        [HttpGet("{gardenId}/accessrequest")]
        public ActionResult<UserData> AccessRequest(string gardenId)
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            _userService.AccessRequest(userData, gardenId);
            return Ok();
        }

        [HttpGet("requested")]
        public ActionResult<List<string>> GetRequestedGardenList()
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            return Ok(_userService.GetRequestedGardenList(userData));
        }

        [HttpGet("{gardenId}/changestatus/{userId}")]
        public ActionResult<UserData> Changestatus(string gardenId, string userId)
        {
            _userService.Changestatus(gardenId, userId);
            return Ok();
        }
    }
}
