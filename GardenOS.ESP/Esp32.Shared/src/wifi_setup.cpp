#include <main.h>
#include <secrets.h>
namespace wifi_setup
{
	static void connect()
	{
		get_api_key();
		WiFi.disconnect(false);
		WiFi.mode(WIFI_STA);
		WiFi.begin(get_wifi_ssid().c_str(), get_wifi_password().c_str());
		Serial.print("Connecting to ");
		Serial.println(get_wifi_ssid().c_str());

		while (WiFi.status() != WL_CONNECTED)
		{
			delay(100);
			Serial.print(".");
		}

		Serial.println("");
		Serial.println("WiFi connected");
		Serial.println("IP address: ");
		Serial.println(WiFi.localIP());
	}
} // namespace wifi_setup
