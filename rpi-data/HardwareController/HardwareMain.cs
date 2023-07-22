using System.Text.Json;
using dotenv.net;

namespace MainService.Hardware
{
    public delegate void ValueChange();  // delegate
    public class MainHardware
    {
        public static String RpiId = "";
        public static String RpiApiKey = "";
        public static string _localIPAdress = "NO IP";
        public static RPIData? rpiData;
        public static RPIDevices __data = new(); // need this hack due recurisons of static propertys on call ->it is kinda proxy for _data
        public static RPIDevices _data
        {
            get => __data;
            set
            {
                __data = value;
                OnProcessCompleted(); // This is a supercool event listner only sends an event to ALL subs if this value is actually changed
            }
        }

        // Starts the main loop for getting Hardwaredata
        public static void Init()
        {
            DotEnv.Load();
            RpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
            RpiApiKey = Environment.GetEnvironmentVariable("API_KEY")!;

            try
            {
                Preperation prep = new(RpiApiKey);

                rpiData = prep.SetRPI(RpiId);
                _data = prep.SetDevices(RpiId);

                MainLoop.Start();
            }
            catch (System.Exception e)
            {
                DeveiceStatus status = new()
                {
                    RpiId = MainHardware.RpiId,
                    Message = "RPI Exeption: MainLoop was exited: " + e,
                    Status = "error",
                    TriggerdBy = "hub"
                };

                ApiService api = new();
                api.Post($"/devices/status", JsonSerializer.Serialize(status));
                SystemService.Reboot();
                throw;
            }
            SystemService.Reboot();
        }

        protected static void OnProcessCompleted()
        {
            //if ProcessCompleted is not null then call delegate -> is null if "no one" gives a sub
            ProcessCompleted?.Invoke();
        }

        public static event ValueChange? ProcessCompleted; // event
    }
}
