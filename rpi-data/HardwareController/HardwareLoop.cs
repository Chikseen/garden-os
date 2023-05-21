using System.Device.Gpio;
using System.Device.I2c;
using System.Net.Http.Headers;
using System.Text.Json;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;

namespace MainService.Hardware
{
    public static class MainLoop
    {
        public static bool _recivedStop = false;
        public static int _loopDelay = 100;

        private static I2cDevice? _i2c_ADC_Device;
        private static I2cDevice? _i2c_LCD_Device;
        private static Lcd2004? _LCD;


        public static void Start()
        {
            // Make a plan how to fake data
            Console.WriteLine("loopcheck");
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

                _LCD.Clear();
                _LCD.SetCursorPosition(0, 0);
                _LCD.Write(MainHardware.rpiData?.GardenName!);

                while (true && !_recivedStop)
                {
                    try
                    {
                        Loop();
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    Thread.Sleep(_loopDelay);

                    if (_recivedStop)
                    {
                        _LCD.Dispose();
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void Loop()
        {
            if (_i2c_LCD_Device == null || _LCD == null)
                return;

            ReadAndSetValues();

        }

        private static void ReadAndSetValues()
        {
            Boolean triggerUpdate = false;
            RPIDevices data = MainHardware._data;

            foreach (RPIDevice device in data.Devices)
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

                        Console.WriteLine($"DEVICE: {device.DeviceName}");
                        Console.WriteLine($"VALUE: {device.Value}");

                        if (Math.Abs(device.LastSavedValue - value) > 1)
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

        private static void SaveDataToDatabase(RPIDevice device, int value)
        {
            Console.WriteLine("Before Save");
            RPIDevice? originalDevice = MainHardware._data.Devices.FirstOrDefault(d => d.ID == device.ID);
            if (originalDevice == null)
                return;

            DateTime lastEntry = originalDevice.LastEntry;
            TimeSpan interval = originalDevice.DataUpdateInterval;

            if (lastEntry < DateTime.Now - interval)
            {
                Console.WriteLine("DATA IS SAVED");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainHardware.RpiApiKey);

                Console.WriteLine("111");
                SaveDataRequest data = new();
                data.Device_ID = device.ID;
                data.Value = value;
                Console.WriteLine("222");

                string jsonString = JsonSerializer.Serialize(data);
                var content = new StringContent(
                    jsonString,
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );

                Console.WriteLine("aaa"); 
                Console.WriteLine(jsonString); 
                Console.WriteLine("aaa"); 
                Console.WriteLine(content); 
                Console.WriteLine("aaa"); 


                var responseString = client.PostAsync($"https://gardenapi.drunc.net/devices/{MainHardware.RpiId}/save", content).Result;
                Console.WriteLine(responseString);

                originalDevice.LastEntry = DateTime.Now;
                originalDevice.LastSavedValue = value;
            }
        }
    }
}