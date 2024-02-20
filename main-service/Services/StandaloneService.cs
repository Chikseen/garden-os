using ESP_sensor.Models;
using MainService.DB;

namespace MainService.Services
{
    public class StandaloneService
    {
        public bool StoreData(StandaloneDevice data)
        {
            string query = StandaloneQueryService.InsertNewDataQuery(data);
            MainDB.Query(query);

            return true;
        }

        public bool IsCredentialsValid(StandaloneDevice data)
        {
            string query = StandaloneQueryService.CheckCredentialsQuery(data);
            List<Dictionary<string, string>> response = MainDB.Query(query);

            if (response.Count != 1)
                return false;
            return true;
        }
    }
}