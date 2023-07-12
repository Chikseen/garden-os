using System.Device.Gpio;
using System.Device.I2c;
using System.Net.Http.Headers;
using System.Text.Json;
using Iot.Device.Ads1115;
using Iot.Device.CharacterLcd;
using Iot.Device.DHTxx;
using Iot.Device.Pcx857x;
using UnitsNet;

namespace MainService.Hardware
{
    public static class MainLoop
    {
        public static bool _recivedStop = false;
        public static int _loopDelay = 250;
        private static I2cDevice? _i2c_LCD_Device;
        private static I2cDevice? _ADC;
        private static Lcd2004? _LCD;
        private static Dictionary<String, List<float>> _Filter = new();

        public static void Start()
        {
            DeviceInit();

            while (true)
            {
                try
                {
                    Loop();
                }
                catch (System.Exception)
                {

                    throw;
                }
                Thread.Sleep(_loopDelay);
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
                Thread.Sleep(_loopDelay);
                switch (device.DeviceTyp)
                {
                    case "ADC":
                        {
                            Ads1115? _ADS1115;
                            short raw = 0;
                            if (_ADC is not null)
                                switch (device.Address)
                                {
                                    case 0:
                                        _ADS1115 = new Ads1115(_ADC, InputMultiplexer.AIN0, MeasuringRange.FS4096);
                                        raw = _ADS1115.ReadRaw();
                                        break;
                                    case 1:
                                        _ADS1115 = new Ads1115(_ADC, InputMultiplexer.AIN1, MeasuringRange.FS4096);
                                        raw = _ADS1115.ReadRaw();
                                        break;
                                    case 2:
                                        _ADS1115 = new Ads1115(_ADC, InputMultiplexer.AIN2, MeasuringRange.FS4096);
                                        raw = _ADS1115.ReadRaw();
                                        break;
                                    case 3:
                                        _ADS1115 = new Ads1115(_ADC, InputMultiplexer.AIN3, MeasuringRange.FS4096);
                                        raw = _ADS1115.ReadRaw();
                                        break;
                                }

                            float value = ((float)raw / 26550.0f * 100.0f);

                            _Filter[$"adc1{device.Address}"].Add(value);
                            if (_Filter[$"adc1{device.Address}"].Count > 50)
                            {
                                _Filter[$"adc1{device.Address}"].RemoveAt(0);

                                float avgValue = _Filter[$"adc1{device.Address}"].Sum() / 50.0f;
                                if (Math.Abs(device.LastSavedValue - avgValue) > 0.25f)
                                {
                                    triggerUpdate = true;
                                    SaveDataToDatabase(device, avgValue);
                                }
                            }
                            else
                                Console.WriteLine("Filling");
                        }
                        break;
                    case "DHT11":
                        {
                            Dht11 dht11 = new Dht11(17);

                            Boolean isTempValid = dht11.TryReadHumidity(out RelativeHumidity humidity);
                            Boolean isHumidValid = dht11.TryReadTemperature(out Temperature temperature);

                            if (isTempValid && isHumidValid)
                                if (humidity.Percent > 0 && temperature.DegreesCelsius > -50)
                                {
                                    if (device.ID == "3032fc92-1bee-11ee-be56-0242ac120002")
                                        SaveDataToDatabase(device, (float)humidity.Percent);
                                    if (device.ID == "de9cca87-eff5-49a5-bcc2-1a5d13d366cc")
                                        SaveDataToDatabase(device, (float)temperature.DegreesCelsius);
                                }

                        }
                        break;
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
            Console.WriteLine(content.ReadAsStringAsync());
            var responseString = client.PostAsync($"https://gardenapi.drunc.net/devices/{MainHardware.RpiId}/save", content).Result;
            originalDevice.LastEntry = DateTime.Now;
            originalDevice.LastSavedValue = value;
        }

        private static void DeviceInit()
        {

            // ADC 1 (ADS1115)
            Console.WriteLine("Check Hardware Data");
            Console.WriteLine("Setup ADS_1 and AIN");
            try
            {
                I2cConnectionSettings settings = new I2cConnectionSettings(1, (int)I2cAddress.GND);
                _ADC = I2cDevice.Create(settings);
                _Filter.Add($"adc10", new List<float>());
                _Filter.Add($"adc11", new List<float>());
                _Filter.Add($"adc12", new List<float>());
                _Filter.Add($"adc13", new List<float>());
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.WriteLine("en");


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