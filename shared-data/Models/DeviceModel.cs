using System.Text.Json.Serialization;

public class RPIDevices
{
    [JsonInclude]
    [JsonPropertyName("devices")]
    public List<RPIDevice> Devices = new();

    public RPIDevices(List<Dictionary<String, String>> data)
    {
        List<RPIDevice> devices = new();
        foreach (Dictionary<String, String> device in data)
        {
            devices.Add(new RPIDevice(device));
        }
        this.Devices = devices;
    }

    [JsonConstructor]
    public RPIDevices() { }
}

public class RPIDevice
{
    [JsonInclude]
    [JsonPropertyName("device_id")]
    public String ID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("device_name")]
    public String DeviceName = String.Empty;

    [JsonInclude]
    [JsonPropertyName("lower_limit")]
    public int LowerLimit = 0;

    [JsonInclude]
    [JsonPropertyName("upper_limit")]
    public int UpperLimit = 0;

    [JsonInclude]
    [JsonPropertyName("device_typ")]
    public String DeviceTyp = String.Empty;

    [JsonInclude]
    [JsonPropertyName("address")]
    public int Address = 0;

    [JsonInclude]
    [JsonPropertyName("serial_id")]
    public int SerialId = 0;

    [JsonInclude]
    [JsonPropertyName("display_id")]
    public String DisplayID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("data_update_interval")]
    public TimeSpan DataUpdateInterval = TimeSpan.Parse("00:00:00");

    [JsonInclude]
    [JsonPropertyName("sort_order")]
    public int SortOrder = -1;

    [JsonInclude]
    [JsonPropertyName("group_id")]
    public String GroupId = String.Empty;

    [JsonInclude]
    [JsonPropertyName("unit")]
    public String Unit = String.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public float Value = 0;

    public float LastSavedValue = -1;
    public DateTime LastEntry = DateTime.Now.AddYears(-1);

    public RPIDevice(Dictionary<String, String> data)
    {
        this.ID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
        this.DeviceName = DeviceStatic.GetString(data, DeviceStatic.DeviceName);
        this.LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
        this.UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
        this.DeviceTyp = DeviceStatic.GetString(data, DeviceStatic.DeviceTyp);
        this.Address = DeviceStatic.GetInt(data, DeviceStatic.Address, 0);
        this.SerialId = DeviceStatic.GetInt(data, DeviceStatic.SerialId, 0);
        this.DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
        this.SortOrder = DeviceStatic.GetInt(data, DeviceStatic.SortOrder, -1);
        this.GroupId = DeviceStatic.GetString(data, DeviceStatic.GroupId);
        this.Unit = DeviceStatic.GetString(data, DeviceStatic.Unit);
        this.DataUpdateInterval = DeviceStatic.GetTimeSpan(data, DeviceStatic.DataUpdateInterval);
    }

    [JsonConstructor]
    public RPIDevice() { }
}