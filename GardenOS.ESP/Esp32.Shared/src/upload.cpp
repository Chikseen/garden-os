#include <main.hpp>
#include "./secrets.h"

namespace upload
{
	static std::string serverAddress = "192.168.2.100";
	static const int serverPort = 5082;
	static std::string serverPath = "/devices";

	static void send(int value)
	{
		if (WiFi.status() == WL_CONNECTED)
		{
			HTTPClient http;

			// Specify the server and port
			http.begin(serverAddress.c_str(), serverPort, serverPath.c_str());

			// Start connection and send HTTP header
			std::stringstream payload;
			payload << "{\"Value\" :" << value
					<< ",\"DeviceId\" : \"" << get_device_id().c_str()
					<< "\",\"GardenId\" : \"" << get_garden_id().c_str()
					<< "\",\"ApiKey\" : \"" << get_api_key().c_str()
					<< "\"}";

			http.addHeader("Content-Type", "application/json");
			int httpResponseCode = http.POST(payload.str().c_str());

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
	}
}