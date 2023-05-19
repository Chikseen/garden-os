using System.Device.Gpio;
using System.Device.I2c;
using System.Runtime.InteropServices;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;

namespace MainService.Hardware
{
    public static class MainLoop
    {
        public static bool _recivedStop = false;
        public static int _loopDelay = 25;

        private static I2cDevice? _i2c_ADC_Device;
        private static I2cDevice? _i2c_LCD_Device;
        private static Lcd2004? _LCD;


        public static void Start()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Make a plan how to fake data
                try
                {
                    _i2c_ADC_Device = I2cDevice.Create(new I2cConnectionSettings(1, 0x4B));
                    _i2c_LCD_Device = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));

                    _LCD = new Lcd2004(registerSelectPin: 0,
                                    enablePin: 2,
                                    dataPins: new int[] { 4, 5, 6, 7 },
                                    backlightPin: 3,
                                    backlightBrightness: 0.1f,
                                    readWritePin: 1,
                                    controller: new GpioController(PinNumberingScheme.Logical, new Pcf8574(_i2c_LCD_Device)));

                    while (true && !_recivedStop)
                    {
                        Loop();
                        Thread.Sleep(_loopDelay);

                        if (_recivedStop)
                        {
                            _LCD.Dispose();
                        }
                    }
                }
                catch (System.Exception)
                { }
            }
        }

        public static void Loop()
        {
            if (_i2c_LCD_Device == null || _LCD == null)
                return;

            ReadAndSetValues();
            /*                        _LCD.SetCursorPosition(8, 1);
                        _LCD.Write($"VII: {data.PotiOne.ToString("000")}");
                        _LCD.SetCursorPosition(0, 1);
                        _LCD.Write($"VI: {data.PotiTwo.ToString("000")}");
*/

            _LCD.SetCursorPosition(0, 0);
            _LCD.Write(MainHardware._localIPAdress);
        }

        private static void ReadAndSetValues()
        {
            Boolean triggerUpdate = false;
            DevicesData data = MainHardware._data;

            foreach (Device device in data.Devices)
            {
                if (device.DeviceTyp == DeviceStatic.ADC7080)
                {
                    if (_i2c_ADC_Device != null)
                    {
                        byte[] readBuffer = new byte[1];
                        _i2c_ADC_Device.WriteByte(device.Address); // Read Channel 0 -> Check ./ADC7830 Sheet and convert Hex To Binary
                        _i2c_ADC_Device.Read(readBuffer); // Read the conversion result
                        int rawValue = readBuffer[0]; // Set rawValue from ReadBuffer
                        int value = (int)Math.Round(((decimal)rawValue / 255 * 100));

                        if (Math.Abs(device.Value - value) > 1)
                        {
                            triggerUpdate = true;
                            SaveDataToDatabase(device, value);
                        }

                        device.Value = value;
                    }
                }
            }

            // Trigger only on change to spare performance
            if (triggerUpdate)
            {
                MainHardware._data = data;
            }
        }

        private static void SaveDataToDatabase(Device device, int value)
        {

            Device? originalDevice = MainHardware._data.Devices.FirstOrDefault(d => d.Id == device.Id);
            if (originalDevice == null)
                return;

            DateTime lastEntry = originalDevice.LastEntry;
            TimeSpan interval = originalDevice.DataUpdateInterval;

            if (lastEntry < DateTime.Now - interval)
            {
                /*MainDB.query(@$"
                    INSERT INTO datalog (value, deviceid)
                    VALUES ({value}, '{device.Id}')");
                originalDevice.LastEntry = DateTime.Now;*/
            }
        }
    }
}