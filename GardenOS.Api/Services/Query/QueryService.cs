using ExtensionMethods;
using Shared.DeviceModels;
using Shared.Models;

public static class QueryService
{
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