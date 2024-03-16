using API.Hub;
using API.Interfaces;
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

        [HttpGet("{gardenId}/users")]
        public ActionResult<UserList> GetUserList(string gardenId)
        {
            UserList response = _userService.GetUserList(gardenId);
            return Ok(response);
        }
    }
}
