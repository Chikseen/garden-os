using System.Text.Json;
using dotenv.net;

namespace MainService.Hardware
{
    public class MainHardware
    {
        public static string _RpiId = "";
        public static string _RpiApiKey = "";
        public static string _LocalIPAdress = "NO IP";
        public static RPIData? _RpiData;
        public static RPIDevices _Devices = new();

        // Starts the main loop for getting Hardwaredata
        public static void Init()
        {
            DotEnv.Load();
            _RpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
            _RpiApiKey = Environment.GetEnvironmentVariable("API_KEY")!;

            try
            {
                Preperation prep = new(_RpiApiKey);

                _RpiData = prep.SetRPI(_RpiId);
                _Devices = prep.SetDevices(_RpiId);

                MainLoop.Start();
            }
            catch (Exception e)
            {
                DeveiceStatus status = new()
                {
                    RpiId = _RpiId,
                    Message = "RPI Exeption: MainLoop was exited: " + e,
                    Status = "error",
                    TriggerdBy = "hub"
                };

                ApiService api = new();
                api.Post($"/devices/status", JsonSerializer.Serialize(status));
                Thread.Sleep(1000 * 60);
                SystemService.Reboot();
            }
        }
    }
}
