using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using API.Hub;
using Services.User;
using Shared.Models;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(
        IHubContext<MainHub, IMainHub> questionHub,
        IDeviceService _deviceService) : ControllerBase
    {
        private readonly UserService _userService = new();

        [HttpGet("{gardenId}")]
        public ActionResult<UserData> GetuserData(string gardenId)
        {
            UserData userData = HttpContext.Features.Get<UserData>()!;
            userData.AddGardenData(gardenId);
            return Ok(userData);
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

        [HttpPost("changestatus")]
        public ActionResult<UserData> Changestatus(ChangeUserStatusModel model)
        {
            _userService.Changestatus(model);
            return Ok();
        }
    }
}
