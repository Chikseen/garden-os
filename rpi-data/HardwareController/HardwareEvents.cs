using System.Device.Gpio;
using System.Text.Json;

namespace MainService.Hardware
{
    public static class HardwareEvents
    {
        // Mabe Using Events here
        public static bool _ledOn = false;
        public static void ToggleLed()
        {
            var controller = new GpioController();
            controller.OpenPin(18, PinMode.Output);
            controller.Write(18, ((_ledOn) ? PinValue.High : PinValue.Low));
            _ledOn = !_ledOn;
            controller.ClosePin(18);
        }

        public static void SaveDataToDatabase(RPIDevice device, float value)
        {
            RPIDevice? originalDevice = MainHardware._Devices.Devices.FirstOrDefault(d => d.ID == device.ID);
            if (originalDevice == null)
                return;

            SaveDataRequest data = new()
            {
                Device_ID = device.ID,
                Value = value
            };

            Console.WriteLine("Save And Send Data");

            try
            {
                ApiService api = new();
                api.Post($"/devices/{MainHardware._RpiId}/save", JsonSerializer.Serialize(data));
                originalDevice.LastEntry = DateTime.Now;
                originalDevice.LastSavedValue = value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

}