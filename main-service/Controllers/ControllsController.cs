using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Services.User;
using MainService.Services;
using Shared.Models;

namespace MainService.Controllers
{
	[ApiController]
	[Route("controlls")]
	public class ControllsController : ControllerBase
	{
		private readonly IHubContext<MainHub, IMainHub> _hubContext;
		private readonly ControllsService _controllsService;
		private readonly UserService _userService;

		public ControllsController(IHubContext<MainHub, IMainHub> questionHub)
		{
			_hubContext = questionHub;
			_controllsService = new();
			_userService = new();
		}

		[HttpGet("{gardenId}")]
		public ActionResult<ControllerResponseModel> GetControlls(string gardenId)
		{
			ControllerResponseModel response = _controllsService.GetControllsOverview(gardenId);
			return Ok(response);
		}
	}
}
