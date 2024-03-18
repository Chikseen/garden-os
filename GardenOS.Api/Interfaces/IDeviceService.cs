using Shared;
using Shared.DeviceModels;
using Shared.Enums;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDeviceService
    {
        void StoreData(DeviceInput model);
        void CreateNewDevice(DeviceCreateModel model);
        List<DeviceMeta> GetAllDevices(string gardenId);
        Device GetSensorValues(string gardenId, string deviceId, TimeFrameId timeFrameId);
        DetailedChartData GetDetailedTimeFrame(string gardenId, string deviceId, TimeFrame timeFrame);
        Device UploadNewValue(NewManualValueModel model);
        void DeleteManualEntry(string gardenId, string entryId);
    }
}