using Npgsql;
using dotenv.net;

namespace MainService.DB
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
            catch (Exception)
            {
                return new List<Dictionary<string, string>>();
            }
        }
    }
}