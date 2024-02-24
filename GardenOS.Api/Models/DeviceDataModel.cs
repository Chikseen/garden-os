using System.Text.Json.Serialization;

public class DevicesData
{
    [JsonInclude]
    public List<DeviceModel> Devices = new();

    public DevicesData(List<Dictionary<string, string>> devicesList)
    {
        List<DeviceModel> devices = new();
        foreach (var deciveDictionary in devicesList)
        {
            devices.Add(new DeviceModel(deciveDictionary));
        }
        Devices = devices;
    }

    public DevicesData()
    { }
}