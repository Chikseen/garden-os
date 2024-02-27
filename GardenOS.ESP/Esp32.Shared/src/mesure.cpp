#include <mesure.h>
#include <secrets.h>

static const int multisamplingLength = 10;

static const int analogInputPin = 34;
static const int sensorOutPut = 17;
static const int isDevInput = 23;

uint32_t batteryValue;
uint32_t sensorValue;

Adafruit_ADS1115 adc;

void set_up()
{
	pinMode(analogInputPin, ANALOG);
	pinMode(sensorOutPut, OUTPUT);
	pinMode(isDevInput, INPUT_PULLUP);

	if (digitalRead(isDevInput))
	{
		SetIsDev(true);
		Serial.println("Set to dev mode");
	}
	else
	{
		SetIsDev(false);
		Serial.println("Set to prod mode");
	}

	adc.setGain(GAIN_ONE);

	if (!adc.begin())
	{
		Serial.println("Failed to initialize ADS.");
		esp_deep_sleep_start();
	}
	return;
}

void mesureAndSend()
{
	float multiplier = 0.1875F;

	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111batteryValue: ");
		Serial.println(batteryValue);
		batteryValue += adc.readADC_Differential_2_3();
		delay(10);
	}

	digitalWrite(sensorOutPut, HIGH);
	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111sensorValue: ");
		Serial.println(sensorValue);
		sensorValue += adc.readADC_Differential_0_1();
		delay(10);
	}
	digitalWrite(sensorOutPut, LOW);

	Serial.print("111batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("111sensorValue: ");
	Serial.println(sensorValue);

	batteryValue = batteryValue / multisamplingLength;
	sensorValue = sensorValue / multisamplingLength;

	Serial.print("batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("sensorValue: ");
	Serial.println(sensorValue);

	upload::send(batteryValue, sensorValue);
	return;
}
