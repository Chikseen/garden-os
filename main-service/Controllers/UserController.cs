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

        [HttpGet("overview/{gardenId}")]
        public ActionResult<ResponseDevices> GetOverview(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            ResponseDevices response = _deviceService.GetOverview(userData, gardenId);
            return Ok(response);
        }

        [HttpPost("detailed/{gardenId}")]
        public ActionResult<ResponseDevices> GetDetailed(String gardenId, TimeFrame timeFrame)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            ResponseDevices response = _deviceService.GetDetailed(userData, gardenId, timeFrame);
            return Ok(response);
        }

        [HttpGet("garden")]
        public ActionResult<GardenResponseModel> GetGardenData()
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            GardenResponseModel response = _userService.GetGardenData(userData);
            return Ok(response);
        }

        [HttpGet("users/{gardenId}")]
        public ActionResult<UserList> GetUserList(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            UserList response = _userService.GetUserList(userData, gardenId);
            return Ok(response);
        }

        [HttpPost("register")]
        public ActionResult<UserData> CreateNewUser()
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            _userService.SaveNewUser(userData);
            return Ok();
        }

        [HttpGet("accessrequest/{gardenId}")]
        public ActionResult<UserData> AccessRequest(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            _userService.AccessRequest(userData, gardenId);
            return Ok();
        }

        [HttpGet("requested")]
        public ActionResult<List<String>> GetRequestedGardenList()
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            return Ok(_userService.GetRequestedGardenList(userData));
        }

        [HttpGet("changestatus/{gardenId}/{userId}")]
        public ActionResult<UserData> Changestatus(String gardenId, String userId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();
            _userService.Changestatus(gardenId, userId);
            return Ok();
        }

        [HttpGet("bridges/{gardenId}")]
        public ActionResult<List<String>> GetBridges(String gardenId)
        {
            UserData userData = _userService.GetUserDataFromKeycloak(Request).Result;
            if (!userData.IsAuthorized)
                return Unauthorized();

            return Ok(_userService.GetBridges(gardenId));
        }
    }
}
