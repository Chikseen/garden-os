using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;

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

        protected static void OnProcessCompleted()
        {
            //if ProcessCompleted is not null then call delegate -> is null if "no one" gives a sub
            ProcessCompleted?.Invoke();
        }

        public static event ValueChange? ProcessCompleted; // event

        public static void DataHandler(I2cConnectionSettings i2cSettings, I2cDevice i2cDevice, Lcd2004 lcd)
        {
            // Read analog data from channel 0 of the ADC
            byte[] readBuffer = new byte[2];

            i2cDevice.WriteByte(0x8C); // Start ADC conversion on channel 0

            i2cDevice.Read(readBuffer); // Read the conversion result
            int rawValue = (readBuffer[0] << 8) + readBuffer[1]; // Combine the two bytes
            double voltage = (rawValue / 32767) * 3.3; // Convert to voltage (assuming Vref = 5V)

            // Print the result on the console
            Console.WriteLine($"Analog value: {voltage:F2}V");
            _testValue = (float)voltage;
	    
	    // Write Value to LCD
    	    lcd.SetCursorPosition(0,0);
    	    lcd.Write(_testValue.ToString());

            Thread.Sleep(100);
        }

        // you cannot use the datahandler on windos thats why i try to fake it here
        public static void DataFaker()
        {
            Random rnd = new Random();
            float num = ((float)rnd.NextDouble());

            if (num > 0.3f) // Just simulating a non constant value change
            {
                _testValue = 5 * num;
            }

            Thread.Sleep(500);
        }
    }
}

/**/
