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
}

public class RPIDevice
{
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
    public String Address = String.Empty;

    [JsonInclude]
    [JsonPropertyName("serial_id")]
    public int SerialId = 0;

    [JsonInclude]
    [JsonPropertyName("display_id")]
    public String DisplayID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("data_update_interval")]
    public TimeSpan DataUpdateInterval = TimeSpan.Parse("00:00:00");


    public RPIDevice(Dictionary<String, String> data)
    {
        this.DeviceName = DeviceStatic.GetString(data, DeviceStatic.DeviceName);
        this.LowerLimit = DeviceStatic.GetInt(data, DeviceStatic.LowerLimit, 0);
        this.UpperLimit = DeviceStatic.GetInt(data, DeviceStatic.UpperLimit, 100);
        this.DeviceTyp = DeviceStatic.GetString(data, DeviceStatic.DeviceTyp);
        this.Address = DeviceStatic.GetString(data, DeviceStatic.Address);
        this.SerialId = DeviceStatic.GetInt(data, DeviceStatic.SerialId, 0);
        this.DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
        this.DataUpdateInterval = DeviceStatic.GetTimeSpan(data, DeviceStatic.DataUpdateInterval);
    }
}