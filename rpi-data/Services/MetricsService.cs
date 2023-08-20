using System.Diagnostics;
using shared_data.Models;

namespace MainService.Hardware
{
    public static class Metrics
	{
		private static int _MetricsCounter = 99;
		private static readonly int _MetricsLimit = 10;

		public static void ReadAndSetMetrics(RPIDevice device)
		{
			if (_MetricsCounter > _MetricsLimit)
			{
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

				HardwareEvents.SaveDataToDatabase(device, float.Parse(memory[2]));
				_MetricsCounter = 0;
			}
			_MetricsCounter++;
		}
	}
}