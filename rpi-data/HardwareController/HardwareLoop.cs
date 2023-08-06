using System.Device.I2c;
using System.Text.Json;
using Iot.Device.Ads1115;
using Iot.Device.DHTxx;
using UnitsNet;

namespace MainService.Hardware
{
    public static class MainLoop
    {
        public static bool _recivedStop = false;
        public static int _loopDelay = 50;
        private static I2cDevice? _ADC;
        private static readonly Dictionary<string, List<float>> _Filter = new();

        public static void Start()
        {
            DeviceInit();

            DeveiceStatus status = new()
            {
                RpiId = MainHardware._RpiId,
                TriggerdBy = "hub",
                Status = "ok",
            };

            ApiService api = new();
            api.Post($"/devices/status", JsonSerializer.Serialize(status));

            while (true)
            {
                Loop();
                Thread.Sleep(_loopDelay);
            }
        }

        public static void Loop()
        {
            ReadAndSetValues();
        }

        private static void ReadAndSetValues()
        {
            RPIDevices data = MainHardware._Devices;

            foreach (RPIDevice device in data.Devices)
            {
                Thread.Sleep(_loopDelay);
                switch (device.DeviceTyp)
                {
                    case "ADC":
                        ReadADC(device);
                        break;
                    case "DHT11":
                        ReadDHT11(device);
                        break;
                    case "DEBUG":
                        Metrics.ReadAndSetMetrics(device);
                        break;
                }
            }
        }

        private static void DeviceInit()
        {
            // ADC 1 (ADS1115)
            Console.WriteLine("Check Hardware Data");
            Console.WriteLine("Setup ADS_1 and AIN");
            try
            {
                I2cConnectionSettings settings = new(1, (int)I2cAddress.GND);
                _ADC = I2cDevice.Create(settings);
                _Filter.Add($"adc10", new List<float>());
                _Filter.Add($"adc11", new List<float>());
                _Filter.Add($"adc12", new List<float>());
                _Filter.Add($"adc13", new List<float>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

        private static void ReadADC(RPIDevice device)
        {
            if (_ADC is not null)
            {
                Ads1115 _ADS1115 = new(_ADC, GetADCInput(device), MeasuringRange.FS4096);
                short raw = _ADS1115.ReadRaw();

                float value = raw / 26550.0f * 100.0f;

                _Filter[$"adc1{device.Address}"].Add(value);
                if (_Filter[$"adc1{device.Address}"].Count > 50)
                {
                    _Filter[$"adc1{device.Address}"].RemoveAt(0);

                    float avgValue = _Filter[$"adc1{device.Address}"].Sum() / 50.0f;
                    if (Math.Abs(device.LastSavedValue - avgValue) > 0.1f)
                    {
                        HardwareEvents.SaveDataToDatabase(device, avgValue);
                    }
                }
                else
                    Console.WriteLine("Filling");
            }
        }

        private static void ReadDHT11(RPIDevice device)
        {
            Dht11 dht11 = new(17);

            bool isTempValid = dht11.TryReadHumidity(out RelativeHumidity humidity);
            bool isHumidValid = dht11.TryReadTemperature(out Temperature temperature);

            if (isTempValid && isHumidValid)
                if (humidity.Percent > 0 && temperature.DegreesCelsius > -50)
                {
                    if (device.ID == "3032fc92-1bee-11ee-be56-0242ac120002")
                        HardwareEvents.SaveDataToDatabase(device, (float)humidity.Percent);
                    if (device.ID == "de9cca87-eff5-49a5-bcc2-1a5d13d366cc")
                        HardwareEvents.SaveDataToDatabase(device, (float)temperature.DegreesCelsius);
                }
        }

        private static InputMultiplexer GetADCInput(RPIDevice device)
        {
            return device.Address switch
            {
                0 => InputMultiplexer.AIN0,
                1 => InputMultiplexer.AIN1,
                2 => InputMultiplexer.AIN2,
                _ => InputMultiplexer.AIN3,
            };
        }
    }
}