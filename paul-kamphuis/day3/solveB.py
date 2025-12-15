import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day3/inputA.txt', 'r') as file:
    data = file.read().splitlines()

banks = data
total = 0

with PerformanceMonitor(name="Computation"):
    # process data
    entries = 12
    for bank in banks:
        voltage = 0
        index = 0
        for j in reversed(range(entries)):
            index = max(range(index,len(bank)-j), key=lambda i: int(bank[i]))
            voltage = voltage*10 + int(bank[index])
            index += 1
        print(voltage)
        total += voltage
print(total)
