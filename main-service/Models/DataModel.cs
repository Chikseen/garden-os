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

    public static bool operator ==(HardwareData A1, HardwareData A2)
    {
        return !(A1 != A2);
    }

    public static bool operator !=(HardwareData A1, HardwareData A2)
    {
        if (Math.Abs(A1.PotiOne - A2.PotiOne) > 3)
            return true;
        if (Math.Abs(A1.PotiTwo - A2.PotiTwo) > 3)
            return true;
        return false;
    }
}