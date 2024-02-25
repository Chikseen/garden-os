#include <main.h>
#include <wifi_setup.cpp>
#include <mesure.h>
#include <secrets.h>

#define uS_TO_S_FACTOR 1000000LL /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP 60LL       /* Time ESP32 will go to sleep (in seconds) */

void setup()
{
  Serial.begin(9600);
  delay(1000);
  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);

  wifi_setup::connect();
  set_up();
  mesureAndSend();

  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);
  Serial.println("Setup ESP32 to sleep for every " + String(TIME_TO_SLEEP) +
                 " Seconds");

  delay(1000);
  Serial.flush();

  if (!isDev())
  {
    esp_deep_sleep_start();
  }
}

void loop()
{
  if (isDev())
  {
    mesureAndSend();
    delay(1000);
  }
  else
  {
    esp_deep_sleep_start();
  }
}