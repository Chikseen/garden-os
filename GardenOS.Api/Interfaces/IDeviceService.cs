using Shared;
using Shared.DeviceModels;
using Shared.Enums;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDeviceService
    {
        void StoreData(DeviceInput model);
        List<DeviceMeta> GetAllDevices(string gardenId);
        List<Sensor> GetSensorMeta(string gardenId);
        Device GetSensorValues(string gardenId, string deviceId, TimeFrameId timeFrameId);
        DetailedChartData GetDetailedTimeFrame(string gardenId, string deviceId, TimeFrame timeFrame);
        Device UploadNewValue(NewManualValueModel model);
        void DeleteManualEntry(string gardenId, string entryId);
        void CreateNewDevice(DeviceCreateModel model);
        void CreateNewSensor(SensorCreateModel model);
    }
}