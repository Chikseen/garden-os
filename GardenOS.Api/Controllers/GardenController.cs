using API.Interfaces;
using API.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.User;
using Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("garden")]
    public class GardenController(
        IHubContext<MainHub, IMainHub> questionHub,
        IDeviceService _deviceService) : ControllerBase
    {
        private readonly UserService _userService = new();

        [HttpPost("{gardenId}/detailed")]
        public ActionResult<ResponseDevices> GetDetailed(string gardenId, TimeFrame timeFrame)
        {
            ResponseDevices response = _deviceService.GetDetailed(gardenId, timeFrame);
            return Ok(response);
        }

        [HttpGet("{gardenId}/users")]
        public ActionResult<UserList> GetUserList(string gardenId)
        {
            UserList response = _userService.GetUserList(gardenId);
            return Ok(response);
        }

        [HttpGet("{gardenId}/info")]
        public ActionResult<GardenInfo> GetGardenInfo(string gardenId)
        {
            GardenInfo response = _deviceService.GetGardenInfo(gardenId);

            return Ok(response);
        }
    }
}
