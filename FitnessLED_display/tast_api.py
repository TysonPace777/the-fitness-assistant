import requests

url = "https://localhost:7087/api/status/1"

headers = {
    "X-API-Key": "my-test-pi-key-12345"
}

response = requests.get(
    url,
    headers=headers
)

print(response.status_code)
print(response.json())