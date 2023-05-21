using System.Text.Json.Serialization;

public class SaveDataRequest
{
    [JsonInclude]
    [JsonPropertyName("device_id")]
    public String Device_ID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public int Value = 0;
}

public class ResponseDevices
{
    [JsonInclude]
    [JsonPropertyName("devices")]
    public List<ReponseDevice> Devices = new();

    public ResponseDevices(List<Dictionary<String, String>> data)
    {
        List<ReponseDevice> devices = new();
        foreach (Dictionary<String, String> device in data)
        {
            devices.Add(new ReponseDevice(device));
        }
        this.Devices = devices;
    }

    [JsonConstructor]
    public ResponseDevices() { }
}

public class ReponseDevice
{
    [JsonInclude]
    [JsonPropertyName("device_id")]
    public String DeviceID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("entry_id")]
    public String EntryID = String.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public int Value = 0;

    [JsonInclude]
    [JsonPropertyName("date")]
    public DateTime date = DateTime.Now;

    [JsonInclude]
    [JsonPropertyName("name")]
    public String Name = String.Empty;

    [JsonInclude]
    [JsonPropertyName("display_id")]
    public String DisplayID = String.Empty;

    public ReponseDevice(Dictionary<String, String> data)
    {
        this.DeviceID = DeviceStatic.GetString(data, DeviceStatic.DeviceId);
        this.EntryID = DeviceStatic.GetString(data, DeviceStatic.ID);
        this.Value = DeviceStatic.GetInt(data, DeviceStatic.Value, 0);
        this.date = DeviceStatic.GetDateTime(data, DeviceStatic.Date);
        this.Name = DeviceStatic.GetString(data, DeviceStatic.Name);
        this.DisplayID = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
    }
}