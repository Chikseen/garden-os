namespace Shared.Services
{
    public static class TemperatureCalculationService
    {
        private const double A = 1.009249522e-03;
        private const double B = 2.378405444e-04;
        private const double C = 2.019202697e-07;

        private static float ResistanceToTemperature(float resistance)
        {
            // Apply Steinhart-Hart equation
            double logR = Math.Log(resistance);
            double invT = A + B * logR + C * Math.Pow(logR, 3);
            double temperatureKelvin = 1.0 / invT;

            // Convert Kelvin to Celsius
            double temperatureCelsius = temperatureKelvin - 273.15d;

            return (float)temperatureCelsius;
        }

        public static float CalculateTemperatureCelsius(float rawValue)
        {
            float balanceResitor = 9800f;
            float maxVoltage = 3.4f;
            float currentVoltage = rawValue * 0.0001875f;

            float thermistorResitance = balanceResitor * currentVoltage / (maxVoltage - currentVoltage);

            float temperatureCelsius = ResistanceToTemperature(thermistorResitance);

            return temperatureCelsius;
        }
    }
}

