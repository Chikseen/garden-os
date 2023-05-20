using RPI.Connection;
using dotenv.net;

namespace MainService.Hardware
{
    public delegate void ValueChange();  // delegate
    public class MainHardware
    {
        private static String RpiId = "";
        private static String RpiApiKey = "";
        public static string _localIPAdress = "NO IP";
        public static RPIdata? rpiData;
        public static DevicesData __data = new(); // need this hack due recurisons of static propertys on call ->it is kinda proxy for _data
        public static DevicesData _data
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

            // SetUpRPI
            rpiData = Prep.SetRPI(RpiId);
            Console.WriteLine("RpiId: " + RpiId);
            // Fetch Device Data
            Prep.SetDevices();

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
