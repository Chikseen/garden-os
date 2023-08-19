using ExtensionMethods;
using MainService.DB;

public class TimeService
{
	public void SetUpDailyTimer(TimeSpan alertTime)
	{
		TimeSpan timeToGo = alertTime - DateTime.Now.TimeOfDay;
		if (timeToGo < TimeSpan.Zero)
		{
			Console.WriteLine("Set timer for next day");
			timeToGo += new TimeSpan(24, 0, 0); // assign it as "+" since the time to go will be negative
		}
		Console.WriteLine("Timer will activate in " + timeToGo);
		_ = new System.Threading.Timer(x =>
		{
			Thread.Sleep(1000);
			CleanDB();
			SetUpDailyTimer(alertTime);
		}, null, timeToGo, Timeout.InfiniteTimeSpan);
	}

	private static void CleanDB()
	{
		Console.WriteLine("Auto clean DB");
		string query = @$"
			BEGIN;
				CREATE TEMPORARY TABLE IF NOT EXISTS tempTable ON COMMIT DROP AS TABLE datalogaccd30d2739240b78a086d9ac9cc22b6;

				DELETE FROM
					tempTable;

				INSERT INTO
					tempTable
				SELECT
					gen_random_uuid(),
					AVG (value) AS VALUE,
					date_trunc('hour', UPLOAD_DATE) AS DATE,
					DEVICE_ID
				FROM
					datalogaccd30d2739240b78a086d9ac9cc22b6
				GROUP BY
					DATE,
					DEVICE_ID;

				DELETE FROM
					datalogaccd30d2739240b78a086d9ac9cc22b6;

				INSERT INTO
					datalogaccd30d2739240b78a086d9ac9cc22b6
				SELECT
					*
				FROM
					tempTable;
			COMMIT;".Clean();
		var result = MainDB.Query(query);
		Console.WriteLine(result);
	}
}