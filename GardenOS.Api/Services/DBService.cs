using Npgsql;
using dotenv.net;
using ExtensionMethods;

namespace API.DB
{
    public static class MainDB
    {
        private static string _connString = "";

        public static void Init()
        {
            DotEnv.Load();
            string host = Environment.GetEnvironmentVariable("POSTGRES_HOST")!;
            string port = Environment.GetEnvironmentVariable("POSTGRES_PORT")!;
            string user = Environment.GetEnvironmentVariable("POSTGRES_USER")!;
            string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")!;
            string database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE")!;
            _connString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";
        }

        public static List<Dictionary<string, string>> Query(string sql)
        {
            try
            {
                using NpgsqlConnection con = new(_connString);
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                NpgsqlCommand command = new(sql, con);
                NpgsqlDataReader dr = command.ExecuteReader();

                List<Dictionary<string, string>> results = new();
                while (dr.Read())
                {
                    Dictionary<string, string> cells = new();

                    for (int i = 0; i < dr.FieldCount; i++)
                        cells.Add(dr.GetName(i).ToString(), dr[i].ToString()!);

                    results.Add(cells);
                }

                con.Close();
                return results;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Dictionary<string, string>>();
            }
        }

        public static void CleanDB()
        {
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

                INSERT INTO rpilog (
                        status,
                        triggerd_by,
                        rpi_id,
                        message
                    ) VALUES (
                        'info',
                        'Automatic Services',
                        'c4202eb8-cf7f-488a-bedd-0c5596847803',
                        'Table was cleaned successfully'
                    );
			COMMIT;".Clean();

            var result = Query(query);
            Console.WriteLine(result);
        }
    }
}