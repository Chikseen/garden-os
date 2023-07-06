
using MainService.DB;
using ExtensionMethods;
using System.Globalization;

namespace Services.Device
{
    public class DeviceService
    {
        private static Dictionary<String, DateTime> lastEntryList = new();
        private static Dictionary<String, ResponseDevices> _deviceCache = new();

        public RPIData? GetRpiMeta(String id, String ApiKey)
        {
            String query = @$"
                SELECT
                    RPIS.ID   AS RPI_ID,
                    GARDEN.ID AS GARDEN_ID,
                    GARDEN.NAME
                FROM
                    RPIS
                    JOIN GARDEN
                    ON RPIS.ID = '{id}'
                    AND RPIS.API_KEY = '{ApiKey}'
                    AND RPIS.GARDEN_ID = GARDEN.ID;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count != 1)
                return null;

            RPIData rpi = new(result);
            return rpi;
        }

        public RPIDevices? GetRPIDevices(String id, String ApiKey)
        {
            String query = @$"
                SELECT
                    DEVICES.ID AS DEVICE_ID,
                    DEVICES.NAME AS DEVICE_NAME,
                    DEVICES.LOWER_LIMIT,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.DEVICE_TYP,
                    DEVICES.ADDRESS,
                    DEVICES.SERIAL_ID,
                    DEVICES.DISPLAY_ID,
                    DEVICES.DATA_UPDATE_INTERVAL
                FROM
                    RPIS
                    JOIN GARDEN
                    ON RPIS.ID = '{id}'
                    AND RPIS.API_KEY = '{ApiKey}'
                    AND RPIS.GARDEN_ID = GARDEN.ID JOIN DEVICES
                    ON RPIS.GARDEN_ID = DEVICES.GARDEN_ID;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count == 0)
                return null;

            RPIDevices devices = new(result);
            return devices;
        }

        public ResponseDevices? SaveDataToDB(SaveDataRequest data, String rpiId, String rpiKey)
        {
            Garden garden = new Garden();
            garden.SetGardenIdByRPI(rpiId);

            String query = @$"
                SET TIMEZONE = 'Europe/Berlin';
                INSERT INTO DATALOG{garden.Id} (ID, VALUE, UPLOAD_DATE, DEVICE_ID) 
                VALUES (GEN_RANDOM_UUID(), {data.Value.ToString("G", CultureInfo.InvariantCulture)}, Now(), '{data.Device_ID}')".Clean();

            RPIDevices? devicesData = GetRPIDevices(rpiId, rpiKey);
            if (devicesData == null)
                return null;

            RPIDevice? deviceData = devicesData.Devices.Where(d => d.ID == data.Device_ID).FirstOrDefault();
            if (deviceData == null)
                return null;

            TimeSpan interval = deviceData.DataUpdateInterval;
            if (!lastEntryList.ContainsKey(data.Device_ID))
                lastEntryList.Add(data.Device_ID, DateTime.Now);

            if (!_deviceCache.ContainsKey(garden.Id))
                _deviceCache.Add(garden.Id, _deviceCache[garden.Id] = GetOverviewFromRPI(rpiId, rpiKey, garden.Id));

            if (lastEntryList[data.Device_ID] < DateTime.Now - interval)
            {
                MainDB.query(query);
                lastEntryList[data.Device_ID] = DateTime.Now;
                _deviceCache[garden.Id] = GetOverviewFromRPI(rpiId, rpiKey, garden.Id);
            }
            else
            {
                _deviceCache[garden.Id].Devices.First(d => d.DeviceID == data.Device_ID).setNewValue(data.Value);
            }
            return _deviceCache[garden.Id];
        }

        public ResponseDevices GetOverview(UserData userData, String gardenId)
        {
            userData.CheckGardenAccess(gardenId);

            String query = @$"
                SELECT
                    DISTINCT ON (DEVICE_ID)
                    DATALOG.UPLOAD_DATE AS UPLOAD_DATE,
                    DATALOG.device_id AS DEVICE_ID,
                    DATALOG.VALUE AS VALUE,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.LOWER_LIMIT,
                    DEVICES.ISINVERTED
                FROM
                    DATALOG{gardenId.Replace("-", "")} AS DATALOG
                    JOIN devices ON devices.id = DATALOG.device_id
                    AND devices.garden_id = '{gardenId}'
                ORDER BY
                    DEVICE_ID,
                    UPLOAD_DATE DESC;";
            String cleandQuery = query.Clean();

            List<Dictionary<String, String>> result = MainDB.query(cleandQuery);
            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices GetOverviewFromRPI(String rpiId, String rpiKey, String gardenId)
        {
            String query = @$"
                SELECT
                    DISTINCT ON (DEVICE_ID)
                    DATALOG.UPLOAD_DATE AS UPLOAD_DATE,
                    DATALOG.device_id AS DEVICE_ID,
                    DATALOG.VALUE AS VALUE,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.LOWER_LIMIT,
                    DEVICES.ISINVERTED
                FROM
                    DATALOG{gardenId.Replace("-", "")} AS DATALOG
                    JOIN DEVICES
                    ON DEVICES.ID = DEVICE_ID 
                    JOIN RPIS
                    ON RPIS.ID = '{rpiId}'
                    AND RPIS.API_KEY = '{rpiKey}'
                    AND RPIS.GARDEN_ID = DEVICES.GARDEN_ID
                ORDER BY
                    DEVICE_ID,
                    UPLOAD_DATE DESC;";
            String cleandQuery = query.Clean();

            List<Dictionary<String, String>> result = MainDB.query(cleandQuery);
            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices GetDetailed(UserData userData, String gardenId, TimeFrame timeFrame)
        {
            userData.CheckGardenAccess(gardenId);

            String query = @$"
                SELECT
                    DATALOG.DATE AS UPLOAD_DATE,
                    DATALOG.device_id AS DEVICE_ID,
                    DATALOG.VALUE AS VALUE,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.LOWER_LIMIT,
                    DEVICES.ISINVERTED
                FROM
                    (
                        SELECT
                            date_trunc('hour', UPLOAD_DATE) AS DATE,
                            AVG (value) AS VALUE,
                            DEVICE_ID
                        FROM
                            DATALOG{gardenId.Replace("-", "")}
                        GROUP BY
                            DATE,
                            DEVICE_ID
                    ) AS DATALOG
                    JOIN DEVICES ON DEVICES.ID = DEVICE_ID
                    AND DATALOG.DATE BETWEEN '{timeFrame.Start.ConvertToPGString()}'
                    AND '{timeFrame.End.ConvertToPGString()}'
                ORDER BY
                    UPLOAD_DATE DESC;";
            String cleandQuery = query.Clean();

            List<Dictionary<String, String>> result = MainDB.query(cleandQuery);
            ResponseDevices devices = new(result);
            return devices;
        }
    }
}