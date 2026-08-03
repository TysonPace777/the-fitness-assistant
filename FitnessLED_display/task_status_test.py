import requests

url = "https://the-fitness-assistant.onrender.com/api/status/1"

response = requests.get(url)

print("API RESPONSE:", response.text)

status = int(response.text)

if status == 2:
    print("GREEN - All tasks complete")

elif status == 1:
    print("YELLOW - Some tasks complete")

else:
    print("RED - No tasks complete")