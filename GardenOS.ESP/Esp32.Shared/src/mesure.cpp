#include <mesure.h>
#include <secrets.h>

static const int analogInputPin = 34;
static const int enviromentJumper = 23;

int16_t batteryValue;
int16_t sensorValue;

Adafruit_ADS1115 adc;

void set_up()
{
	pinMode(analogInputPin, ANALOG);
	pinMode(enviromentJumper, INPUT_PULLUP);
	adc.setGain(GAIN_ONE);

	if (!adc.begin())
	{
		Serial.println("Failed to initialize ADS.");
	}
}

void mesureAndSend()
{
	float multiplier = 0.1875F;

	batteryValue = adc.readADC_Differential_2_3();
	delay(100);
	sensorValue = adc.readADC_Differential_0_1();

	Serial.print("batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("sensorValue: ");
	Serial.println(sensorValue);

	upload::send(batteryValue, sensorValue);
}
