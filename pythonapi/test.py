import requests

BASE = "http://127.0.0.1:5000/"
obj = {"P1StartQuantity":0, "P2StartQuantity":0, "P1OperatingTime(m)":0, "P2OperatingTime(m)":0,
                                "Rain(mm)":75, "Niveau(cm)":120, "Month": 3, "Day":13, "Hour":0}
response = requests.put(BASE, json=obj)
print(response.json())