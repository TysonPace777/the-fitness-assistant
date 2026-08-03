import requests
import time
from led_controller import set_red, set_yellow, set_green, set_off


URL = "https://the-fitness-assistant.onrender.com/api/status/1"

HEADERS = {
    "X-API-Key": "my-test-pi-key-12345"
}


def get_task_status():

    response = requests.get(
        URL,
        headers=HEADERS,
        timeout=10
    )

    print("API RESPONSE:", response.text)

    return int(response.text)


while True:

    status = get_task_status()

    if status == 0:
        set_red()
        print("RED - No tasks complete")

    elif status == 1:
        set_yellow()
        print("YELLOW - Some tasks complete")

    elif status == 2:
        set_green()
        print("GREEN - All tasks complete")

    else:
        set_off()
        print("Unknown status")

    time.sleep(5)