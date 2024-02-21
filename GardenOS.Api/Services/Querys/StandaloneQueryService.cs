using ESP_sensor.Models;
using ExtensionMethods;
using System.Globalization;

public static class StandaloneQueryService
{
	public static string CheckCredentialsQuery(StandaloneDevice data)
	{
		return @$"
			SELECT 
				*
			FROM
				STANDALONEDEVICES
			WHERE 
				ID = '{data.DeviceId}'
				AND NAME = '{data.Name}'
				AND GARDEN_ID = '{data.GardenId}'
				AND API_KEY = '{data.ApiKey}'".Clean();
	}

    public static string InsertNewDataQuery(StandaloneDevice data)
    {
        return @$"
			SET TIMEZONE = 'Europe/Berlin';
			INSERT INTO DATALOG{data.GardenId.Replace("-", "")} (ID, VALUE, UPLOAD_DATE, DEVICE_ID) 
			VALUES (GEN_RANDOM_UUID(), {data.Value.ToString("G", CultureInfo.InvariantCulture)}, Now(), '{data.DeviceId}')".Clean();
    }
}

