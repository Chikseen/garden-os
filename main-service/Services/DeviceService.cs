
using MainService.DB;
using ExtensionMethods;
using System.Globalization;

namespace Services.Device
{
    public class DeviceService
    {
        private static Dictionary<String, DateTime> lastEntryList = new();

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

        public ResponseDevices? SaveDataToDB(SaveDataRequest data, String id, String ApiKey)
        {
            String query = @$"
                SET TIMEZONE = 'Europe/Berlin';
                INSERT INTO DATALOG (ID, VALUE, DATE, DEVICE_ID) 
                VALUES (GEN_RANDOM_UUID(), {data.Value.ToString("G", CultureInfo.InvariantCulture)}, Now(), '{data.Device_ID}')".Clean();

            RPIDevices? devicesData = GetRPIDevices(id, ApiKey);
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
                MainDB.query(query);
                lastEntryList[data.Device_ID] = DateTime.Now;
            }

            ResponseDevices? devices = GetDataLog(id, ApiKey);
            return devices;
        }

        public ResponseDevices GetDataLog(String id, String ApiKey, Boolean isUser = false)
        {
            String query = BuildDataLogQuery(id, ApiKey, null, isUser);
            List<Dictionary<String, String>> result = MainDB.query(query);

            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices? GetDataLog(String id, String ApiKey, TimeFrame timeframe, Boolean isUser = false)
        {

            String query = BuildDataLogQuery(id, ApiKey, timeframe, isUser);
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count == 0)
                return null;

            ResponseDevices devices = new(result);
            return devices;
        }

        private String BuildDataLogQuery(String ID, String ApiKey, TimeFrame? timeFrame = null, Boolean isUser = false)
        {

            String query = "SELECT ";

            if (timeFrame == null)
                query += "DISTINCT ON (DEVICE_ID) ";

            query += @$"
                    DATALOG.DATE,
                    VALUE,
                    DEVICES.ID AS DEVICE_ID,
                    DATALOG.ID,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.LOWER_LIMIT
                FROM
                    DATALOG";

            if (isUser)
                // JoinUSer
                query += @$"
                    JOIN DEVICES
                        ON DEVICES.ID = DATALOG.DEVICE_ID JOIN USERS
                        ON USERS.ID = '{ID}'
                        AND USERS.API_KEY = '{ApiKey}'
                        AND USERS.GARDEN_ID = DEVICES.GARDEN_ID";

            else
                query += @$"
                    JOIN DEVICES
                        ON DEVICES.ID = DATALOG.DEVICE_ID JOIN RPIS
                        ON RPIS.ID = '{ID}'
                        AND RPIS.API_KEY = '{ApiKey}'
                        AND RPIS.GARDEN_ID = DEVICES.GARDEN_ID";

            if (timeFrame != null)
                query += @$"
                      AND DATALOG.DATE 
                        BETWEEN '{timeFrame.Start.ConvertToPGString()}'
                        AND '{timeFrame.End.ConvertToPGString()}'";

            if (timeFrame == null)
                query += @$"
                    ORDER BY
                        DEVICE_ID,
                        DATE DESC; ";
            else
                query += @$"
                    ORDER BY DATE DESC; ";

            String cleandQuery = query.Clean();
            return cleandQuery;
        }
    }
}