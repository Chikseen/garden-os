
using MainService.DB;
using ExtensionMethods;

namespace Services.Device
{
    public class DeviceService
    {
        public RPIdata? GetRPI(String id, String ApiKey)
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
    }
}