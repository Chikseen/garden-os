using API.Interfaces;
using ESP_sensor.Models;
using MainService.DB;
using Shared;
using Shared.Models;
using System.Reflection;

namespace Services.Device
{
    public class DeviceService : IDeviceService
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

        public ReponseDevice? SaveDataToDB(SaveDataRequest model, string rpiId, string rpiKey)
        {
            GardenId garden = new();
            garden.SetGardenIdByRPI(rpiId, false);

            RPIDevices? devicesData = GetRPIDevices(rpiId, rpiKey);
            if (devicesData == null)
                return null;

            RPIDevice? deviceData = devicesData.Devices.Where(d => d.ID == model.DeviceId).FirstOrDefault();
            if (deviceData == null)
                return null;

            TimeSpan interval = deviceData.DataUpdateInterval;
            if (!_lastEntryList.ContainsKey(model.DeviceId))
                _lastEntryList.Add(model.DeviceId, DateTime.Now);

            if (_lastEntryList[model.DeviceId] < DateTime.Now - interval)
            {
                _lastEntryList[model.DeviceId] = DateTime.Now;
                _cacheList.Remove(model.DeviceId);
            }


            if (_lastEntryList[model.DeviceId] < DateTime.Now - interval)
            {
                _lastEntryList[model.DeviceId] = DateTime.Now;
                _cacheList.Remove(model.DeviceId);
            }
            string query = QueryService.SaveDataToDatabaseQuery(garden, model);
            MainDB.Query(query);

            ReponseDevice response = GetDevice(garden, model.DeviceId, model.Value);
            if (!_cacheList.ContainsKey(model.DeviceId))
                _cacheList.Add(model.DeviceId, response);
            else
                _cacheList[model.DeviceId] = response;

            return response;
        }

        public ResponseDevices GetOverview(UserData userData, string gardenId)
        {
            userData.CheckGardenAccess(gardenId);

            string query = QueryService.OverviewQuery(gardenId);

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

        public ReponseDevice GetDevice(GardenId garden, string deviceId, double value)
        {
            string query = QueryService.OverviewDeviceQuery(garden.Id, deviceId);

            List<Dictionary<string, string>> devicelist = MainDB.Query(query);
            Dictionary<string, string> deviceDic = devicelist.FirstOrDefault()!;
            deviceDic["value"] = value.ToString();
            deviceDic["date"] = DateTime.Now.ToString();
            ReponseDevice devices = new(deviceDic);
            return devices;
        }

        public ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame)
        {
            string query = QueryService.DetailedViewQuery(gardenId, timeFrame);
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

        public ReponseDevice StoreData(DeviceInput model)
        {
            string query = QueryService.InsertNewDataQuery(model);
            MainDB.Query(query);

            GardenId gardenId = new(model.GardenId);
            ReponseDevice response = GetDevice(gardenId, model.DeviceId, model.Value);

            return response;
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