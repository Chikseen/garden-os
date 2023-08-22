using System.Text.Json.Serialization;

namespace shared_data.Models
{
	public class GardenInfo
	{
		[JsonInclude]
		[JsonPropertyName("garden_info")]
		public readonly List<UserGardenData> GardenInfoList = new();

		public GardenInfo(ResponseDevices detailed)
		{
			Console.WriteLine(detailed);
			List<IGrouping<string, ReponseDevice>> grouped = detailed.Devices.GroupBy(d => d.DeviceID).ToList();

			foreach (IGrouping<string, ReponseDevice> gDeviceData in grouped)
			{
				switch (gDeviceData.ElementAt(0).SpecialId)
				{
					case "temperatur":
						{
							float maxTemp = gDeviceData.Max(d => d.CorrectedValue);
							if (maxTemp > 27.0f)
								GardenInfoList.Add(new(
									Titel: "Warmer Tag",
									Text: "Heute war ein Warmer Tag, du solltest deine Pflanzen überprüfen"
								));
						}
						break;
					case "brightness":
						{
							float avgBrightness = gDeviceData.Average(d => d.CorrectedValue);
							if (avgBrightness > 5.0f)
								GardenInfoList.Add(new(
									Titel: "Hohe Sonneneinstarhlung",
									Text: "Denk an deine Sonnencreme, Außerdem solltest du deine Pflanzen und Wiesen nicht von oben bewässern"
								));
						}
						break;
					case "soil_moisture":
						{
							ReponseDevice latestSoilMoisture = gDeviceData.OrderByDescending(d => d.Date).ToList().FirstOrDefault()!;
							if (latestSoilMoisture.CorrectedValue < 60.0f)
								GardenInfoList.Add(new(
									Titel: $"{latestSoilMoisture.Name} ist etwas trocken",
									Text: "Du solltest deine Beete immer ausreichend bewässern"
								));
							if (latestSoilMoisture.CorrectedValue > 110.0f)
								GardenInfoList.Add(new(
									Titel: $"{latestSoilMoisture.Name} ist etwas zu nass",
									Text: "Zu viel Wasser tut deinen Beeten nicht gut, du kannst das Gießen auslassen"
								));
						}
						break;
				}
			}
		}
	}


	public record UserGardenData(string Titel, string Text);
}