using API.Hub;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.User;
using Shared;
using Shared.DeviceModels;
using Shared.Enums;
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

    [HttpGet("{gardenId}/{deviceId}/{timeFrameId}")]
    public ActionResult<Device> GetSensorValues(string gardenId, string deviceId, int timeFrameId)
    {
        Device devices = _deviceService.GetSensorValues(gardenId, deviceId, (TimeFrameId)timeFrameId);
        return Ok(devices);
    }

    [HttpPost("{gardenId}/{deviceId}")]
    public ActionResult<DetailedChartData> GetDetailedTimeFrame(string gardenId, string deviceId, TimeFrame timeFrame)
    {
        DetailedChartData chartData = _deviceService.GetDetailedTimeFrame(gardenId, deviceId, timeFrame);
        return chartData;
    }

    [HttpPost("manual")]
    public ActionResult<Device> PostNewManualValue(NewManualValueModel model)
    {
        Device devices = _deviceService.UploadNewValue(model);
        return Ok(devices);
    }

    [HttpDelete("{gardenId}/{entryId}")]
    public ActionResult DeleteManualEntry(string gardenId, string entryId)
    {
        _deviceService.DeleteManualEntry(gardenId, entryId);
        return Ok();
    }
}
