using System.Text.Json.Serialization;

public class HardwareData
{
    [JsonInclude]
    public uint PotiOne = 0;

    [JsonInclude]
    public uint PotiTwo = 0;

    // use Named Parameters here later
    /* public HardwareData(Dictionary<string, string> data)
     {
         Data = data;
     }*/
}