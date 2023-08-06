using System.Diagnostics;

namespace MainService.Hardware
{
	public static class Metrics
	{
		private static int _MetricsCounter = 99;
		private static readonly int _MetricsLimit = 10;

		public static void ReadAndSetMetrics(RPIDevice device)
		{
			Console.WriteLine("start met", device.ID);
			Console.WriteLine(device.ID);
			if (_MetricsCounter > _MetricsLimit)
			{
				Console.WriteLine("in met");
				var output = "";

				ProcessStartInfo info = new("free -m")
				{
					FileName = "/bin/bash",
					Arguments = "-c \"free -m\"",
					RedirectStandardOutput = true
				};

				using (var process = Process.Start(info))
				{
					output = process!.StandardOutput.ReadToEnd();
				}

				var lines = output.Split("\n");
				var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

				Console.WriteLine("val " + float.Parse(memory[2]));

				HardwareEvents.SaveDataToDatabase(device, float.Parse(memory[2]));
				_MetricsCounter = 0;
			}
			_MetricsCounter++;
		}
	}
}