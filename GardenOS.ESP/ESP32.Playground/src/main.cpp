#include <iostream>
#include <WiFi.h>

void setup()
{
  Serial.begin(9600);
  delay(100);
  pinMode(12, OUTPUT);
}

void loop()
{
  delay(1500);
  digitalWrite(12, 1);
  delay(1500);
  digitalWrite(12, 0);
}