#include <main.h>
#include <mesure.h>
#include <secrets.h>

#define uS_TO_S_FACTOR 1000000LL /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP 900LL      /* Time ESP32 will go to sleep (in seconds) */

void set_up_serial()
{
  Serial.begin(9600);
  while (!Serial)
    ;
  uint64_t sleepTime = 900000000LL;
  esp_sleep_enable_timer_wakeup(sleepTime);
  Serial.flush();
}

void setup()
{
  delay(100);
  set_up_serial();
  set_up_mesure();
}

void loop()
{
  set_up_mesure();
  ValuesModel values = mesure_adc_all_values();
  upload::post_values(values);

  if (!is_dev())
  {
    esp_deep_sleep_start();
  }
}