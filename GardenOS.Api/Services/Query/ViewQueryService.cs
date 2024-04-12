using ExtensionMethods;
using Shared;
using Shared.DeviceModels;
using Shared.Models;

public static class ViewQueryService
{
    public static string DetailedViewQuery(string gardenId, string deviceid, TimeFrame timeFrame)
    {
        return @$"
			SELECT
				DISTINCT ON (
					UPLOAD_DATE
					,DEVICE_ID
					,DATALOG.SENSOR_ID)
				DATALOG.DATE AS UPLOAD_DATE 
				,DATALOG.{DeviceStatic.DeviceId} AS DEVICE_ID
				,DATALOG.{DeviceStatic.Value} AS VALUE
				,DATALOG.{DeviceStatic.Id}
				,DEVICES.{DeviceStatic.DisplayId}
				,DEVICES.{DeviceStatic.SortOrder}
				,DEVICES.{DeviceStatic.GroupId}
				,DEVICES.{DeviceStatic.DeviceName}
				,DEVICES.{DeviceStatic.DeviceTypId}
				,DEVICE_SENSORS.{DeviceStatic.SensorName}
				,DEVICE_SENSORS.{DeviceStatic.UpperLimit}
				,DEVICE_SENSORS.{DeviceStatic.LowerLimit}
				,DEVICE_SENSORS.{DeviceStatic.SensorId}
				,DEVICE_SENSORS.{DeviceStatic.Unit}
				,DEVICE_SENSORS.{DeviceStatic.IsInverted}
				,DEVICE_SENSORS.{DeviceStatic.SensorTypeId}
			FROM
				(
					SELECT
						UPLOAD_DATE AS DATE
						,ID
						,AVG (value) AS VALUE
						,DEVICE_ID, SENSOR_ID
					FROM
						DATALOG{gardenId.Replace("-", "")}
					WHERE 
						UPLOAD_DATE 
							BETWEEN 
								'{timeFrame.Start.ConvertToPGString()}' 
							AND 
								'{timeFrame.End.ConvertToPGString()}'
					GROUP BY
						ID
						,DATE
						,DEVICE_ID
						,SENSOR_ID
						,VALUE
					ORDER BY
						SENSOR_ID, DATE DESC
				) AS DATALOG
			JOIN 
				DEVICES
			ON 
				DEVICES.GARDEN_ID = '{gardenId}'
			AND 
				DEVICES.ID = '{deviceid}'
			JOIN 
				DEVICE_SENSORS
			ON 
				DATALOG.SENSOR_ID = DEVICE_SENSORS.SENSOR_ID
			AND
				DATALOG.DEVICE_ID = '{deviceid}'
			ORDER BY 
				UPLOAD_DATE DESC ".Clean();
    }

    public static string GetSensorValuessQuery(string gardenId, string deviceId)
    {
        return @$"
			SELECT
				DISTINCT ON(DATALOG.SENSOR_ID)
				DATALOG.{DeviceStatic.Id}
				,DATALOG.{DeviceStatic.Value}
				,DATALOG.{DeviceStatic.UploadDate}
				,DATALOG.{DeviceStatic.DeviceId}
				,DATALOG.{DeviceStatic.SensorId}
				,DEVICES.{DeviceStatic.IsManual}
				,DEVICES.{DeviceStatic.DeviceName}
				,DEVICES.{DeviceStatic.DeviceTypId}
				,DEVICE_SENSORS.{DeviceStatic.UpperLimit}
				,DEVICE_SENSORS.{DeviceStatic.LowerLimit}
				,DEVICE_SENSORS.{DeviceStatic.Unit}
				,DEVICE_SENSORS.{DeviceStatic.SensorName}
				,DEVICE_SENSORS.{DeviceStatic.IsInverted}
				,DEVICE_SENSORS.{DeviceStatic.SensorTypeId}
			FROM
				DATALOG{gardenId.Replace("-", "")} AS DATALOG
			JOIN 
				DEVICE_SENSORS
			ON
				DATALOG.SENSOR_ID = DEVICE_SENSORS.SENSOR_ID
			JOIN
				DEVICES
			ON
				DEVICES.ID = DEVICE_SENSORS.DEVICE_ID
			AND 
				DATALOG.{DeviceStatic.DeviceId} = '{deviceId}'
			ORDER BY 
				DATALOG.SENSOR_ID, 
				UPLOAD_DATE DESC".Clean();
    }
}

