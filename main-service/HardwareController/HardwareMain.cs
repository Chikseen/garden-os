using System.Collections;
using System.Device.Gpio;
using System.Device.I2c;
using System.Net;
using System.Net.Sockets;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;

namespace MainService.Hardware
{
    public delegate void ValueChange();  // delegate
    public class MainHardware
    {
        private static string _localIPAdress = "NO IP";
        private static GpioController? _controller { get; set; }
        private static bool ledOn { get; set; }

        private static HardwareData __data = new(); // need this hack due recurisons of static propertys on call ->it is kinda proxy for _testValue
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
            Task.Run(() => mainLoop());
        }

        private static Task mainLoop()
        {
            getLastEntryforMachines();
            SetIpAdress();

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // SetUp Value Read
                I2cConnectionSettings i2cSettings = new I2cConnectionSettings(1, 0x4B);
                I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

                // SetUp LCD Write
                using I2cDevice i3c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
                using var driver = new Pcf8574(i3c);
                using var lcd = new Lcd2004(registerSelectPin: 0,
                                enablePin: 2,
                                dataPins: new int[] { 4, 5, 6, 7 },
                                backlightPin: 3,
                                backlightBrightness: 0.1f,
                                readWritePin: 1,
                                controller: new GpioController(PinNumberingScheme.Logical, driver));

                while (true)
                    DataHandler(i2cSettings, i2cDevice, lcd);
            }
            else
                while (true)
                    DataFaker();
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

        public static void DataHandler(I2cConnectionSettings i2cSettings, I2cDevice i2cDevice, Lcd2004 lcd)
        {
            byte[] readBuffer = new byte[1];
            // Start ADC conversion on channel 
            i2cDevice.WriteByte(0x8c); // Read Channel 0 -> Check ./ADC7830 Sheet and convert Hex To Binary
            i2cDevice.Read(readBuffer); // Read the conversion result
            int rawValue1 = readBuffer[0]; // Set rawValue from ReadBuffer
            lcd.SetCursorPosition(0, 1);
            lcd.Write($"VI: {rawValue1.ToString("000")}");

            // Reset readBuffer and read Channel 1
            readBuffer = new byte[1];
            // Start ADC conversion on channel 
            i2cDevice.WriteByte(0xcc); // Read Channel 0 -> Check ./ADC7830 Sheet and convert Hex To Binary
            i2cDevice.Read(readBuffer); // Read the conversion result
            int rawValue2 = readBuffer[0]; // Set rawValue from ReadBuffer
            lcd.SetCursorPosition(8, 1);
            lcd.Write($"VII: {rawValue2.ToString("000")}");

            HardwareData data = new();

            data.PotiOne = (uint)rawValue1;
            data.PotiTwo = (uint)rawValue2;

            if (data != _data)
            {
                _data = data;
            }

            lcd.SetCursorPosition(0, 0);
            lcd.Write(_localIPAdress);


            Thread.Sleep(25);
        }

        // you cannot use the datahandler on windos thats why i try to fake it here
        public static void DataFaker()
        {
            Random rnd = new Random();
            float num = ((float)rnd.NextDouble());

            if (num > 0.3f) // Just simulating a non constant value change
            {
                _data.PotiOne = (uint)(5 * num + 20);
            }

            Thread.Sleep(500);
        }

        private static DevicesData getLastEntryforMachines()
        {
            // GET ALL DEVICE DATA HERE
            // ToDo Finsh Model
            return new DevicesData();
        }

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

        public static void toggleLed()
        {
            var controller = new GpioController();
            controller.OpenPin(18, PinMode.Output);
            controller.Write(18, ((ledOn) ? PinValue.High : PinValue.Low));
            ledOn = !ledOn;
            controller.ClosePin(18);
        }
    }
}

/**/
