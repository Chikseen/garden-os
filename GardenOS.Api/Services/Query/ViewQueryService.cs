using ExtensionMethods;
using Shared;
using Shared.DeviceModels;
using Shared.Models;

public static class ViewQueryService
{
    public static string DetailedViewQuery(string gardenId, TimeFrame? timeFrame = null)
    {
        return @$"
			SELECT
				{GetDistinct(timeFrame)}
				{GetSelect(timeFrame)}
				DATALOG.device_id AS DEVICE_ID
				,DATALOG.VALUE AS VALUE
				,DEVICES.DISPLAY_ID
				,DEVICES.SORT_ORDER
				,DEVICES.GROUP_ID
				,DEVICE_SENSORS.NAME
				,DEVICE_SENSORS.UPPER_LIMIT
				,DEVICE_SENSORS.LOWER_LIMIT
				,DEVICE_SENSORS.SENSOR_ID
				,DEVICE_SENSORS.UNIT
				,DEVICE_SENSORS.IS_INVERTED

			FROM
				(
					SELECT
						{GetDataLogSelect(timeFrame)}
					FROM
						DATALOG{gardenId.Replace("-", "")}
					{GetWhere(timeFrame)}
					GROUP BY
						DATE,
						DEVICE_ID,
						SENSOR_ID,
						VALUE
					ORDER BY
						SENSOR_ID, DATE DESC
				) AS DATALOG
			JOIN 
				DEVICES
			ON 
				DEVICES.GARDEN_ID = '{gardenId}'
			JOIN 
				DEVICE_SENSORS
			ON 
				DATALOG.SENSOR_ID = DEVICE_SENSORS.SENSOR_ID
			{GetOrder(timeFrame)}".Clean();
    }

    public static string GetLastSensorValueQuery(string gardenId, string deviceId, string sensorId)
    {
        return @$"
			SELECT
				DISTINCT ON(DATALOG.SENSOR_ID)
				DATALOG.{DeviceStatic.Id},
				DATALOG.{DeviceStatic.Value},
				DATALOG.{DeviceStatic.UploadDate},
				DATALOG.{DeviceStatic.DeviceId},
				DATALOG.{DeviceStatic.SensorId},
				DEVICE_SENSORS.{DeviceStatic.UpperLimit},
				DEVICE_SENSORS.{DeviceStatic.LowerLimit},
				DEVICE_SENSORS.{DeviceStatic.Unit},
				DEVICE_SENSORS.{DeviceStatic.Name},
				DEVICE_SENSORS.{DeviceStatic.IsInverted},
				DEVICES.{DeviceStatic.IsManual}
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
			AND 
				DATALOG.{DeviceStatic.SensorId} = '{sensorId}'
			ORDER BY 
				DATALOG.SENSOR_ID, 
				UPLOAD_DATE DESC".Clean();
    }

    private static string GetDistinct(TimeFrame? timeFrame)
    {
        if (timeFrame is null)
            return " ";
        return " DISTINCT ON (UPLOAD_DATE, DEVICE_ID, DATALOG.SENSOR_ID) ";
    }

    private static string GetSelect(TimeFrame? timeFrame)
    {
        if (timeFrame is null)
            return " DISTINCT ON(DEVICE_SENSORS.SENSOR_ID) DATALOG.DATE AS UPLOAD_DATE, ";
        return " DATALOG.DATE AS UPLOAD_DATE, ";
    }

    private static string GetDataLogSelect(TimeFrame? timeFrame)
    {
        if (timeFrame is null)
            return " DISTINCT ON (SENSOR_ID) UPLOAD_DATE AS DATE, VALUE, DEVICE_ID, SENSOR_ID ";
        return " date_trunc('hour', UPLOAD_DATE) AS DATE, AVG (value) AS VALUE, DEVICE_ID, SENSOR_ID ";
    }

    private static object GetWhere(TimeFrame? timeFrame)
    {
        if (timeFrame is null)
            return "";
        return @$" WHERE UPLOAD_DATE BETWEEN '{timeFrame.Start.ConvertToPGString()}' AND '{timeFrame.End.ConvertToPGString()}' ";
    }

    private static object GetOrder(TimeFrame? timeFrame)
    {
        if (timeFrame is null)
            return "";
        return @$" ORDER BY UPLOAD_DATE DESC ";
    }
}

