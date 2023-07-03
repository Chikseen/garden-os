using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MainService.Hub;
using Services.Device;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using Services.User;

namespace MainService.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private DeviceService _deviceService;
        private UserService _userService;

        public MainController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _deviceService = new();
            _userService = new();
        }

        [HttpGet()]
        public async Task<UserData> Get()
        {
            var t = await _userService.GetUserDataFromKeycloak(Request);
            return t;
        }

        [HttpGet("ledToggle")]
        public OkResult ledToggle()
        {
            //WebInput.toggleLed();

            // I did not forgot to remove the apikey its an dummy key so dont be hyped
            String rpiid = "1a667139-d648-4745-8529-a296c6de6f05";
            String apiKey = "OExfKUsFUh8bpVaR8soHNGhvFcwMXAcsLLQazmzdDumn0nSKMne2lsMJCgkPoEF2rZuUkWRMlQ7lK4WH3TNnTe16adkHeVCVwqhmZXASrcBaZzQ5j2qVQoubRDMiVbOW";

            ResponseDevices devices = _deviceService.GetDataLog(rpiid, apiKey, true)!;
            _hubContext.Clients.All.SendCurrentDeviceData(devices);
            Console.WriteLine("pts");
            return Ok();
        }
    }
}
