using System.Text.Json.Serialization;

namespace shared_data.Models
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
        [JsonInclude]
        [JsonPropertyName("device_id")]
        public string DeviceID = string.Empty;

        [JsonInclude]
        [JsonPropertyName("entry_id")]
        public string EntryID = string.Empty;

        [JsonInclude]
        [JsonPropertyName("value")]
        public float Value = 0;

        [JsonInclude]
        [JsonPropertyName("corrected_value")]
        public float CorrectedValue = 0;

        [JsonInclude]
        [JsonPropertyName("date")]
        public DateTime Date = DateTime.Now;

        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name = string.Empty;

        [JsonInclude]
        [JsonPropertyName("display_id")]
        public string DisplayID = string.Empty;

        [JsonInclude]
        [JsonPropertyName("upper_limit")]
        public int UpperLimit = 100;

        [JsonInclude]
        [JsonPropertyName("lower_limit")]
        public int LowerLimit = 0;

        [JsonInclude]
        [JsonPropertyName("sort_order")]
        public int SortOrder = -1;

        [JsonInclude]
        [JsonPropertyName("group_id")]
        public string GroupId = string.Empty;

        [JsonInclude]
        [JsonPropertyName("unit")]
        public string Unit = string.Empty;

        [JsonInclude]
        [JsonPropertyName("special_id")]
        public string SpecialId = string.Empty;

        public bool IsInverted = false;

        public ReponseDevice(Dictionary<string, string> data)
        {
            DeviceID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
            EntryID = DeviceStatic.GetString(data, DeviceStatic.ID);
            Value = DeviceStatic.GetFloat(data, DeviceStatic.Value, 0);
            Date = DeviceStatic.GetDateTime(data, DeviceStatic.UploadDate);
            Name = DeviceStatic.GetString(data, DeviceStatic.Name);
            DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
            UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
            LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
            SortOrder = DeviceStatic.GetInt(data, DeviceStatic.SortOrder, -1);
            GroupId = DeviceStatic.GetString(data, DeviceStatic.GroupId);
            Unit = DeviceStatic.GetString(data, DeviceStatic.Unit);
            IsInverted = DeviceStatic.GetBool(data, DeviceStatic.IsInverted, false);
            SpecialId = DeviceStatic.GetString(data, DeviceStatic.SpecialId);

            Invert();
            AdjustSpecial();
        }

        public void SetNewValue(float value)
        {
            Value = value;
            Date = DateTime.Now;
            Invert();
            AdjustSpecial();
        }

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
    }
}