using ESP_sensor.Models;
using Shared;

namespace API.Interfaces
{
    public interface IStandaloneService
    {
        bool IsCredentialsValid(StandaloneDevice data);
        bool StoreData(DeviceInput data);
    }
}