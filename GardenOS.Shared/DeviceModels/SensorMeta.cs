using Shared.Models;

namespace Shared.DeviceModels
{
    public class SensorMeta
    {
        public string DeviceId { get; set; }
        public string SensorId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public bool IsManual { get; set; }

        public SensorMeta(Dictionary<string, string> payload)
        {
            DeviceId = DeviceStatic.GetString(payload, DeviceStatic.DeviceId);
            SensorId = DeviceStatic.GetString(payload, DeviceStatic.SensorId);
            Name = DeviceStatic.GetString(payload, DeviceStatic.Name);
            Unit = DeviceStatic.GetString(payload, DeviceStatic.Unit);
            IsManual = DeviceStatic.GetBool(payload, DeviceStatic.IsManual);
        }
    }
}
