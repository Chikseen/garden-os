using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class DevicesData
{
    [JsonInclude]
    public List<Device> Devices = new();

    public DevicesData(List<Dictionary<String, String>> devicesList)
    {
        List<Device> devices = new();
        foreach (var deciveDictionary in devicesList)
        {
            devices.Add(new Device(deciveDictionary));
        }
        Devices = devices;
    }

    public DevicesData()
    { }
}