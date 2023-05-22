
using MainService.DB;
using ExtensionMethods;

namespace Services.Device
{
    public class DeviceService
    {
        private static Dictionary<String, DateTime> lastEntryList = new();

        public RPIdata? GetRpiMeta(String id, String ApiKey)
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

            RPIdata rpi = new(result);
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
                INSERT INTO DATALOG (ID, VALUE, DATE, DEVICE_ID) 
                VALUES (GEN_RANDOM_UUID(), {data.Value}, LOCALTIMESTAMP, '{data.Device_ID}')".Clean();

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

        public ResponseDevices? GetDataLog(String id, String ApiKey)
        {
            String query = @$"  
                SELECT
                    DISTINCT ON (DEVICE_ID) DATALOG.DATE,
                    VALUE,
                    DEVICES.ID AS DEVICE_ID,
                    DATALOG.ID,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID
                FROM
                    DATALOG
                    JOIN DEVICES
                    ON DEVICES.ID = DATALOG.DEVICE_ID JOIN RPIS
                    ON RPIS.ID = '{id}'
                    AND RPIS.API_KEY = '{ApiKey}'
                    AND RPIS.GARDEN_ID = DEVICES.GARDEN_ID
                ORDER BY
                    DEVICE_ID,
                    DATE DESC;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count == 0)
                return null;

            ResponseDevices devices = new(result);
            return devices;
        }

        public ResponseDevices? GetDataLog(String id, String ApiKey, TimeFrame timeframe)
        {
            String query = @$"  
                SELECT
                    DEVICES.ID AS DEVICE_ID,
                    DATALOG.ID,
                    DATALOG.VALUE,
                    DATALOG.DATE,
                    DEVICES.NAME,
                    DEVICES.DISPLAY_ID
                FROM
                    DATALOG
                    JOIN DEVICES
                    ON DEVICES.ID = DATALOG.DEVICE_ID JOIN RPIS
                    ON RPIS.ID = '{id}'
                    AND RPIS.API_KEY = '{ApiKey}'
                    AND RPIS.GARDEN_ID = DEVICES.GARDEN_ID
                    AND DATALOG.DATE 
                        BETWEEN '{timeframe.Start.ConvertToPGString()}'
                        AND '{timeframe.End.ConvertToPGString()}'
                ORDER BY
                    DATALOG.DATE DESC".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count == 0)
                return null;

            ResponseDevices devices = new(result);
            return devices;
        }
    }
}