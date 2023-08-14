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

        [HttpGet("overview/{gardenId}")]
        public ActionResult<ResponseDevices> GetOverview(string gardenId)
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            ResponseDevices response = _deviceService.GetOverview(userData, gardenId);
            return Ok(response);
        }

        [HttpPost("detailed/{gardenId}")]
        public ActionResult<ResponseDevices> GetDetailed(string gardenId, TimeFrame timeFrame)
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            ResponseDevices response = _deviceService.GetDetailed(userData, gardenId, timeFrame);
            return Ok(response);
        }

        [HttpGet("garden")]
        public ActionResult<GardenResponseModel> GetGardenData()
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            GardenResponseModel response = _userService.GetGardenData(userData);
            return Ok(response);
        }

        [HttpGet("users/{gardenId}")]
        public ActionResult<UserList> GetUserList(string gardenId)
        {
            UserList response = _userService.GetUserList(gardenId);
            return Ok(response);
        }

        [HttpPost("register")]
        public ActionResult<UserData> CreateNewUser()
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            _userService.SaveNewUser(userData);
            return Ok();
        }

        [HttpGet("accessrequest/{gardenId}")]
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

        [HttpGet("changestatus/{gardenId}/{userId}")]
        public ActionResult<UserData> Changestatus(string gardenId, string userId)
        {
            _userService.Changestatus(gardenId, userId);
            return Ok();
        }

        [HttpGet("bridges/{gardenId}")]
        public ActionResult<List<string>> GetBridges(string gardenId)
        {
            return Ok(_userService.GetBridges(gardenId));
        }
    }
}
