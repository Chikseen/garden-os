using API.Hub;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.User;
using Shared.DeviceModels;
using Shared.Models;

namespace API.Controllers;

[ApiController]
[Route("devices")]
public class DeviceController(
    IHubContext<MainHub, IMainHub> _hubContext,
    IDeviceService _deviceService) : ControllerBase
{
    private readonly UserService _userService = new();

    [HttpPost()]
    public ActionResult<ControllerResponseModel> PostNewDeviceData(DeviceInput data)
    {
        _deviceService.StoreData(data);
        return Ok();
    }

    [HttpPost("create")]
    public ActionResult CreateNewDevice(DeviceCreateModel model)
    {
        _deviceService.CreateNewDevice(model);
        return Ok();
    }

    [HttpGet("{gardenId}")]
    public ActionResult<List<DeviceMeta>> GetAllDevices(string gardenId)
    {
        List<DeviceMeta> devices = _deviceService.GetAllDevices(gardenId);
        return Ok(devices);
    }

    [HttpGet("{gardenId}/{deviceId}")]
    public ActionResult<List<DeviceSensorMeta>> GetDevice(string deviceId)
    {
        List<DeviceSensorMeta> devices = _deviceService.GetDevice(deviceId);
        return Ok(devices);
    }

    [HttpGet("{gardenId}/{deviceId}/{sensorId}")]
    public ActionResult<ReponseDevice> GetLastSensorValue(string gardenId, string deviceId, string sensorId)
    {
        ReponseDevice devices = _deviceService.GetLastSensorValue(gardenId, deviceId, sensorId);
        return Ok(devices);
    }
}
