using System.Text.Json.Serialization;

namespace shared_data.Models
{
	public class ControllerResponseModel
	{
		public List<Controller> Controllers { get; set; } = new();

		public ControllerResponseModel(List<Dictionary<string, string>> data)
		{
			foreach (Dictionary<string, string> controller in data)
			{
				Controllers.Add(new Controller(controller));
			}
		}

		[JsonConstructor]
		public ControllerResponseModel() { }
	}

	public class Controller
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string DisplayId { get; set; }
		public string GardenId { get; set; }

		public Controller(Dictionary<string, string> data)
		{
			Id = DeviceStatic.GetString(data, DeviceStatic.ID);
			Name = DeviceStatic.GetString(data, DeviceStatic.Name);
			DisplayId = DeviceStatic.GetString(data, DeviceStatic.DisplayId);
			GardenId = DeviceStatic.GetString(data, DeviceStatic.GardenID);
		}
	};
}

