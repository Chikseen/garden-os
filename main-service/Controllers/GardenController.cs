using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Services.User;
using shared_data.Models;

namespace MainService.Controllers
{
	[ApiController]
	[Route("garden")]
	public class GardenController : ControllerBase
	{
		private readonly IHubContext<MainHub, IMainHub> _hubContext;
		private readonly DeviceService _deviceService;
		private readonly UserService _userService;

		public GardenController(IHubContext<MainHub, IMainHub> questionHub)
		{
			_hubContext = questionHub;
			_deviceService = new();
			_userService = new();
		}

		[HttpGet("{gardenId}/overview")]
		public ActionResult<ResponseDevices> GetOverview(string gardenId)
		{
			UserData userData = HttpContext.Features.Get<UserData>()!;
			ResponseDevices response = _deviceService.GetOverview(userData, gardenId);
			return Ok(response);
		}

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

		[HttpGet("{gardenId}/bridges")]
		public ActionResult<List<string>> GetBridges(string gardenId)
		{
			return Ok(_userService.GetBridges(gardenId));
		}

		[HttpGet("{gardenId}/status")]
		public ActionResult<DeveiceStatus> GetStatus(string gardenId)
		{
			DeveiceStatus response = _deviceService.GetStatus(gardenId);

			return Ok(response);
		}

		[HttpGet("{gardenId}/status/log")]
		public ActionResult<List<DeveiceStatus>> GetStatusLog(string gardenId)
		{
			List<DeveiceStatus> response = _deviceService.GetStatusLog(gardenId);

			return Ok(response);
		}

		[HttpPatch("changedevice")]
		public ActionResult<List<DeveiceStatus>> PatchDevice(PatchDeviceRequest device)
		{
			_deviceService.PatchDevice(device);
			return Ok();
		}

		[HttpGet("{gardenId}/info")]
		public ActionResult<GardenInfo> GetGardenInfo(string gardenId)
		{
			GardenInfo response = _deviceService.GetGardenInfo(gardenId);

			return Ok(response);
		}
	}
}
