#!/usr/bin/python

import requests
import json
import sys
import time

# Дефинираме константи за API ключ, единици за измерване, език и град
API_TOKEN = "69c655f5c49d7a1612da1c5a0617d786"
UNITS = 'metric'
LANG = 'bg'
CITY = "Gabrovo"

# Функция за връщане на иконката според кода на времето
def get_icon(code):
    icons = {
        "01d": "",
        "01n": "",
        "02d": "",
        "02n": "",
        "03d": "󰖐",
        "03n": "󰖐",
        "04d": "",
        "04n": "",
        "09d": "",
        "09n": "",
        "10d": "",
        "10n": "",
        "11d": "",
        "11n": "",
        "13d": "󰖘",
        "13n": "󰖘",
        "50d": "",
        "50n": "" 
    }

    try:
        return icons[code]
    except KeyError:
        return None

# Основна функция за получаване на информация за времето
def main():
    # Извличаме данните от API на OpenWeatherMap
    response = requests.get(f"https://api.openweathermap.org/data/2.5/weather?q={CITY}&appid={API_TOKEN}&lang={LANG}&units={UNITS}").json()
    # Създаваме обект с информация за времето
    data = {
        "icon": get_icon(response['weather'][0]['icon']),
        "temp": str(round(response['main']['temp'])) + "°",
        "desc": response['weather'][0]['description'].capitalize()
    }
    return data

# Проверка дали скриптът се изпълнява като основна програма
if __name__ == "__main__":
    try:
        # Безкраен цикъл за извличане на информация за времето и извеждане на резултата
        while True:
            try:
                # Извличаме информацията за времето и я изписваме в стандартния изход
                sys.stdout.write(json.dumps(main()) + "\n")
                sys.stdout.flush()
                # Изчакваме 30 минути преди следващото извличане на информация
                time.sleep(1800)
            except requests.exceptions.ConnectionError:
                # Обработка на грешка при връзката и опит за връщане към изпълнението на цикъла
                print("Connection error! Retrying...")
                time.sleep(2)

    except KeyboardInterrupt:
        # Излизане от програмата при прекъсване от потребителя
        exit(0)
