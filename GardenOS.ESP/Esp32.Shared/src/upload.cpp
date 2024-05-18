#include <main.h>
#include <secrets.h>
#include <wifi_setup.cpp>
#include "ValuesModel.h"

namespace upload
{
	static void post_values(ValuesModel values)
	{
		wifi_setup::connect();

		if (WiFi.status() == WL_CONNECTED)
		{
			HTTPClient http;

			// Specify the server and port
			http.begin(get_server_address().c_str(), get_server_port(), get_server_path().c_str());

			// Start connection and send HTTP header
			std::stringstream payload;
			payload
				<< "{\"ApiKey\" : \"" << get_api_key() << "\","
				<< "\"GardenId\" : \"" << get_garden_id() << "\","
				<< "\"DeviceId\" : \"" << get_device_id() << "\","
				<< "\"Sensor\" : {";

			if (get_0_value_id() != "")
			{
				payload << "\"" << get_0_value_id() << "\" : \"" << values.v_one << "\",";
			}
			if (get_1_value_id() != "")
			{
				payload << "\"" << get_1_value_id() << "\" : \"" << values.v_two << "\"}";
			}

			payload << "}";

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