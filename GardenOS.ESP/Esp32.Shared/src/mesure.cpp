#include <main.hpp>
#include "./upload.cpp"

namespace mesure
{
	static const int analogInputPin = 34;
	static const int enviromentJumper = 23;

	static int analogValue = 0;
	static int enviromentValue = 0;

	static void set_up()
	{
		pinMode(analogInputPin, ANALOG);
		pinMode(enviromentJumper, INPUT_PULLUP);
	}

	static void mesureAndSend()
	{
		analogValue = analogRead(analogInputPin);
		enviromentValue = digitalRead(enviromentJumper);
		bool env = enviromentValue;
		std::cout << "Prepare to send Value: " << analogValue << "\n";
		std::cout << "To: " << enviromentValue << "\n";

		upload::send(analogValue);
	}
}