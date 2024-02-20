#include <WiFi.h>
#include <HTTPClient.h>
#include "secrets.cpp"

const int serverPort = 9992; // Port of the server, typically 80 for HTTP
const int analogInputPin = 34;
int analogValue = 0;
const char *serverAddress = "157.90.170.184"; // IP address or domain name of the server you want to request from

Secrets *secrets = new Secrets;

void setup()
{
	Serial.begin(9600);
	delay(1000);

	pinMode(analogInputPin, ANALOG);

	// Connect to Wi-Fi
	WiFi.begin(secrets->ssid, secrets->password);
	Serial.print("Connecting to ");
	Serial.println(secrets->ssid);

	while (WiFi.status() != WL_CONNECTED)
	{
		delay(1000);
		Serial.print(".");
	}

	Serial.println("");
	Serial.println("WiFi connected");
	Serial.println("IP address: ");
	Serial.println(WiFi.localIP());
}

void loop()
{
	analogValue = analogRead(analogInputPin);
	Serial.println(analogValue);
	delay(2000);
	if (WiFi.status() == WL_CONNECTED)
	{
		HTTPClient http;

		// Specify the server and port
		http.begin(serverAddress, serverPort, "/");

		// Start connection and send HTTP header
		int httpResponseCode = http.GET();

		if (httpResponseCode > 0)
		{
			Serial.print("HTTP Response code: ");
			Serial.println(httpResponseCode);

			String payload = http.getString(); // Get the response payload
			Serial.println(payload);
		}
		else
		{
			Serial.print("Error code: ");
			Serial.println(httpResponseCode);
		}

		http.end(); // Close connection
	}
	else
	{
		Serial.println("WiFi Disconnected. Reconnecting...");
		WiFi.reconnect();
	}

	delay(5000); // Send the request every 5 seconds (for demonstration purposes)*/
}