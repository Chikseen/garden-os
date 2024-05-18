using ExtensionMethods;
using Shared.Models;

public static class ViewQueryService
{
    public static string DetailedViewQuery(string gardenId, string deviceid, TimeFrame timeFrame)
    {
        return @$"
			{CreateTableIfNotExists(gardenId)}

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
			{CreateTableIfNotExists(gardenId)}

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
				UPLOAD_DATE DESC
        ".Clean();
    }

    private static string CreateTableIfNotExists(string gardenId)
    {
        return @$"
			CREATE TABLE IF NOT EXISTS public.datalog{gardenId.Replace("-", "")}
			(
				id character varying(36) COLLATE pg_catalog.""default"" NOT NULL,
				value real NOT NULL,
				upload_date timestamp without time zone NOT NULL,
				device_id character varying(36) COLLATE pg_catalog.""default"" NOT NULL,
				sensor_id character varying(36) COLLATE pg_catalog.""default"" NOT NULL,
				CONSTRAINT datalog{gardenId.Replace("-", "")}_pkey PRIMARY KEY (id),
				CONSTRAINT device_id FOREIGN KEY (device_id)
					REFERENCES public.devices(id) MATCH SIMPLE

						ON UPDATE CASCADE
						ON DELETE CASCADE

						NOT VALID,
					CONSTRAINT sensor_id FOREIGN KEY(sensor_id)

						REFERENCES public.device_sensors(sensor_id) MATCH SIMPLE

						ON UPDATE CASCADE
						ON DELETE CASCADE

						NOT VALID
			);".Clean();
    }
}

