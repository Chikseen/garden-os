using ESP_sensor.Models.DeviceTypes;

namespace ESP_sensor.Models
{
    public class StandaloneDevice
    {
        public float Value {  get; set; }
        public string DeviceId { get; init; }
        public string Name { get; init; }
        public string GardenId { get; init; }
        public string ApiKey { get; init; }
        public DeviceType DeviceType { get; init; }
        public SensorType SensorType { get; init; }
        public ControllType ControllType { get; init; }
    }
}