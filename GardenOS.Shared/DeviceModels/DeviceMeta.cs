using Shared.Models;

namespace Shared.DeviceModels
{
    public class DeviceMeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GardenId { get; set; }
        public int DeviceType { get; set; }
        public string DisplayId { get; set; }
        public int SortOrder { get; set; }
        public string GroupId { get; set; }
        public bool IsManual { get; set; }

        public DeviceMeta(Dictionary<string, string> payload)
        {
            Id = DeviceStatic.GetString(payload, DeviceStatic.Id);
            Name = DeviceStatic.GetString(payload, DeviceStatic.Name);
            GardenId = DeviceStatic.GetString(payload, DeviceStatic.GardenID);
            DeviceType = DeviceStatic.GetInt(payload, DeviceStatic.DeviceTyp, 0);
            DisplayId = DeviceStatic.GetString(payload, DeviceStatic.DisplayId);
            SortOrder = DeviceStatic.GetInt(payload, DeviceStatic.SortOrder, 0);
            GroupId = DeviceStatic.GetString(payload, DeviceStatic.GroupId);
            IsManual = DeviceStatic.GetBool(payload, DeviceStatic.IsManual);
        }
    }
}
