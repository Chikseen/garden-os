using System.Text.Json.Serialization;

public class DeviceVersion
{
	[JsonInclude]
	[JsonPropertyName("build")]
	public string Build = "-1";
}