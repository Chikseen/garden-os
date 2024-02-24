using ExtensionMethods;
using Shared.DeviceModels;
using System.Globalization;

public static class IncommingDataQuery
{
    public static string InsertNewDataQuery(DeviceInput data)
    {
        string query = string.Empty;
        foreach (KeyValuePair<string, int> sensor in data.Sensor)
        {
            query += GetQuery(data, sensor);
        }
        return query;
    }

    private static string GetQuery(DeviceInput data, KeyValuePair<string, int> sensor)
    {
        return @$"
			SET TIMEZONE = 'UTC';
			INSERT INTO DATALOG{data.GardenId.Replace("-", "")} (
                ID, 
                UPLOAD_DATE, 
                DEVICE_ID, 
                SENSOR_ID,
                VALUE)
			VALUES (
                GEN_RANDOM_UUID(), 
                Now(), 
                '{data.DeviceId}',
                '{sensor.Key}',
                {sensor.Value.ToString("G", CultureInfo.InvariantCulture)});".Clean();
    }
}
