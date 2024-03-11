using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers;
/*
[ApiController]
[Route("manualData")]
public class GardenController(IManualService _manualService) : ControllerBase
{
    [HttpPost("{gardenId}")]
    public ActionResult SaveManualData(string gardenId)
    {
        UserData userData = HttpContext.Features.Get<UserData>()!;
        ResponseDevices response = _manualService.SaveManualData(userData, gardenId);
        return Ok();
    }
}
*/