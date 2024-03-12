using ESP_sensor.Models;
using Shared;
using Shared.DeviceModels;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDeviceService
    {
        void StoreData(DeviceInput model);
        ResponseDevices GetDetailed(string gardenId, TimeFrame timeFrame);
        GardenInfo GetGardenInfo(string gardenId);
        bool IsCredentialsValid(StandaloneDevice data);
        void CreateNewDevice(DeviceCreateModel model);
        List<DeviceMeta> GetAllDevices(string gardenId);
        List<DeviceSensorMeta> GetDevice(string deviceId);
        ReponseDevice GetLastSensorValue(string gardenId, string deviceId, string sensorId);
        ReponseDevice UploadNewValue(NewManualValueModel model);
    }
}