using ESP_sensor.Models;
using MainService.Hub;
using MainService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace MainService.Controllers
{
    [ApiController]
    [Route("standalone")]
    public class StandaloneDeviceController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly StandaloneService _standaloneQueryService;

        public StandaloneDeviceController(IHubContext<MainHub, IMainHub> questionHub)
        {
            _hubContext = questionHub;
            _standaloneQueryService = new();
        }

        [HttpPost()]
        public ActionResult<ControllerResponseModel> PostNewDeviceData(StandaloneDevice data)
        {
            bool IsTaskSuccess = _standaloneQueryService.StoreData(data);
            if (IsTaskSuccess)
                return Ok();
            return BadRequest();
        }
    }
}
