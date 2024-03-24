using API.Enums;
using Shared.Models;

namespace Shared.DeviceModels
{
    public class DeviceMeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GardenId { get; set; }
        public DeviceTypeId DeviceTypeId { get; set; }
        public string DisplayId { get; set; }
        public int SortOrder { get; set; }
        public string GroupId { get; set; }
        public bool IsManual { get; set; }
        public List<Sensor> Sensor { get; set; } = [];

        public DeviceMeta(Dictionary<string, string> devicePayload, List<Sensor> sensorMeta)
        {
            Id = DeviceStatic.GetString(devicePayload, DeviceStatic.Id);
            Name = DeviceStatic.GetString(devicePayload, DeviceStatic.DeviceName);
            GardenId = DeviceStatic.GetString(devicePayload, DeviceStatic.GardenID);
            DeviceTypeId = (DeviceTypeId)DeviceStatic.GetInt(devicePayload, DeviceStatic.DeviceTypId, 0);
            DisplayId = DeviceStatic.GetString(devicePayload, DeviceStatic.DisplayId);
            SortOrder = DeviceStatic.GetInt(devicePayload, DeviceStatic.SortOrder, 0);
            GroupId = DeviceStatic.GetString(devicePayload, DeviceStatic.GroupId);
            IsManual = DeviceStatic.GetBool(devicePayload, DeviceStatic.IsManual);

            Sensor = sensorMeta;
        }
    }
}
