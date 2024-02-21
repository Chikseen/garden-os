using ESP_sensor.Models;
using Shared;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDeviceService
    {
        ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame);
        ReponseDevice GetDevice(GardenId garden, string deviceId, double value);
        GardenInfo GetGardenInfo(string gardenId);
        ResponseDevices GetOverview(UserData userData, string gardenId);
        RPIDevices? GetRPIDevices(string rpiId, string apiKey);
        RPIData? GetRpiMeta(string rpiId, string apiKey);
        DeveiceStatus GetStatus(string gardenId);
        List<DeveiceStatus> GetStatusLog(string gardenId);
        void PatchDevice(PatchDeviceRequest device);
        ReponseDevice? SaveDataToDB(SaveDataRequest data, string rpiId, string rpiKey);
        DeveiceStatus SetStatus(DeveiceStatus status);
        bool IsCredentialsValid(StandaloneDevice data);
        ReponseDevice StoreData(DeviceInput data);
    }
}