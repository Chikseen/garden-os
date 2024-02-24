#include <string>
#include <secrets.h>

bool isDev()
{
	return false;
}

// WIFI
std::string get_wifi_ssid()
{
	return "WLAN-MeK";
}

std::string get_wifi_password()
{
	return "W-Pass28052077!";
}

// CONNECTION
std::string get_server_address()
{
	if (isDev())
		return "192.168.2.100";
	return "157.90.170.184";
}

int get_server_port()
{
	if (isDev())
		return 5082;
	return 9992;
}

std::string get_server_path()
{
	return "/devices";
}

// DEVICE
std::string get_api_key()
{
	return "cz8sjTLnloEntVKkQ2cUA7oyWlMPPlc6nF9SlDVj0iBubwUSZUWDNMn4IFBuErnuDeb9SHauaCXd7OL9pE51O9giANSbn0rtYMA6K3wB2jZU54vpCHdGCUG4PeNkTB0y";
}

std::string get_garden_id()
{
	return "d1526afc-9eba-4ee6-b933-c2bcd6c6ef92";
}

std::string get_device_id()
{
	return "1168cb69-c251-4f79-9886-276ddab34831";
}

std::string get_battery_id()
{
	return "33bed312-ad94-4a83-9a25-4969af3312b3";
}

std::string get_sensor_id()
{
	return "b83d29c3-24fb-4435-b89c-ccf7d0c6b03d";
}