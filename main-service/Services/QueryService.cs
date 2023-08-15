using System.Globalization;
using ExtensionMethods;


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
				DEVICES.LOWER_LIMIT,
				DEVICES.UPPER_LIMIT,
				DEVICES.DEVICE_TYP,
				DEVICES.ADDRESS,
				DEVICES.SERIAL_ID,
				DEVICES.DISPLAY_ID,
				DEVICES.DATA_UPDATE_INTERVAL, 
				DEVICES.SORT_ORDER, 
				DEVICES.GROUP_ID, 
				DEVICES.UNIT 
			FROM
				RPIS
				JOIN GARDEN
				ON RPIS.ID = '{rpiId}'
				AND RPIS.API_KEY = '{apiKey}'
				AND RPIS.GARDEN_ID = GARDEN.ID JOIN DEVICES
				ON RPIS.GARDEN_ID = DEVICES.GARDEN_ID;".Clean();
	}

	public static string SaveDataToDatabaseQuery(Garden garden, SaveDataRequest data)
	{
		return @$"
			SET TIMEZONE = 'Europe/Berlin';
			INSERT INTO DATALOG{garden.Id} (ID, VALUE, UPLOAD_DATE, DEVICE_ID) 
			VALUES (GEN_RANDOM_UUID(), {data.Value.ToString("G", CultureInfo.InvariantCulture)}, Now(), '{data.DeviceId}')".Clean();
	}

	public static string OverviewQuery(string gardenId)
	{
		return @$"
			{OverviewSelectDistinct()}
			FROM
				DATALOG{gardenId.Replace("-", "")} AS DATALOG
				JOIN devices ON devices.id = DATALOG.device_id
				AND devices.garden_id = '{gardenId}'
			ORDER BY
				DEVICE_ID,
				UPLOAD_DATE DESC;".Clean();
	}

	public static string OverviewDeviceFromRPI(string gardenId, string deviceId)
	{
		return @$"
			{OverviewSelectDistinct()}
			FROM
				DATALOG{gardenId.Replace("-", "")} AS DATALOG
				JOIN DEVICES
				ON DATALOG.DEVICE_ID = '{deviceId}'
				AND DEVICES.ID = '{deviceId}'
				AND DEVICES.GARDEN_ID = '{gardenId}'
			ORDER BY
				DEVICE_ID,
				UPLOAD_DATE DESC;".Clean();
	}

	public static string DetailedViewQuery(string gardenId, TimeFrame timeFrame)
	{
		return @$"
			SELECT
				DATALOG.DATE AS UPLOAD_DATE,
				DATALOG.device_id AS DEVICE_ID,
				DATALOG.VALUE AS VALUE,
				DEVICES.NAME,
				DEVICES.DISPLAY_ID,
				DEVICES.UPPER_LIMIT,
				DEVICES.LOWER_LIMIT,
				DEVICES.ISINVERTED, 
				DEVICES.SORT_ORDER, 
				DEVICES.GROUP_ID, 
				DEVICES.UNIT,
				DEVICES.SPECIAL_ID
			FROM
				(
					SELECT
						date_trunc('hour', UPLOAD_DATE) AS DATE,
						AVG (value) AS VALUE,
						DEVICE_ID
					FROM
						DATALOG{gardenId.Replace("-", "")}
					GROUP BY
						DATE,
						DEVICE_ID
				) AS DATALOG
				JOIN DEVICES ON DEVICES.ID = DEVICE_ID
				AND DEVICES.GARDEN_ID = '{gardenId}'
				AND DATALOG.DATE BETWEEN '{timeFrame.Start.ConvertToPGString()}'
				AND '{timeFrame.End.ConvertToPGString()}'
			ORDER BY
				UPLOAD_DATE DESC;".Clean();
	}

	public static string SetStatusQuery(DeveiceStatus status)
	{
		return @$"
			INSERT INTO rpilog (
				status,
				triggerd_by,
				rpi_id,
				message
			) VALUES (
				'{status.Status}',
				'{status.TriggerdBy}',
				'{status.RpiId}',
				'{status.Message}'
			);".Clean();
	}

	public static string GetStatusQuery(string gardenId)
	{
		return @$"
			SELECT
				DISTINCT ON (rpi_id)
				rpilog.rpi_id AS rpi_id,
				rpilog.date,
				rpilog.status,
				rpilog.triggerd_by,
				rpilog.message
			FROM
				rpilog
				JOIN rpis ON rpis.garden_id = '{gardenId}'
				AND rpis.id = rpilog.rpi_id
			ORDER BY
				rpi_id,
				rpilog.date DESC ".Clean();
	}

	public static string GetStatusLogQuery(string gardenId)
	{
		return @$"
			SELECT
				rpilog.rpi_id AS rpi_id,
				rpilog.date,
				rpilog.status,
				rpilog.triggerd_by,
				rpilog.message
			FROM
				rpilog
				JOIN rpis ON rpis.garden_id = '{gardenId}'
				AND rpis.id = rpilog.rpi_id
			ORDER BY
				rpilog.date DESC
			LIMIT
				20;".Clean();
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
				DEVICES.UPPER_LIMIT,
				DEVICES.LOWER_LIMIT,
				DEVICES.ISINVERTED, 
				DEVICES.SORT_ORDER, 
				DEVICES.GROUP_ID, 
				DEVICES.UNIT,
				DEVICES.SPECIAL_ID";
	}
}

