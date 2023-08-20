using System.Device.Gpio;
using System.Text.Json;
using shared_data.Models;

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
                DeviceId = device.ID,
                Value = value
            };

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