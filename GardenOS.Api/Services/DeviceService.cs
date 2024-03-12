using API.DB;
using API.Hub;
using API.Interfaces;
using ESP_sensor.Models;
using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.DeviceModels;
using Shared.Models;

namespace API.Services
{
    public class DeviceService(IHubContext<MainHub, IMainHub> _hubContext) : IDeviceService
    {
        public ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame)
        {
            string query = ViewQueryService.DetailedViewQuery(gardenId, timeFrame);
            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);
            return devices;
        }

        public GardenInfo GetGardenInfo(string gardenId)
        {
            TimeFrame timeframe = new();
            ResponseDevices detailed = GetDetailed(gardenId, timeframe);
            GardenInfo info = new(detailed);
            return info;
        }

        public void StoreData(DeviceInput model)
        {
            string query = IncommingDataQuery.InsertNewDataQuery(model);
            MainDB.Query(query);

            GardenId gardenId = new(model.GardenId);
            _hubContext.Clients.Group(gardenId.Id).SendCurrentDeviceData(model);
        }

        public bool IsCredentialsValid(StandaloneDevice data)
        {
            string query = QueryService.CheckCredentialsQuery(data);
            List<Dictionary<string, string>> response = MainDB.Query(query);

            if (response.Count != 1)
                return false;
            return true;
        }

        public void CreateNewDevice(DeviceCreateModel model)
        {
            string query = QueryService.CreateNewDevice(model);
            MainDB.Query(query);
        }

        public List<DeviceMeta> GetAllDevices(string gardenId)
        {
            string query = QueryService.GetAllDevices(gardenId);
            MainDB.Query(query);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<DeviceMeta> response = new();
            foreach (var device in result)
            {
                response.Add(new(device));
            }
            return response;
        }

        public List<DeviceSensorMeta> GetDevice(string deviceId)
        {
            string query = QueryService.GetDevice(deviceId);
            MainDB.Query(query);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<DeviceSensorMeta> response = new();
            foreach (var device in result)
            {
                response.Add(new(device));
            }
            return response;
        }

        public ReponseDevice GetLastSensorValue(string gardenId, string deviceId, string sensorId)
        {
            string query = ViewQueryService.GetLastSensorValueQuery(gardenId, deviceId, sensorId);

            List<Dictionary<string, string>> result = MainDB.Query(query);
            if (result.Count > 0)
            {
                ReponseDevice devices = new(result.FirstOrDefault());
                return devices;
            }
            else
                return new();

        }

        public ReponseDevice UploadNewValue(NewManualValueModel model)
        {
            string query = IncommingDataQuery.UploadNewValueQuery(model);
            MainDB.Query(query);
            return GetLastSensorValue(model.GardenId, model.DeviceId, model.SensorId);

        }
    }
}