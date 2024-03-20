using API.Enums;

namespace Shared.Models
{
    public class Device
    {
        public bool IsManual { get; set; } = false;
        public int SortOrder { get; set; } = -1;
        public string DeviceId { get; set; } = string.Empty;
        public string EntryId { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string SpecialId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayId { get; set; } = string.Empty;
        public DeviceTypeId DeviceTypeId { get; set; }
        public List<Sensor> Sensor { get; set; } = [];
        public DateTime Date { get; set; } = DateTime.Now;

        public Device() { }

        public Device(List<Dictionary<string, string>> payload)
        {
            var deviceMeta = payload.FirstOrDefault();

            if (deviceMeta is null)
                return;

            IsManual = DeviceStatic.GetBool(deviceMeta, DeviceStatic.IsManual, false);
            SortOrder = DeviceStatic.GetInt(deviceMeta, DeviceStatic.SortOrder, -1);
            EntryId = DeviceStatic.GetString(deviceMeta, DeviceStatic.Id);
            DeviceId = DeviceStatic.GetString(deviceMeta, DeviceStatic.DeviceId);
            GroupId = DeviceStatic.GetString(deviceMeta, DeviceStatic.GroupId);
            SpecialId = DeviceStatic.GetString(deviceMeta, DeviceStatic.SpecialId);
            Name = DeviceStatic.GetString(deviceMeta, DeviceStatic.DeviceName);
            DisplayId = DeviceStatic.GetString(deviceMeta, DeviceStatic.DisplayId);
            DeviceTypeId = (DeviceTypeId)DeviceStatic.GetInt(deviceMeta, DeviceStatic.DeviceTypId)!;
            Date = DeviceStatic.GetLocalDateTime(deviceMeta, DeviceStatic.UploadDate);

            foreach (var device in payload)
            {
                Sensor.Add(new(device));
            }

            AdjustValueIfBatteryDevice();
        }

        private void AdjustValueIfBatteryDevice()
        {
            if (DeviceTypeId == DeviceTypeId.SoilMoistureBattery)
            {
                foreach (Sensor sensor in Sensor)
                {
                    if (sensor.SensorTypeId == SensorTypeId.SoilMoisture)
                    {
                        Sensor? deviceBattery = Sensor.FirstOrDefault(sensor => sensor.SensorTypeId == SensorTypeId.Battery);

                        if (deviceBattery is null)
                        {
                            Console.Error.WriteLine($"Device {DeviceId} is set as battery device but has no battery value!");
                            return;
                        }

                        sensor.Value = sensor.Value + ((deviceBattery.UpperLimit - deviceBattery.Value) * 0.715f);
                        sensor.SetValues();
                    }
                }
            }
        }
    }

    public class Sensor
    {
        public bool IsManual { get; set; } = false;
        public bool IsInverted { get; set; } = false;
        public int UpperLimit { get; set; } = 100;
        public int LowerLimit { get; set; } = 0;
        public float Value { get; set; } = 0;
        public float CorrectedValue { get; set; } = 0;
        public string DeviceId { get; set; } = string.Empty;
        public string SensorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public SensorTypeId SensorTypeId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Sensor() { }

        public Sensor(Dictionary<string, string> sensorData)
        {
            IsManual = DeviceStatic.GetBool(sensorData, DeviceStatic.IsManual, false);
            IsInverted = DeviceStatic.GetBool(sensorData, DeviceStatic.IsInverted, false);
            UpperLimit = DeviceStatic.GetInt(sensorData, DeviceStatic.UpperLimit, 100);
            LowerLimit = DeviceStatic.GetInt(sensorData, DeviceStatic.LowerLimit, 0);
            Value = DeviceStatic.GetFloat(sensorData, DeviceStatic.Value, 0);
            DeviceId = DeviceStatic.GetString(sensorData, DeviceStatic.DeviceId);
            SensorId = DeviceStatic.GetString(sensorData, DeviceStatic.SensorId);
            Name = DeviceStatic.GetString(sensorData, DeviceStatic.SensorName);
            Unit = DeviceStatic.GetString(sensorData, DeviceStatic.Unit);
            SensorTypeId = (SensorTypeId)DeviceStatic.GetInt(sensorData, DeviceStatic.SensorTypeId)!;
            Date = DeviceStatic.GetLocalDateTime(sensorData, DeviceStatic.UploadDate);

            SetValues();
            CalculateTempertureValue();
        }

        public void SetValues()
        {
            if (IsInverted)
                CorrectedValue = 100 - (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
            else
                CorrectedValue = (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
        }

        public void CalculateTempertureValue()
        {
            const double beta = 3950;
            const double normalRoomTemp = 20;
            const double resitanceWithRoomTempinKOhm = 10;

            if (SensorTypeId == SensorTypeId.SoilTemperature)
            {
                double voltage = Value * 0.000125d;
                double tKelvin = (beta * normalRoomTemp) / (beta + (normalRoomTemp * Math.Log(voltage / resitanceWithRoomTempinKOhm)));
                CorrectedValue = (float)tKelvin;
            }
        }
    }
}