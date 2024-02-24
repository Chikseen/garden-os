#include <string>

bool isDev();

// WIFI
std::string get_wifi_ssid();
std::string get_wifi_password();

// CONNECTION
std::string get_server_address();
int get_server_port();
std::string get_server_path();

// DEVICE
std::string get_device_id();
std::string get_garden_id();
std::string get_api_key();
std::string get_battery_id();
std::string get_sensor_id();
