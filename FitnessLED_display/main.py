import requests
import time
from led_controller import set_red, set_yellow, set_green, set_off

URL = "http://192.168.0.79:5011/api/status/1"

HEADERS = {
    "X-API-Key": "my-test-pi-key-12345"
}


def get_status():
    response = requests.get(
        URL,
        headers=HEADERS,
        timeout=10
    )

    print("API RESPONSE:")
    print(response.text)

    # 401 means the API key is missing or wrong.
    if response.status_code != 200:
        print("API REJECTED THE REQUEST:", response.status_code)
        return -1

    return int(response.text)


while True:
    status = get_status()

    if status == 0:
        set_red()

    elif status == 1:
        set_yellow()

    elif status == 2:
        set_green()

    else:
        set_off()

    time.sleep(10)