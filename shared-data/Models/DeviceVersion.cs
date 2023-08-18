using System.Text.Json.Serialization;

public class DeviceVersion
{
	[JsonInclude]
	[JsonPropertyName("version")]
	public string Version = "-1";
}