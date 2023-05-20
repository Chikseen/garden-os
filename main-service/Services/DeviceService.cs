
using MainService.DB;
using ExtensionMethods;

namespace Services.Device
{
    public class DeviceService
    {
        public RPIdata GetRPI(String id)
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
                    AND RPIS.GARDEN_ID = GARDEN.ID;".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            RPIdata rpi = new(result);
            return rpi;
        }
    }
}