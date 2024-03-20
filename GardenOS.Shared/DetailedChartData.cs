using Shared.Models;
using System.Text.Json.Serialization;

namespace Shared
{
    public class DetailedChartData
    {
        [JsonInclude]
        string DeviceName { get; set; } = string.Empty;

        [JsonInclude]
        HashSet<DateTime> XAxis { get; set; } = [];

        [JsonInclude]
        List<string> EntryIds { get; set; } = [];

        [JsonInclude]
        List<List<float?>> YAxis { get; set; } = [];

        [JsonInclude]
        List<Sensor> Sensors { get; set; } = [];

        public DetailedChartData() { }

        public DetailedChartData(List<Device> devices)
        {
            SetXAxis(devices);
            SetNames(devices);
            SetYAxis(devices);
        }

        private void SetXAxis(List<Device> devices)
        {
            foreach (Device device in devices)
            {
                XAxis.Add(device.Date);
                EntryIds.Add(device.EntryId);
            }
        }

        private void SetYAxis(List<Device> devices)
        {
            foreach (DateTime date in XAxis)
            {
                Device? device = devices.Find(d => d.Date == date);
                if (device is null)
                    continue;

                List<float?> sensorData = [];
                foreach (Sensor sensor in Sensors)
                {
                    Sensor? sensorFromDevice = device.Sensor.Find(s => s.SensorId == sensor.SensorId);
                    if (sensorFromDevice is null)
                        sensorData.Add(null);
                    else
                        sensorData.Add(sensorFromDevice.CorrectedValue);
                }

                YAxis.Add(sensorData);
            }
        }

        private void SetNames(List<Device> devices)
        {

            foreach (Device device in devices)
            {
                DeviceName = device.Name;

                foreach (Sensor sensor in device.Sensor)
                {
                    if (!Sensors.Any(s => s.SensorId == sensor.SensorId))
                        Sensors.Add(sensor);
                }
            }
        }
    }
}
