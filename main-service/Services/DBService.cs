using Npgsql;
using dotenv.net;

namespace MainService.DB
{
    public class MainDB
    {
        String _connString = "";

        public MainDB()
        {
            DotEnv.Load();
            String host = Environment.GetEnvironmentVariable("PI_DB_HOST")!;
            String port = Environment.GetEnvironmentVariable("PI_DB_PORT")!;
            String user = Environment.GetEnvironmentVariable("PI_DB_USER")!;
            String password = Environment.GetEnvironmentVariable("PI_DB_PASSWORD")!;
            String database = Environment.GetEnvironmentVariable("PI_DB_DATABASE")!;
            _connString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";
        }

        public List<List<String>> query(String sql)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_connString))
            {
                var columns = new List<List<String>>();
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                //Console.WriteLine(sql);
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
                return columns;
            }
        }
    }
}