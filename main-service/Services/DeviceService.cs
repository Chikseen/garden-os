
using MainService.DB;

namespace Services.Device
{
    public class DeviceService
    {
        private static readonly Dictionary<string, DateTime> lastEntryList = new();
        private static readonly Dictionary<string, Dictionary<string, float>> _deviceCache = new();

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

        public ResponseDevices? SaveDataToDB(SaveDataRequest data, string rpiId, string rpiKey)
        {
            Garden garden = new();
            garden.SetGardenIdByRPI(rpiId);

            string query = QueryService.SaveDataToDatabaseQuery(garden, data);

            RPIDevices? devicesData = GetRPIDevices(rpiId, rpiKey);
            if (devicesData == null)
                return null;

            RPIDevice? deviceData = devicesData.Devices.Where(d => d.ID == data.Device_ID).FirstOrDefault();
            if (deviceData == null)
                return null;

            TimeSpan interval = deviceData.DataUpdateInterval;
            if (!lastEntryList.ContainsKey(data.Device_ID))
                lastEntryList.Add(data.Device_ID, DateTime.Now);

            if (lastEntryList[data.Device_ID] < DateTime.Now - interval)
            {
                MainDB.Query(query);
                lastEntryList[data.Device_ID] = DateTime.Now;
            }

            if (!_deviceCache.ContainsKey(garden.Id))
                _deviceCache.Add(garden.Id, new());
            if (!_deviceCache[garden.Id].ContainsKey(data.Device_ID))
                _deviceCache[garden.Id].Add(data.Device_ID, 0.0f);
            _deviceCache[garden.Id][data.Device_ID] = data.Value;

            ResponseDevices response = GetOverviewFromRPI(rpiId, rpiKey, garden.Id);

            foreach (ReponseDevice device in response.Devices)
            {
                if (_deviceCache[garden.Id].ContainsKey(device.DeviceID))
                {
                    int i = response.Devices.FindIndex(d => d.DeviceID == device.DeviceID);
                    response.Devices[i].SetNewValue(_deviceCache[garden.Id][device.DeviceID]);
                }
            }

            return response;
        }

        public ResponseDevices GetOverview(UserData userData, string gardenId)
        {
            userData.CheckGardenAccess(gardenId);

            string query = QueryService.OverviewQuery(gardenId);

            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices GetOverviewFromRPI(string rpiId, string rpiKey, string gardenId)
        {
            string query = QueryService.OverviewFromRPIQuery(rpiId, rpiKey, gardenId);

            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices GetDetailed(UserData userData, string gardenId, TimeFrame timeFrame)
        {
            userData.CheckGardenAccess(gardenId);

            string query = QueryService.DetailedViewQuery(gardenId, timeFrame);
            List<Dictionary<string, string>> result = MainDB.Query(query);
            ResponseDevices devices = new(result);
            return devices;
        }

        public DeveiceStatus SetStatus(DeveiceStatus status)
        {
            string insertQuery = QueryService.SetStatusQuery(status);
            MainDB.Query(insertQuery);

            Garden garden = new();
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
    }
}