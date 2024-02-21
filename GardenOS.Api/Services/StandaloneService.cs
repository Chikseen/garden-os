using API.Interfaces;
using ESP_sensor.Models;
using MainService.DB;
using Shared;

namespace MainService.Services
{
    public class StandaloneService : IStandaloneService
    {
        public bool StoreData(DeviceInput data)
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