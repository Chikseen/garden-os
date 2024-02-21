using API.Interfaces;
using ESP_sensor.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace MainService.Controllers
{
    [ApiController]
    [Route("standalone")]
    public class StandaloneDeviceController(IStandaloneService _standaloneQueryService) : ControllerBase
    {

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
