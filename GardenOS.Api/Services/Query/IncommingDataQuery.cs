using ExtensionMethods;
using Shared;
using Shared.DeviceModels;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class IncommingDataQuery
{
    public static string InsertNewDataQuery(DeviceInput data)
    {
        string query = string.Empty;
        foreach (KeyValuePair<string, uint> sensor in data.Sensor)
        {
            query += GetQuery(data, sensor);
        }
        return query;
    }

    public static string UploadNewValueQuery(NewManualValueModel model)
    {
        return @$"
			SET TIMEZONE = 'UTC';
			INSERT INTO DATALOG{model.GardenId.Replace("-", "")} (
                ID, 
                UPLOAD_DATE, 
                DEVICE_ID, 
                SENSOR_ID,
                VALUE)
			VALUES (
                GEN_RANDOM_UUID(), 
                Now(), 
                '{model.DeviceId}',
                '{model.SensorId}',
                {model.Value.ToString("G", CultureInfo.InvariantCulture)});".Clean();
    }

    private static string GetQuery(DeviceInput data, KeyValuePair<string, uint> sensor)
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
