using System.Text.Json.Serialization;

public class DevicesData
{
    [JsonInclude]
    public List<Device> Devices = new();

    public DevicesData(List<Dictionary<String, String>> devices)
    {
        foreach (var d in devices)
        {
            Devices.Add(new Device(d));
        }
    }

    public DevicesData()
    {

    }
}

public class Device
{
    [JsonInclude]
    public DateTime LastEntry = new();

    [JsonInclude]
    public String DeviceId = String.Empty;

    [JsonInclude]
    public Int16 RawValue = -1;

    [JsonInclude]
    public DateTime Date = new();

    public Device(Dictionary<String, String> device)
    {
        device.TryGetValue(DeviceStatic.DeviceId, out DeviceId!);
    }
}