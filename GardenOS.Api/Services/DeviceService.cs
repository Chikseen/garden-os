using API.Interfaces;
using ESP_sensor.Models;
using MainService.DB;
using MainService.Hub;
using Microsoft.AspNetCore.SignalR;
using Shared.DeviceModels;
using Shared.Models;

namespace API.Services
{
    public class DeviceService(IHubContext<MainHub, IMainHub> _hubContext) : IDeviceService
    {
        private static readonly Dictionary<string, DateTime> _lastEntryList = new();
        private static readonly Dictionary<string, ReponseDevice> _cacheList = new();

        public RPIData? GetRpiMeta(string rpiId, string apiKey)
        {
            string query = QueryService.RPIMetaQuery(rpiId, apiKey);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            if (result.Count != 1)
                return null;

            RPIData rpi = new(result);
            return rpi;
        }

        public RPIDevices? GetRPIDevices(string rpiId, string apiKey)
        {
            string query = QueryService.RPIDeviceQuery(rpiId, apiKey);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            if (result.Count == 0)
                return null;

            RPIDevices devices = new(result);
            return devices;
        }

        public ResponseDevices GetOverview(UserData userData, string gardenId)
        {
            userData.CheckGardenAccess(gardenId);

            string query = ViewQueryService.DetailedViewQuery(gardenId);

            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);

            for (int i = 0; i < devices.Devices.Count; i++)
            {
                ReponseDevice device = devices.Devices[i];
                if (_cacheList.ContainsKey(device.DeviceID))
                    devices.Devices[i] = _cacheList[device.DeviceID];
            }

            return devices;
        }

        public ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame)
        {
            string query = ViewQueryService.DetailedViewQuery(gardenId, timeFrame);
            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);
            return devices;
        }

        public DeveiceStatus SetStatus(DeveiceStatus status)
        {
            string insertQuery = QueryService.SetStatusQuery(status);
            MainDB.Query(insertQuery);

            GardenId garden = new();
            garden.SetGardenIdByRPI(status.RpiId, false);

            return GetStatus(garden.Id);
        }

        public DeveiceStatus GetStatus(string gardenId)
        {
            string query = QueryService.GetStatusQuery(gardenId);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            return new(result.FirstOrDefault()!);
        }

        public List<DeveiceStatus> GetStatusLog(string gardenId)
        {
            string query = QueryService.GetStatusLogQuery(gardenId);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<DeveiceStatus> list = new();
            foreach (Dictionary<string, string> item in result)
            {
                list.Add(new(item));
            }

            return list;
        }

        public void PatchDevice(PatchDeviceRequest device)
        {
            string query = QueryService.GetPatchDeviceQuery(device);
            MainDB.Query(query);
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
    }
}