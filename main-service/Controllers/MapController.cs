using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Microsoft.Extensions.Primitives;
using Services.User;
using Services.Maps;

namespace MainService.Controllers
{
    [ApiController]
    [Route("maps")]
    public class MapController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;
        private UserService _userService;
        private MapsService _mapsService;


        public MapController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
            _mapsService = new();
        }

        [HttpPost("{mapid}")]
        public ActionResult<MapJSON> SaveMap(String mapid, MapJSON data)
        {
            String? apiKey = _userService.GetApiKey(Request);

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            MapJSON? response = _mapsService.SaveMap(mapid, data);

            // _hubContext.Clients.All.SendMyEvent(response);

            return Ok(response);
        }

        [HttpGet("{mapid}")]
        public ActionResult<MapJSON> GetMap(String mapid)
        {
            String? apiKey = _userService.GetApiKey(Request);

            if (String.IsNullOrEmpty(apiKey))
                return Unauthorized();

            MapJSON? response = _mapsService.GetMap(mapid);

            if (response == null)
                return BadRequest();

            // _hubContext.Clients.All.SendMyEvent(response);

            return Ok(response);
        }
    }
}
