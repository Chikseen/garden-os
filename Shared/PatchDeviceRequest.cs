using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class PatchDeviceRequest
	{
		[JsonInclude]
		[JsonPropertyName("device_id")]
		public string DeviceId = string.Empty;

		[JsonInclude]
		[JsonPropertyName("name")]
		public string Name = string.Empty;

		[JsonInclude]
		[JsonPropertyName("display_id")]
		public string DisplayID = string.Empty;

		[JsonInclude]
		[JsonPropertyName("upper_limit")]
		public int UpperLimit = 100;

		[JsonInclude]
		[JsonPropertyName("lower_limit")]
		public int LowerLimit = 100;

		[JsonInclude]
		[JsonPropertyName("sort_order")]
		public int SortOrder = 100;

		[JsonInclude]
		[JsonPropertyName("group_id")]
		public string GroupId = string.Empty;

		[JsonInclude]
		[JsonPropertyName("unit")]
		public string Unit = string.Empty;
	}
}