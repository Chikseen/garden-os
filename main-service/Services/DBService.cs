using Npgsql;
using dotenv.net;
using System.Runtime.InteropServices;

namespace MainService.DB
{
    public static class MainDB
    {
        private static String _connString = "";

        public static void Init()
        {
            DotEnv.Load();
            String host = Environment.GetEnvironmentVariable("POSTGRES_HOST")!;
            String port = Environment.GetEnvironmentVariable("POSTGRES_PORT")!;
            String user = Environment.GetEnvironmentVariable("POSTGRES_USER")!;
            String password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")!;
            String database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE")!;
            _connString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";
        }

        public static List<Dictionary<String, String>> query(String sql)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(_connString))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    NpgsqlCommand command = new NpgsqlCommand(sql, con);
                    NpgsqlDataReader dr = command.ExecuteReader();

                    List<Dictionary<String, String>> results = new();
                    while (dr.Read())
                    {
                        Dictionary<String, String> cells = new();

                        for (int i = 0; i < dr.FieldCount; i++)
                            cells.Add(dr.GetName(i).ToString(), dr[i].ToString()!);

                        results.Add(cells);
                    }

                    con.Close();
                    return results;
                }
            }
            catch (System.Exception)
            {
                return new List<Dictionary<String, String>>();
            }
        }

        private static List<Dictionary<String, String>> DbFake(String sql)
        {
            return new List<Dictionary<String, String>>();
        }
    }
}