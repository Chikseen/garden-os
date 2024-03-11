using System.Globalization;
using ESP_sensor.Models;
using ExtensionMethods;
using Shared;
using Shared.DeviceModels;
using Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class QueryService
{
    public static string RPIMetaQuery(string rpiId, string apiKey)
    {
        return @$"
			SELECT
				RPIS.ID   AS RPI_ID,
				GARDEN.ID AS GARDEN_ID,
				GARDEN.NAME
			FROM
				RPIS
				JOIN GARDEN
				ON RPIS.ID = '{rpiId}'
				AND RPIS.API_KEY = '{apiKey}'
				AND RPIS.GARDEN_ID = GARDEN.ID;".Clean();
    }

    public static string RPIDeviceQuery(string rpiId, string apiKey)
    {
        return @$"
			SELECT
				DEVICES.ID AS DEVICE_ID,
				DEVICES.NAME AS DEVICE_NAME,
				DEVICES.DEVICE_TYP,
				DEVICES.DISPLAY_ID,
				DEVICES.SORT_ORDER, 
				DEVICES.GROUP_ID
			FROM
				RPIS
				JOIN GARDEN
				ON RPIS.ID = '{rpiId}'
				AND RPIS.API_KEY = '{apiKey}'
				AND RPIS.GARDEN_ID = GARDEN.ID JOIN DEVICES
				ON RPIS.GARDEN_ID = DEVICES.GARDEN_ID;".Clean();
    }

    public static string GetControllerOverviewQuery(string gardenId)
    {
        return @$"
			SELECT
				controlls.id,
				controlls.name,
				controlls.garden_id
			FROM
				controlls
			WHERE
				controlls.garden_id = '{gardenId}'".Clean();
    }

    private static string OverviewSelectDistinct()
    {
        return @$"
			SELECT
				DISTINCT ON (DEVICE_ID)
				DATALOG.UPLOAD_DATE AS UPLOAD_DATE,
				DATALOG.device_id AS DEVICE_ID,
				DATALOG.VALUE AS VALUE,
				DEVICES.NAME,
				DEVICES.DISPLAY_ID,
				DEVICES.SORT_ORDER, 
				DEVICES.GROUP_ID";
    }

    public static string CheckCredentialsQuery(StandaloneDevice data)
    {
        return @$"
			SELECT 
				*
			FROM
				DEVICES
			WHERE 
				ID = '{data.DeviceId}'
				AND GARDEN_ID = '{data.GardenId}'
				AND API_KEY = '{data.ApiKey}'".Clean();
    }

    public static string CreateNewDevice(DeviceCreateModel model)
    {
        return @$"
			INSERT INTO 
				devices(
					{DeviceStatic.Id},
					{DeviceStatic.Name},
					{DeviceStatic.GardenID},
					{DeviceStatic.IsManual})
			VALUES (
					'{Guid.NewGuid()}',
					'{model.Name}',
					'{model.GardenId}',
					{model.IsManual});
			".Clean();
    }

    public static string GetAllDevices(string gardenId)
    {
        return @$"
			SELECT 
				*
			FROM
				DEVICES
			WHERE 
				GARDEN_ID = '{gardenId}'".Clean();
    }

    public static string GetDevice(string deviceId)
    {
        return @$"
			SELECT 
				*
			FROM
				device_sensors
			WHERE 
				device_id = '{deviceId}'".Clean();
    }
}