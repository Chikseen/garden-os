using ESP_sensor.Models;
using Shared.DeviceModels;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDeviceService
    {
        void StoreData(DeviceInput model);
        ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame);
        GardenInfo GetGardenInfo(string gardenId);
        ResponseDevices GetOverview(UserData userData, string gardenId);
        RPIDevices? GetRPIDevices(string rpiId, string apiKey);
        RPIData? GetRpiMeta(string rpiId, string apiKey);
        DeveiceStatus GetStatus(string gardenId);
        List<DeveiceStatus> GetStatusLog(string gardenId);
        void PatchDevice(PatchDeviceRequest device);
        DeveiceStatus SetStatus(DeveiceStatus status);
        bool IsCredentialsValid(StandaloneDevice data);
    }
}