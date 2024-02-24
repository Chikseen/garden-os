#include <main.h>
#include <wifi_setup.cpp>
#include <mesure.h>
#include <secrets.h>

#define uS_TO_S_FACTOR 1000000LL /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP 60LL       /* Time ESP32 will go to sleep (in seconds) */

void setup()
{
  delay(100);
  Serial.begin(9600);

  wifi_setup::connect();
  set_up();
  mesureAndSend();

  delay(1000);
  Serial.flush();

  if (!isDev())
  {
    esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);
    Serial.println("Setup ESP32 to sleep for every " + String(TIME_TO_SLEEP) +
                   " Seconds");

    Serial.println("Going to sleep now");
    esp_deep_sleep_start();
  }
}

void loop()
{
  mesureAndSend();
  delay(1000);
}