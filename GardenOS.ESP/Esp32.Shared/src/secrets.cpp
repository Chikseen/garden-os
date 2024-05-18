#include <string>
#include <secrets.h>

bool isDev = false;

bool is_dev()
{
	return isDev;
}

void SetIsDev(bool value)
{
	isDev = value;
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
	if (is_dev())
		return "192.168.2.100";
	return "49.13.196.127";
}

int get_server_port()
{
	if (is_dev())
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
	return "ff2ea42a200d4dbd879d5cdd6ba829ccf3d589f958ab48fdb13f5840d7a56034bfad114124f228403937b11859c0c";
}

std::string get_garden_id()
{
	if (is_dev())
		return "aaa80c8a-a5e0-492f-bbe6-52cd892a34ff";
	return "aaa80c8a-a5e0-492f-bbe6-52cd892a34ff";
}

std::string get_device_id()
{
	return "2bb8da0d-c89d-47aa-adc8-5e056f5fa800";
}

std::string get_0_value_id()
{
	return "c6c732e2-c1f3-4517-8fe6-732320cf56c7";
}

std::string get_1_value_id()
{
	return "3ae9ecab-eccb-44dc-b797-b6f1fe32576a";
}

std::string get_2_value_id()
{
	return "";
}

std::string get_3_value_id()
{
	return "";
}