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
            DeviceInit();

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


        public static void Loop()
        {
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

                        float value = ((float)rawValue / 255.0f * 100.0f);
                        Console.WriteLine(value);


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

        private static void SaveDataToDatabase(RPIDevice device, float value)
        {
            RPIDevice? originalDevice = MainHardware._data.Devices.FirstOrDefault(d => d.ID == device.ID);
            if (originalDevice == null)
                return;

            DateTime lastEntry = originalDevice.LastEntry;
            TimeSpan interval = originalDevice.DataUpdateInterval;

            if (lastEntry < DateTime.Now - interval)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainHardware.RpiApiKey);

                SaveDataRequest data = new();
                data.Device_ID = device.ID;
                data.Value = value;

                string jsonString = JsonSerializer.Serialize(data);
                var content = new StringContent(
                    jsonString,
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );

                Console.WriteLine("Save And Send Data");
                Console.WriteLine(content.ToString());
                var responseString = client.PostAsync($"https://gardenapi.drunc.net/devices/{MainHardware.RpiId}/save", content).Result;
                originalDevice.LastEntry = DateTime.Now;
                originalDevice.LastSavedValue = value;
            }
        }

        private static void DeviceInit()
        {

            // I 0
            Console.WriteLine("Check Hardware Data");
            try
            {
                _i2c_ADC_Device = I2cDevice.Create(new I2cConnectionSettings(1, 0x4B));
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error while ADC Setup");
                Console.WriteLine(e);
            }

            /* // LCD
             try
             {
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
             }
             catch (System.Exception e)
             {
                 Console.WriteLine("Error while LCD Setup");
                 Console.WriteLine(e);
             }*/
        }
    }
}