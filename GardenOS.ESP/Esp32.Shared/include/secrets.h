#include <string>

bool is_dev();
void SetIsDev(bool value);

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
std::string get_0_value_id();
std::string get_1_value_id();
std::string get_2_value_id();
std::string get_3_value_id();
