#include <mesure.h>
#include <secrets.h>
#include <Preferences.h>

static const int multisamplingLength = 10;

static const gpio_num_t analogInputPin = GPIO_NUM_34;
static const gpio_num_t sensorOutPut = GPIO_NUM_17;
static const gpio_num_t isDevInput = GPIO_NUM_23;

Adafruit_ADS1115 adc;
Preferences pref;

void set_up()
{
	pref.begin("Relay-State", false);
	gpio_hold_dis(sensorOutPut);
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

	float multiplier = 0.1875F;

	digitalWrite(sensorOutPut, LOW);
	delay(10);
	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111batteryValue: ");
		int16_t value = adc.readADC_SingleEnded(2);
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		batteryValue += value;
		delay(10);
	}

	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("nnn111sensorValue: ");
		int16_t value = adc.readADC_SingleEnded(0);
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		sensorValue += value;
		delay(10);
	}

	digitalWrite(sensorOutPut, HIGH);
	gpio_hold_en(sensorOutPut);

	batteryValue = batteryValue / multisamplingLength;
	sensorValue = sensorValue / multisamplingLength;

	Serial.print("batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("sensorValue: ");
	Serial.println(sensorValue);

	uint32_t previousStoredBatteryValue = pref.getUInt("batteryVlaue", 0);
	uint32_t previousStoredSensorValue = pref.getUInt("sensorValue", 0);

	if ((((int)batteryValue - (int)previousStoredBatteryValue) > 20 || ((int)sensorValue - (int)previousStoredSensorValue) > 20) || (((int)batteryValue - (int)previousStoredBatteryValue) < -20 || ((int)sensorValue - (int)previousStoredSensorValue) < -20))
	{
		pref.putUInt("batteryVlaue", batteryValue);
		pref.putUInt("sensorValue", sensorValue);
		upload::send(batteryValue, sensorValue);
	}

	return;
}
