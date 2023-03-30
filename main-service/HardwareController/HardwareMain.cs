using System.Device.Gpio;
using System.Device.I2c;

namespace main_service.Hardware
{
    public delegate void ValueChange();  // delegate
    public class Hardware
    {
        private static float __testValue; // need this hack due recurisons of static propertys on call ->it is kinda proxy for _testValue
        public static float _testValue
        {
            get => __testValue;
            set
            {
                __testValue = value;
                OnProcessCompleted(); // This is a supercool event listner only sends an event to ALL subs if this value is actually changed
            }
        }

        // Starts the main loop for getting Hardwaredata
        public static void Init()
        {
            Task.Run(() => mainLoop());
        }

        private static Task mainLoop()
        {
            while (true)
            {
                Random rnd = new Random();
                float num = ((float)rnd.NextDouble());

                if (num > 0.3f) // Just simulating a non constant value change
                {
                    _testValue = 5 * num;
                }

                // Just for some cool randomnis
                Random sleepRnd = new Random();
                Thread.Sleep(sleepRnd.Next(1000, 3000));

               /* try
                {

                    var i2cSetting = new I2cConnectionSettings(0, 0x48)
                    {
                    };
                    var i = I2cDevice.Create(i2cSetting);
                    byte[] buffer = new byte[3];
                    i.Read(buffer);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }*/
            }
        }

        protected static void OnProcessCompleted()
        {
            //if ProcessCompleted is not null then call delegate -> is null if "no one" gives a sub
            ProcessCompleted?.Invoke();
        }

        public static event ValueChange? ProcessCompleted; // event
    }
}
