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

            AdjustValues();
        }

        private void AdjustValues()
        {
            if (DeviceTypeId == DeviceTypeId.SoilTempAndMoistureBatteryDevice)
            {
                foreach (Sensor sensor in Sensor)
                {
                    Sensor? deviceBattery = Sensor.FirstOrDefault(sensor => sensor.SensorTypeId == SensorTypeId.Battery);

                    switch (sensor.SensorTypeId)
                    {
                        case SensorTypeId.SoilMoisture:
                            {
                                if (deviceBattery is not null)
                                    sensor.Value = sensor.Value + ((deviceBattery.UpperLimit - deviceBattery.Value));
                            }
                            break;
                    }
                    sensor.CorrectValues();
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
        }

        public void CorrectValues()
        {
            switch (SensorTypeId)
            {
                case SensorTypeId.SoilTemperature:
                    {
                        CalculateTempertureValue();
                    }
                    break;
                default:
                    {
                        if (IsInverted)
                            CorrectedValue = 100 - (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
                        else

                            CorrectedValue = (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
                    }
                    break;
            };
            if (float.IsInfinity(CorrectedValue) || float.IsNegativeInfinity(CorrectedValue) || float.IsNaN(CorrectedValue))
                CorrectedValue = 0;
        }

        private void CalculateTempertureValue()
        {
            const double beta = 3950d;
            const double tConstantRoom = 298.15d;
            const double rInTConstant = 10000d;
            const double rBalance = 9800d;
            const double vMax = 22560d;

            if (SensorTypeId == SensorTypeId.SoilTemperature)
            {
                double rThermistor = rBalance * ((vMax / Value) - 1);
                double tKelvin = (beta * tConstantRoom) / (beta + (tConstantRoom * Math.Log(rThermistor / rInTConstant)));
                CorrectedValue = (float)(tKelvin - 273.15d);
            }
        }
    }
}
