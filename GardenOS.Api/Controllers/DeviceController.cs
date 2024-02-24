using API.Interfaces;
using MainService.DB;
using MainService.Hub;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.User;
using Shared.DeviceModels;
using Shared.Models;
using System.Reflection;
using System.Text.Json;

namespace MainService.Controllers
{
    [ApiController]
    [Route("devices")]
    public class DeviceController(
        IHubContext<MainHub, IMainHub> _hubContext,
        IDeviceService _deviceService) : ControllerBase
    {
        private readonly UserService _userService = new();

        [HttpPost("{rpiId}/version")]
        public ActionResult<DeviceVersion> SetVersion(string rpiId, DeviceVersion version)
        {
            string query = QueryService.GetSetVersionQuery(rpiId, version.Build);
            MainDB.Query(query);
            using StreamReader reader = new("./build.json");
            var json = reader.ReadToEnd();
            DeviceVersion deviceversion = JsonSerializer.Deserialize<DeviceVersion>(json)!;
            if (int.Parse(version.Build) < int.Parse(version.Build))
            {
                GardenId garden = new();
                garden.SetGardenIdByRPI(rpiId);
                _hubContext.Clients.Group(garden.Id).NewVersion();
            }
            Console.WriteLine(json);
            return Ok();
        }

        [HttpPost()]
        public ActionResult<ControllerResponseModel> PostNewDeviceData(DeviceInput data)
        {
            _deviceService.StoreData(data);
            return Ok();
        }

        [HttpGet("{rpiid}/metadata")]
        public ActionResult<RPIData> GetRpiMeta(string rpiid)
        {
            string? apiKey = _userService.GetApiKey(Request);

            if (string.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIData? data = _deviceService.GetRpiMeta(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpGet("{rpiid}")]
        public ActionResult<RPIDevices> GetRpiDevices(string rpiid)
        {
            string? apiKey = _userService.GetApiKey(Request);

            if (string.IsNullOrEmpty(apiKey))
                return Unauthorized();

            RPIDevices? data = _deviceService.GetRPIDevices(rpiid, apiKey!);

            if (data == null)
                return BadRequest();

            return Ok(data);
        }

        [HttpPost("status")]
        public ActionResult<ResponseDevices> SetStatus(DeveiceStatus status)
        {
            GardenId garden = new();
            garden.SetGardenIdByRPI(status.RpiId);

            DeveiceStatus response = _deviceService.SetStatus(status);

            _hubContext.Clients.Group(garden.Id).NewDeviceStatus(response);

            return Ok(response);
        }
    }
}
