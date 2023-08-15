
using MainService.DB;

namespace Services.Device
{
    public class DeviceService
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

        public ReponseDevice? SaveDataToDB(SaveDataRequest data, string rpiId, string rpiKey)
        {
            Garden garden = new();
            garden.SetGardenIdByRPI(rpiId, false);

            RPIDevices? devicesData = GetRPIDevices(rpiId, rpiKey);
            if (devicesData == null)
                return null;

            RPIDevice? deviceData = devicesData.Devices.Where(d => d.ID == data.DeviceId).FirstOrDefault();
            if (deviceData == null)
                return null;

            TimeSpan interval = deviceData.DataUpdateInterval;
            if (!_lastEntryList.ContainsKey(data.DeviceId))
                _lastEntryList.Add(data.DeviceId, DateTime.Now);

            if (_lastEntryList[data.DeviceId] < DateTime.Now - interval)
            {
                _lastEntryList[data.DeviceId] = DateTime.Now;
                _cacheList.Remove(data.DeviceId);
            }


            if (_lastEntryList[data.DeviceId] < DateTime.Now - interval)
            {
                string query = QueryService.SaveDataToDatabaseQuery(garden, data);
                MainDB.Query(query);
                _lastEntryList[data.DeviceId] = DateTime.Now;
                _cacheList.Remove(data.DeviceId);
            }

            ReponseDevice response = GetDeviceFromRPI(garden, data);
            if (!_cacheList.ContainsKey(data.DeviceId))
                _cacheList.Add(data.DeviceId, response);
            else
                _cacheList[data.DeviceId] = response;

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

        public ReponseDevice GetDeviceFromRPI(Garden garden, SaveDataRequest data)
        {
            string query = QueryService.OverviewDeviceFromRPI(garden.Id, data.DeviceId);

            List<Dictionary<string, string>> devicelist = MainDB.Query(query);
            Dictionary<string, string> deviceDic = devicelist.FirstOrDefault()!;
            deviceDic["value"] = data.Value.ToString();
            deviceDic["date"] = DateTime.Now.ToString();
            ReponseDevice devices = new(deviceDic);
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