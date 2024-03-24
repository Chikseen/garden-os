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
					{DeviceStatic.DeviceName},
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

    public static string DeleteManualEntryQuery(string gardenId, string entryId)
    {
        return @$"
			DELETE FROM
				DATALOG{gardenId.Replace("-", "")}
			WHERE		
				upload_date IN (
					SELECT 
						upload_date 
					FROM 
						DATALOG{gardenId.Replace("-", "")}
					WHERE 
						id = '{entryId}')".Clean();
    }

    public static string GetSensorMetaQuery(string deviceId)
    {
        return @$"
			SELECT 
				*
			FROM
				DEVICE_SENSORS
			WHERE 
				DEVICE_ID = '{deviceId}'".Clean();
    }

    public static string CreateNewSensorQuery(SensorCreateModel model)
    {
        return @$"
			INSERT INTO
				device_sensors (
					{DeviceStatic.DeviceId}
					,{DeviceStatic.SensorId}
					,{DeviceStatic.SensorName}
					,{DeviceStatic.SensorTypeId}
					,{DeviceStatic.UpperLimit}
					,{DeviceStatic.LowerLimit}
					,{DeviceStatic.Unit})
			VALUES	(
				'{model.DeviceId}'
				,'{Guid.NewGuid()}'
				,'{model.Name}'
				,{model.SensorTypeId}
				,{model.UpperLimit}
				,{model.LowerLimit}
				,'{model.Unit}')".Clean();
    }

    public static string EditSensorQuery(string sensorId, string propertyName, string propertyValue)
    {
        return @$"
			UPDATE 
				DEVICE_SENSORS
			SET 
				{propertyName} = '{propertyValue}'
			WHERE 
				{DeviceStatic.SensorId} = '{sensorId}'".Clean();
    }

    public static string EditDeviceQuery(string deviceId, string propertyName, string propertyValue)
    {
        return @$"
			UPDATE 
				DEVICEs
			SET 
				{propertyName} = '{propertyValue}'
			WHERE 
				{DeviceStatic.Id} = '{deviceId}'".Clean();
    }
}