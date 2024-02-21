#include <main.hpp>
#include "./upload.cpp"

namespace mesure
{
	static const int analogInputPin = 34;
	static int analogValue = 0;

	static void set_up()
	{
		pinMode(analogInputPin, ANALOG);
	}

	static void mesureAndSend()
	{
		analogValue = analogRead(analogInputPin);
		std::cout << "Prepare to send Value: " << analogValue << "\n";

		upload::send(analogValue);
	}
}