import requests

BASE = "http://127.0.0.1:5000/"
obj = {"P1StartQuantity":0.0, "P2StartQuantity":0.0, "P1OperatingTime":0.0, "P2OperatingTime":0.0,
                                "Rain":75.0, "Niveau":120.0, "month": 3, "day":13, "hour":0}
response = requests.put(BASE+'', obj)
print(response.json())