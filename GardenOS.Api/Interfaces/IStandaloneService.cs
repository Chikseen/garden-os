using ESP_sensor.Models;

namespace API.Interfaces
{
    public interface IStandaloneService
    {
        bool IsCredentialsValid(StandaloneDevice data);
        bool StoreData(StandaloneDevice data);
    }
}