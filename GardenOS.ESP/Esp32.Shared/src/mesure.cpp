#include <mesure.h>
#include <secrets.h>
#include <Preferences.h>

static const int multisamplingLength = 10;

static const gpio_num_t adcPowerPin = GPIO_NUM_12;
static const gpio_num_t tempPowerPin = GPIO_NUM_16;
static const gpio_num_t soilPowerPin = GPIO_NUM_5;

Adafruit_ADS1115 adc;
Preferences pref;

void set_output_pins()
{
	// pref.begin("Relay-State", false);
	gpio_hold_dis(adcPowerPin);
	gpio_hold_dis(tempPowerPin);
	gpio_hold_dis(soilPowerPin);

	pinMode(adcPowerPin, OUTPUT);
	pinMode(tempPowerPin, OUTPUT);
	pinMode(soilPowerPin, OUTPUT);

	digitalWrite(adcPowerPin, HIGH);
	digitalWrite(tempPowerPin, HIGH);
	digitalWrite(soilPowerPin, HIGH);
}

void set_up_mesure()
{
	set_output_pins();
	adc.setGain(GAIN_ONE);

	while (!adc.begin())
	{
		Serial.println("Failed to initialize ADS.");
		Serial.println("Retry in one sec");
		delay(1000);
	}
	return;
}

uint mesure_value(size_t pin)
{
	delay(50);

	int16_t value = adc.readADC_SingleEnded(pin);

	Serial.print("pin: ");
	Serial.print(pin);
	Serial.print(" value: ");
	Serial.print(value);
	Serial.print(" : ");
	Serial.print(value * 0.000125f);
	Serial.println("V");
	return value;
}

int16_t mesure_multisample_value(size_t pin)
{
	uint64_t values = 0;

	for (size_t multisampleIndex = 0; multisampleIndex < multisamplingLength; multisampleIndex++)
	{
		values = values + mesure_value(pin);
	}

	Serial.println("A");
	Serial.println(values);

	int16_t sampledValue = values / multisamplingLength;

	Serial.println("s");
	Serial.println(sampledValue);

	return sampledValue;
}

void prepare_pins_for_sleep()
{
	digitalWrite(adcPowerPin, LOW);
	digitalWrite(tempPowerPin, LOW);
	digitalWrite(soilPowerPin, LOW);

	gpio_hold_en(adcPowerPin);
	gpio_hold_en(tempPowerPin);
	gpio_hold_en(soilPowerPin);
}

ValuesModel mesure_adc_all_values()
{
	ValuesModel values;

	values.v_one = mesure_multisample_value(0);
	values.v_two = mesure_multisample_value(1);
	values.v_three = mesure_multisample_value(2);
	values.v_four = mesure_multisample_value(3);

	prepare_pins_for_sleep();

	return values;
}