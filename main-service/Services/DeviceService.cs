
using MainService.DB;
using ExtensionMethods;
using System.Globalization;
using System.Linq;

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
            String gardenID = GetGardenID(id, false);

            String query = @$"
                SET TIMEZONE = 'Europe/Berlin';
                INSERT INTO DATALOG{gardenID} (ID, VALUE, UPLOAD_DATE, DEVICE_ID) 
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
            String gardenID = GetGardenID(id, isUser);

            String query = BuildDataLogQuery(id, ApiKey, gardenID, null, isUser);
            List<Dictionary<String, String>> result = MainDB.query(query);

            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices? GetDataLog(String id, String ApiKey, TimeFrame timeframe, Boolean isUser = false)
        {
            String gardenID = GetGardenID(id, isUser);
            if (String.IsNullOrEmpty(gardenID))
                return null;

            String query = BuildDataLogQuery(id, ApiKey, gardenID, timeframe, isUser);
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count == 0)
                return null;

            ResponseDevices devices = new(result);

            if (DateTime.UtcNow < timeframe.End)
                devices.Devices.AddRange(GetDataLog(id, ApiKey, true).Devices);

            devices.Devices = devices.Devices.OrderBy(d => d.date).ToList(); // sort each group
            return devices;
        }

        private String BuildDataLogQuery(String id, String apiKey, String gardenID, TimeFrame? timeFrame = null, Boolean isUser = false)
        {
            String query = "SELECT ";

            if (timeFrame == null)
                query += "DISTINCT ON (DEVICE_ID) ";

            if (timeFrame == null)
                query += " DATALOG.UPLOAD_DATE AS UPLOAD_DATE, ";
            else
                query += " DATALOG.DATE AS UPLOAD_DATE, ";

            query += @"
                    DATALOG.device_id AS DEVICE_ID,
                    DATALOG.VALUE AS VALUE,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID,
                    DEVICES.UPPER_LIMIT,
                    DEVICES.LOWER_LIMIT,
                    DEVICES.ISINVERTED
                FROM ";

            if (timeFrame == null)
                query += @$"DATALOG{gardenID} AS DATALOG";
            else
                query += @$"	
                    (
                        SELECT
                            date_trunc('hour', UPLOAD_DATE) AS DATE,
                            AVG (value) AS VALUE,
                            DEVICE_ID
                        FROM
                           DATALOG{gardenID}
                        GROUP BY
                            DATE,
                            DEVICE_ID
                    ) AS DATALOG";

            if (isUser)
                // JoinUSer
                query += @$"
                    JOIN DEVICES
                        ON DEVICES.ID = DEVICE_ID JOIN USERS
                        ON USERS.ID = '{id}'
                        AND USERS.API_KEY = '{apiKey}'
                        AND USERS.GARDEN_ID = DEVICES.GARDEN_ID";

            else
                query += @$"
                    JOIN DEVICES
                        ON DEVICES.ID = DEVICE_ID JOIN RPIS
                        ON RPIS.ID = '{id}'
                        AND RPIS.API_KEY = '{apiKey}'
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
                        UPLOAD_DATE DESC; ";
            else
                query += @$"
                    ORDER BY UPLOAD_DATE DESC; ";

            String cleandQuery = query.Clean();
            return cleandQuery;
        }

        public String GetGardenID(String id, Boolean isUser)
        {
            String query = String.Empty;
            if (isUser)
                query = $"SELECT GARDEN_ID FROM USERS WHERE ID = '{id}'";
            else
                query = $"SELECT GARDEN_ID FROM RPIS WHERE ID = '{id}'";

            List<Dictionary<String, String>> result = MainDB.query(query);
            Dictionary<String, String>? entry = result.FirstOrDefault();
            if (entry is not null)
                return entry["garden_id"].Replace("-", "");
            else return String.Empty;
        }
    }
}