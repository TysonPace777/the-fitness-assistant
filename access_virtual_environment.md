# Accessing a Python Virtual Environment on Raspberry Pi

## Purpose

This document explains how to access the Python virtual environment used for the Raspberry Pi Fitness LED Controller project.

The virtual environment keeps project-specific Python packages separate from the system Python installation.

Project location:
/home/holly/fitness_led_test

Virtual environment location:
/home/holly/fitness_led_test/.venv

---

# 1. SSH into the Raspberry Pi

From your computer:

```bash
ssh holly@holly-pi.local

cd ~/fitness_led_test

holly@holly-pi:~/fitness_led_test $

source .venv/bin/activate

(.venv) holly@holly-pi:~/fitness_led_test $

python fitness_led_controller.py
```

This is a great note to keep because future-you will absolutely forget the exact activation command (`source .venv/bin/activate`). 😄 It is one of those commands every developer looks up repeatedly until it becomes muscle memory.
