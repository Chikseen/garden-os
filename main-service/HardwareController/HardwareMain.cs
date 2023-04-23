using System.Net;

namespace MainService.Hardware
{
    public delegate void ValueChange();  // delegate
    public class MainHardware
    {
        public static string _localIPAdress = "NO IP";

        public static HardwareData __data = new(); // need this hack due recurisons of static propertys on call ->it is kinda proxy for _testValue
        public static HardwareData _data
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
            Task.Run(() => MainLoop.Start());
            SetIpAdress();
        }

        public static void ManuelSend()
        {
            OnProcessCompleted();
        }

        protected static void OnProcessCompleted()
        {
            //if ProcessCompleted is not null then call delegate -> is null if "no one" gives a sub
            ProcessCompleted?.Invoke();
        }

        public static event ValueChange? ProcessCompleted; // event

        public static void SetIpAdress()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            Console.WriteLine(hostName);
            // Get the IP
            string myIP = Dns.GetHostEntry(hostName).AddressList.Where(e => e.ToString().Contains("192.")).ToList()[0].ToString();


            if (!String.IsNullOrEmpty(myIP))
                _localIPAdress = myIP;

            Console.WriteLine($"Ip Adress: {_localIPAdress}");
        }
    }
}

/**/
