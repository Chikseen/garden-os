#include <main.h>
#include <secrets.h>
#include <wifi_setup.cpp>

namespace upload
{
	static void send(int16_t batteryValue, int16_t firstSensorValue, int16_t secondSensorValue)
	{
		wifi_setup::connect();
		Serial.println("Try to send data:");

		if (WiFi.status() == WL_CONNECTED)
		{
			HTTPClient http;

			Serial.print("host: ");
			Serial.print(get_server_address().c_str());
			Serial.print(get_server_port());
			Serial.println(get_server_path().c_str());

			// Specify the server and port
			http.begin(get_server_address().c_str(), get_server_port(), get_server_path().c_str());

			// Start connection and send HTTP header
			std::stringstream payload;
			payload
				<< "{\"ApiKey\" : \"" << get_api_key() << "\","
				<< "\"GardenId\" : \"" << get_garden_id() << "\","
				<< "\"DeviceId\" : \"" << get_device_id() << "\","
				<< "\"Sensor\" : {"
				<< "\"" << get_battery_id() << "\" : \"" << batteryValue << "\","
				<< "\"" << get_first_sensor_id() << "\" : \"" << firstSensorValue << "\","
				<< "\"" << get_second_sensor_id() << "\" : \"" << secondSensorValue << "\"}"
				<< "}";

			http.addHeader("Content-Type", "application/json");

			Serial.println("JSON:");
			Serial.println(payload.str().c_str());
			Serial.println("");
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

		WiFi.disconnect(true);
		WiFi.mode(WIFI_OFF);
	}
}