using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class RPIDevices
    {
        [JsonInclude]
        [JsonPropertyName("devices")]
        public List<RPIDevice> Devices = new();

        public RPIDevices(List<Dictionary<string, string>> data)
        {
            List<RPIDevice> devices = new();
            foreach (Dictionary<string, string> device in data)
            {
                devices.Add(new RPIDevice(device));
            }
            Devices = devices;
        }

        [JsonConstructor]
        public RPIDevices() { }
    }

    public class RPIDevice
    {
        [JsonInclude]
        [JsonPropertyName("device_id")]
        public string ID = string.Empty;

        [JsonInclude]
        [JsonPropertyName("device_name")]
        public string DeviceName = string.Empty;

        [JsonInclude]
        [JsonPropertyName("lower_limit")]
        public int LowerLimit = 0;

        [JsonInclude]
        [JsonPropertyName("upper_limit")]
        public int UpperLimit = 0;

        [JsonInclude]
        [JsonPropertyName("device_typ")]
        public string DeviceTyp = string.Empty;

        [JsonInclude]
        [JsonPropertyName("address")]
        public int Address = 0;

        [JsonInclude]
        [JsonPropertyName("serial_id")]
        public int SerialId = 0;

        [JsonInclude]
        [JsonPropertyName("display_id")]
        public string DisplayID = string.Empty;

        [JsonInclude]
        [JsonPropertyName("data_update_interval")]
        public TimeSpan DataUpdateInterval = TimeSpan.Parse("00:00:00");

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
        [JsonPropertyName("value")]
        public float Value = 0;

        [JsonInclude]
        [JsonPropertyName("threshold")]
        public float Threshold = 0;

        public float LastSavedValue = -1;
        public DateTime LastEntry = DateTime.Now.AddYears(-1);

        public RPIDevice(Dictionary<string, string> data)
        {
            ID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
            DeviceName = DeviceStatic.GetString(data, DeviceStatic.DeviceName);
            LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
            UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
            DeviceTyp = DeviceStatic.GetString(data, DeviceStatic.DeviceTyp);
            Address = DeviceStatic.GetInt(data, DeviceStatic.Address, 0);
            SerialId = DeviceStatic.GetInt(data, DeviceStatic.SerialId, 0);
            DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
            SortOrder = DeviceStatic.GetInt(data, DeviceStatic.SortOrder, -1);
            GroupId = DeviceStatic.GetString(data, DeviceStatic.GroupId);
            Unit = DeviceStatic.GetString(data, DeviceStatic.Unit);
            DataUpdateInterval = DeviceStatic.GetTimeSpan(data, DeviceStatic.DataUpdateInterval);
        }

        [JsonConstructor]
        public RPIDevice() { }
    }
}