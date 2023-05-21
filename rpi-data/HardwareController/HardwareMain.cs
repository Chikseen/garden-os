using RPI.Connection;
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
        public static Connection? _connection;


        // Starts the main loop for getting Hardwaredata
        public static void Init(Connection connection)
        {
            DotEnv.Load();
            RpiId = Environment.GetEnvironmentVariable("RPI_ID")!;
            RpiApiKey = Environment.GetEnvironmentVariable("API_KEY")!;

            // Set up Connection
            _connection = connection;

            Preperation prep = new(RpiApiKey);

            rpiData = prep.SetRPI(RpiId);
            _data = prep.SetDevices(RpiId);

            MainLoop.Start();
        }

        protected static void OnProcessCompleted()
        {
            //if ProcessCompleted is not null then call delegate -> is null if "no one" gives a sub
            ProcessCompleted?.Invoke();
        }

        public static event ValueChange? ProcessCompleted; // event
    }
}
