/*
 * Скрипт разработал Алешин С.А.
 * Специально для СС "Гонец"
 * Измерительно температуры и влажности с интерактивным выводом на сайт
 * 16/12/2020
 */
#include <ESP8266WiFi.h>
#include "DHTesp.h"

//Настройки для работы с WiFi
DHTesp dht;
const char* ssid = "";
const char* password = "";
WiFiServer server(80);

//точка росы
float Td = 8.8;

void setup() {
  dht.setup(2, DHTesp::DHT22);
  Serial.begin(115200);
  delay(10);
  // prepare GPIO2
  pinMode(2, OUTPUT);
  digitalWrite(2, 0);
  //Подключение к WiFi
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  //Загрузка сервера
  server.begin();
  Serial.println("Server started");
  //Вывод IP адреса
  Serial.println(WiFi.localIP());  
}
void loop() {

  delay(dht.getMinimumSamplingPeriod());

  float humidity = dht.getHumidity();
  float temperature = dht.getTemperature();

  Serial.print(dht.getStatusString());
  Serial.print("\t");
  Serial.print(humidity, 1);
  Serial.print("\t");
  Serial.println(temperature, 1);

  //Рассчитываем относительную влажность
  //humidity = 100 - 5 * (temperature - 8.8);

  
  // Check if a client has connected
  WiFiClient client = server.available();

  client.flush();
  // Prepare the response
  String s = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<!DOCTYPE HTML>\r\n<html>\r\n";
  s += "<head><meta charset=\"utf-8\"></head>";
  s += "<style> .back {height: 700px; color: black; font-size: 16pt; text-align: center; background-image: url(https://cdnimg.rg.ru/img/content/187/13/91/iStock-1012400706_d_850_d_850.jpg); background-size: cover;}</style>";
  s += "<script>function restartPage() {window.location.reload();} setTimeout(restartPage, 60000);</script>";
  s += "<div class=\"back\"><br><div style=\"position: absolute; background-color: white; width: 98.8%; height: 80px; margin: auto; padding: 5px; opacity: 0.8;\"><b>Серверная: </b><br>";
  if (temperature < 25)
  {
    s += "Температура: "+String(temperature)+"<br>";
  }
  else
  {
    s += "Температура: <b style=\"color: red\">"+String(temperature)+"</b><br>";
    s += "<audio autoplay><source src=\"https://drive.google.com/u/0/uc?id=1nE3zgV2aNYro5QF51_m4Rrvav12Ndppe&export=download\" type=\"audio/mp3\"></audio>";
  }

  if (humidity < 85)
  {
    s += "Влажность: "+String(humidity);
  }
  else
  {
    s += "Влажность: <b style=\"color: red\">"+String(humidity)+"</b><br>";
    s += "<audio autoplay><source src=\"https://drive.google.com/u/0/uc?id=1beQM_8aBOuKMcjhvKhgYQcRKcoYeLbju&export=download\" type=\"audio/mp3\"></audio>";
  }
  
  s += "</div></div></html>\n";
  // Send the response to the client
  client.print(s);
  delay(5000);
}
