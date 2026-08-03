# Raspberry Pi LED Controller Startup Instructions

## Overview

This project creates an IoT feedback loop between the Fitness Assistant web application and a physical Raspberry Pi LED display.

The system flow:


Fitness Assistant Production App (Render)
|
|
/api/status/{userId}
|
|
Raspberry Pi Python Program
|
|
GPIO LED Controller
|
|
Red / Yellow / Green LEDs


The Raspberry Pi checks the user's daily task completion status and displays progress using LEDs:

| API Status | Task Progress | LED |
|---|---|---|
| 0 | No tasks completed | Red |
| 1 | Some tasks completed | Yellow |
| 2 | All tasks completed | Green |

---

# Raspberry Pi LED Controller Startup

## 1. Power on Raspberry Pi

1. Connect Raspberry Pi power.
2. Wait for Raspberry Pi OS to boot.
3. Connect to the Pi through SSH.

From the development computer:

```bash
ssh holly@holly-pi

Enter the Raspberry Pi password when prompted.

2. Navigate to LED Controller Project

Move into the project directory:

cd ~/fitness_led

Verify location:

pwd

Expected:

/home/holly/fitness_led

View project files:

ls

Expected:

led_controller.py
main.py
task_status_led.py
test_api.py
venv
3. Activate Python Virtual Environment

Activate the project environment:

source venv/bin/activate

The terminal should now show:

(venv) holly@holly-pi:~/fitness_led $
4. Verify GPIO Access

Optional troubleshooting command:

python3 -c "import RPi.GPIO; print('GPIO works')"

Expected:

GPIO works
5. Run LED Controller Program

Start the LED controller:

python3 task_status_led.py

Expected output:

API RESPONSE: 0
RED - No tasks complete

API RESPONSE: 1
YELLOW - Some tasks complete

API RESPONSE: 2
GREEN - All tasks complete

The program checks the production API periodically and updates the LED state.

6. Stop the Program

To stop the running Python program:

Press:

CTRL + C
GPIO Cleanup

When a Raspberry Pi GPIO program stops, it is good practice to release the GPIO pins.

The cleanup command is:

GPIO.cleanup()

This resets the GPIO pins and prevents LEDs from remaining in an unexpected state.

Manual Cleanup

If an LED remains on after stopping the program, run:

python3

Then:

import RPi.GPIO as GPIO
GPIO.cleanup()
exit()

The LEDs should turn off.

Future Improvement

The LED controller should eventually be updated to use a try/finally block:

try:
    # Run LED controller loop here

finally:
    GPIO.cleanup()

This automatically cleans up GPIO pins when the program exits.

Project Files
task_status_led.py

Purpose:

Connects to the Fitness Assistant production API hosted on Render.
Requests the current daily task completion status.
Converts the response into a physical LED state.
led_controller.py

Purpose:

Controls the physical LEDs connected to the Raspberry Pi.

GPIO wiring:

LED	Raspberry Pi BOARD Pin
Red	11
Yellow	13
Green	15

Each LED uses a 330Ω resistor.

Troubleshooting Commands
Check current directory
pwd
List project files
ls
Check Python version
python3 --version
Check API manually

Production API:

curl https://the-fitness-assistant.onrender.com/api/status/1

Possible responses:

0
1
2
Test API with Device API Key

(Currently implemented but not required for the demo)

curl -H "X-API-Key: my-test-pi-key-12345" https://the-fitness-assistant.onrender.com/api/status/1
Leave Virtual Environment

When finished:

deactivate
Hardware Notes

LED wiring:

Red LED → Raspberry Pi BOARD pin 11
Yellow LED → Raspberry Pi BOARD pin 13
Green LED → Raspberry Pi BOARD pin 15
330Ω resistor used for each LED
Ground connected to Raspberry Pi ground rail
Future Improvements

Possible future enhancements:

Run LED controller automatically at Raspberry Pi startup using a systemd service.
Store API keys securely using environment variables.
Add API retry handling if Render is unavailable.
Add automatic GPIO cleanup on shutdown.
Use HTTPS certificate validation for API requests.
Create a dedicated device authentication system.