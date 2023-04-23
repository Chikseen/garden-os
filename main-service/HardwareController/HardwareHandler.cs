using System.Device.Gpio;
using System.Device.I2c;
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

        public static void Loop()
        {
            if (_i2c_ADC_Device == null || _i2c_LCD_Device == null || _LCD == null)
                return;

            byte[] readBuffer = new byte[1];
            // Start ADC conversion on channel 
            _i2c_ADC_Device.WriteByte(0x8c); // Read Channel 0 -> Check ./ADC7830 Sheet and convert Hex To Binary
            _i2c_ADC_Device.Read(readBuffer); // Read the conversion result
            int rawValue1 = readBuffer[0]; // Set rawValue from ReadBuffer


            // Reset readBuffer and read Channel 1
            readBuffer = new byte[1];
            // Start ADC conversion on channel 
            _i2c_ADC_Device.WriteByte(0xcc); // Read Channel 0 -> Check ./ADC7830 Sheet and convert Hex To Binary
            _i2c_ADC_Device.Read(readBuffer); // Read the conversion result
            int rawValue2 = readBuffer[0]; // Set rawValue from ReadBuffer

            HardwareData data = new();

            data.PotiOne = (uint)Math.Round(((decimal)rawValue1 / 255 * 100));
            data.PotiTwo = (uint)Math.Round(((decimal)rawValue2 / 255 * 100));

            _LCD.SetCursorPosition(8, 1);
            _LCD.Write($"VII: {data.PotiOne.ToString("000")}");
            _LCD.SetCursorPosition(0, 1);
            _LCD.Write($"VI: {data.PotiTwo.ToString("000")}");

            if (data != MainHardware._data)
            {
                MainHardware._data = data;
            }

            _LCD.SetCursorPosition(0, 0);
            _LCD.Write(MainHardware._localIPAdress);
        }
    }
}