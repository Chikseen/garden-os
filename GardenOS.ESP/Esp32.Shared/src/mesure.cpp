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
	gpio_hold_dis(sensorOutPut);

	uint32_t batteryValue;
	uint32_t firstSensorValue;
	uint32_t secondSensorValue;

	float multiplier = 0.1875F;

	digitalWrite(sensorOutPut, LOW);
	delay(10);
	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("batteryValue: ");
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
		Serial.print("firstSensorValue: ");
		int16_t value = adc.readADC_SingleEnded(0);
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		firstSensorValue += value;
		delay(10);
	}

	for (size_t i = 0; i < multisamplingLength; i++)
	{
		Serial.print("secondSensorValue: ");
		int16_t value = adc.readADC_SingleEnded(3);
		Serial.print(" V: ");
		Serial.print(value * 0.000125f);
		Serial.print(" T: ");
		Serial.println(value);

		secondSensorValue += value;
		delay(10);
	}

	digitalWrite(sensorOutPut, HIGH);
	gpio_hold_en(sensorOutPut);

	batteryValue = batteryValue / multisamplingLength;
	firstSensorValue = firstSensorValue / multisamplingLength;
	secondSensorValue = secondSensorValue / multisamplingLength;

	Serial.print("batteryValue: ");
	Serial.println(batteryValue);

	Serial.print("firstSensorValue: ");
	Serial.println(firstSensorValue);

	Serial.print("secondSensorValue: ");
	Serial.println(secondSensorValue);

	uint32_t previousStoredBatteryValue = pref.getUInt("bv", 0);
	uint32_t previousStoredFirstSensorValue = pref.getUInt("fsv", 0);
	uint32_t previousStoredSecondSensorValue = pref.getUInt("ssv", 0);

	if ((((int)batteryValue - (int)previousStoredBatteryValue) > 20 || ((int)firstSensorValue - (int)previousStoredFirstSensorValue) > 20 || ((int)secondSensorValue - (int)previousStoredSecondSensorValue) > 20) || (((int)batteryValue - (int)previousStoredBatteryValue) < -20 || ((int)firstSensorValue - (int)previousStoredFirstSensorValue) < -20) || ((int)secondSensorValue - (int)previousStoredSecondSensorValue) < -20)
	{
		pref.putUInt("bv", batteryValue);
		pref.putUInt("fsv", firstSensorValue);
		pref.putUInt("ssv", secondSensorValue);
		upload::send(batteryValue, firstSensorValue, secondSensorValue);
	}

	return;
}
