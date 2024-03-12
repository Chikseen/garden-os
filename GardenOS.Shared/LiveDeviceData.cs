using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class SaveDataRequest
    {
        [JsonInclude]
        [JsonPropertyName("device_id")]
        public string DeviceId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("value")]
        public float Value = 0;
    }

    public class ResponseDevices
    {
        [JsonInclude]
        [JsonPropertyName("devices")]
        public List<ReponseDevice> Devices = new();

        public ResponseDevices(List<Dictionary<string, string>> data)
        {
            List<ReponseDevice> devices = new();
            foreach (Dictionary<string, string> device in data)
            {
                devices.Add(new ReponseDevice(device));
            }
            Devices = devices;
        }

        [JsonConstructor]
        public ResponseDevices() { }
    }

    public class ReponseDevice
    {
        public string DeviceID { get; set; } = string.Empty;
        public string SensorId { get; set; } = string.Empty;
        public string EntryID { get; set; } = string.Empty;
        public float Value { get; set; } = 0;
        public float CorrectedValue { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Name { get; set; } = string.Empty;
        public string DisplayID { get; set; } = string.Empty;
        public int UpperLimit { get; set; } = 100;
        public int LowerLimit { get; set; } = 0;
        public int SortOrder { get; set; } = -1;
        public string GroupId { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string SpecialId { get; set; } = string.Empty;
        public bool IsInverted { get; set; } = false;
        public bool IsManual { get; set; } = false;

        public ReponseDevice(Dictionary<string, string> data)
        {
            DeviceID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
            EntryID = DeviceStatic.GetString(data, DeviceStatic.Id);
            Value = DeviceStatic.GetFloat(data, DeviceStatic.Value, 0);
            Date = DeviceStatic.GetDateTime(data, DeviceStatic.UploadDate);
            Name = DeviceStatic.GetString(data, DeviceStatic.Name);
            DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
            UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
            LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
            SortOrder = DeviceStatic.GetInt(data, DeviceStatic.SortOrder, -1);
            GroupId = DeviceStatic.GetString(data, DeviceStatic.GroupId);
            Unit = DeviceStatic.GetString(data, DeviceStatic.Unit);
            SpecialId = DeviceStatic.GetString(data, DeviceStatic.SpecialId);
            SensorId = DeviceStatic.GetString(data, DeviceStatic.SensorId);
            IsInverted = DeviceStatic.GetBool(data, DeviceStatic.IsInverted, false);
            IsManual = DeviceStatic.GetBool(data, DeviceStatic.IsManual, false);

            Invert();
            AdjustSpecial();
            SetTimeZone();
        }

        public ReponseDevice() { }

        private void Invert()
        {
            if (IsInverted)
                CorrectedValue = 100 - (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
            else
                CorrectedValue = (float)(Value - LowerLimit) * 100.0f / (UpperLimit - LowerLimit);
        }

        private void AdjustSpecial()
        {
            if (!string.IsNullOrEmpty(SpecialId))
            {
                switch (SpecialId)
                {
                    case "brightness":
                        {
                            CorrectedValue /= 10;
                        }
                        break;
                }
            }
        }

        private void SetTimeZone()
        {
            Date = Date.ToLocalTime();
        }
    }
}