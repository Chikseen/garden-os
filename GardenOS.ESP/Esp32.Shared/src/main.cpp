#include <main.h>
#include <mesure.h>
#include <secrets.h>

#define uS_TO_S_FACTOR 1000000LL /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP 400LL      /* Time ESP32 will go to sleep (in seconds) */

void setup()
{
  Serial.begin(9600);
  delay(100);
  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);

  set_up();
  mesureAndSend();

  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);
  Serial.println("Setup ESP32 to sleep for every " + String(TIME_TO_SLEEP) +
                 " Seconds");

  delay(100);
  Serial.flush();

  if (!GetIsDev())
  {
    gpio_hold_en(GPIO_NUM_17);
    esp_deep_sleep_start();
  }
}

void loop()
{
  if (GetIsDev())
  {
    mesureAndSend();
    delay(1000);
  }
  else
  {
    gpio_hold_en(GPIO_NUM_17);
    esp_deep_sleep_start();
  }
}