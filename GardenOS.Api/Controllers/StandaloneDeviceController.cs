using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;

namespace MainService.Controllers
{
    [ApiController]
    [Route("standalone")]
    public class StandaloneDeviceController(IStandaloneService _standaloneQueryService) : ControllerBase
    {

        [HttpPost()]
        public ActionResult<ControllerResponseModel> PostNewDeviceData(DeviceInput data)
        {
            bool IsTaskSuccess = _standaloneQueryService.StoreData(data);
            if (IsTaskSuccess)
                return Ok();
            return BadRequest();
        }
    }
}
