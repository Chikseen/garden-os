#include <mesure.h>
#include <secrets.h>

static const int multisamplingLength = 10;

static const gpio_num_t analogInputPin = GPIO_NUM_34;
static const gpio_num_t sensorOutPut = GPIO_NUM_17;
static const gpio_num_t isDevInput = GPIO_NUM_23;

Adafruit_ADS1115 adc;

void set_up()
{
	pinMode(analogInputPin, ANALOG);
	pinMode(sensorOutPut, OUTPUT);
	pinMode(isDevInput, INPUT_PULLUP);

	adc.setGain(GAIN_ONE);

	digitalWrite(sensorOutPut, LOW);
	if (!adc.begin())
	{
		Serial.println("Failed to initialize ADS.");
		esp_deep_sleep_start();
	}
	return;
}

void mesureAndSend()
{
	uint32_t batteryValue;
	uint32_t sensorValue;

	digitalWrite(sensorOutPut, LOW);
	float multiplier = 0.1875F;

	delay(500);
	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111batteryValue: ");
		int16_t value = adc.readADC_SingleEnded(2);
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		batteryValue += value;
		delay(500);
	}

	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111sensorValue: ");
		int16_t value = adc.readADC_SingleEnded(0);
		sensorValue += value;
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		delay(500);
	}
	delay(500);
	digitalWrite(sensorOutPut, HIGH);

	batteryValue = batteryValue / multisamplingLength;
	sensorValue = sensorValue / multisamplingLength;

	Serial.print("batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("sensorValue: ");
	Serial.println(sensorValue);

	upload::send(batteryValue, sensorValue);
	return;
}
