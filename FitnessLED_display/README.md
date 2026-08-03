# Fitness Assistant LED Controller

A Raspberry Pi IoT project that connects to the Fitness Assistant application and displays daily task progress using LEDs.

## Purpose

The Raspberry Pi checks the user's daily task completion status from the Fitness Assistant web application and displays progress:

- 🔴 Red - No tasks completed
- 🟡 Yellow - Some tasks completed
- 🟢 Green - All tasks completed

## Hardware

- Raspberry Pi
- Breadboard
- LEDs
- 330Ω resistors
- Jumper wires

## Software

- Python
- Raspberry Pi GPIO
- REST API
- Git/GitHub

## Running the Project

Activate the virtual environment:

```bash
source venv/bin/activate

Run:

python3 task_status_led.py

Files

task_status_led.py - Main program that checks task status and controls LEDs
led_controller.py - GPIO LED control functions
task_status_test.py - Tests the API connection

## Notes

Built on Raspberry Pi as part of the Fitness Assistant IoT integration project.
```
