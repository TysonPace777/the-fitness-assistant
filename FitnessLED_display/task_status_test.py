import requests

url = "https://the-fitness-assistant.onrender.com/api/status/1"

# The status endpoint now requires the device API key.
headers = {
    "X-API-Key": "my-test-pi-key-12345"
}

response = requests.get(url, headers=headers)

if response.status_code != 200:
    print("API REJECTED THE REQUEST:", response.status_code)
    raise SystemExit(1)

print("API RESPONSE:", response.text)

status = int(response.text)

if status == 2:
    print("GREEN - All tasks complete")

elif status == 1:
    print("YELLOW - Some tasks complete")

else:
    print("RED - No tasks complete")