import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BOARD)

RED = 11
YELLOW = 13
GREEN = 15

GPIO.setup(RED, GPIO.OUT)
GPIO.setup(YELLOW, GPIO.OUT)
GPIO.setup(GREEN, GPIO.OUT)


def set_off():
    GPIO.output(RED, GPIO.LOW)
    GPIO.output(YELLOW, GPIO.LOW)
    GPIO.output(GREEN, GPIO.LOW)


def set_red():
    set_off()
    GPIO.output(RED, GPIO.HIGH)


def set_yellow():
    set_off()
    GPIO.output(YELLOW, GPIO.HIGH)


def set_green():
    set_off()
    GPIO.output(GREEN, GPIO.HIGH)