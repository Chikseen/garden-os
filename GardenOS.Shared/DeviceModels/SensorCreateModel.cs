namespace Shared.DeviceModels
{
    public class SensorCreateModel
    {
        public int UpperLimit { get; set; }
        public int LowerLimit { get; set; }
        public int SensorTypeId { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
