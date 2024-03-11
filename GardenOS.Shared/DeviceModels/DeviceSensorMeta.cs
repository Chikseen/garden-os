using Shared.Models;

namespace Shared.DeviceModels
{
    public class DeviceSensorMeta
    {
        public string DeviceId { get; set; }
        public string SensorId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public bool IsInverted { get; set; }

        public DeviceSensorMeta(Dictionary<string, string> payload)
        {
            DeviceId = DeviceStatic.GetString(payload, DeviceStatic.DeviceId);
            SensorId = DeviceStatic.GetString(payload, DeviceStatic.SensorId);
            Name = DeviceStatic.GetString(payload, DeviceStatic.Name);
            Unit = DeviceStatic.GetString(payload, DeviceStatic.Unit);
            LowerLimit = DeviceStatic.GetInt(payload, DeviceStatic.LowerLimit, 0);
            UpperLimit = DeviceStatic.GetInt(payload, DeviceStatic.UpperLimit, 32000);
            IsInverted = DeviceStatic.GetBool(payload, DeviceStatic.IsInverted);
        }
    }
}
