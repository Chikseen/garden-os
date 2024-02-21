#include <main.hpp>
#include "./wifi_setup.cpp"
#include "./mesure.cpp"

#define uS_TO_S_FACTOR 1000000LL /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP 30LL       /* Time ESP32 will go to sleep (in seconds) */

void setup()
{
  Serial.begin(9600);
  delay(100);
  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);

  wifi_setup::connect();
  mesure::set_up();
  mesure::mesureAndSend();

  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);
  Serial.println("Setup ESP32 to sleep for every " + String(TIME_TO_SLEEP) +
                 " Seconds");

  Serial.println("Going to sleep now");
  delay(1000);
  Serial.flush();
  esp_deep_sleep_start();
}

void loop()
{
}