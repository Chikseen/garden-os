using System.Device.Gpio;

namespace MainService.Hardware
{
    public static class WebInput
    {
        // Mabe Using Events here
        public static bool _ledOn = false;
        public static void toggleLed()
        {
            var controller = new GpioController();
            controller.OpenPin(18, PinMode.Output);
            controller.Write(18, ((_ledOn) ? PinValue.High : PinValue.Low));
            _ledOn = !_ledOn;
            controller.ClosePin(18);
        }
    }
}