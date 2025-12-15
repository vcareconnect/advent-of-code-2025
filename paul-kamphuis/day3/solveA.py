import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day3/sample.txt', 'r') as file:
    data = file.read().splitlines()
    # products = data[0].strip().split(',')

result = 0

with PerformanceMonitor(name="Computation"):
    banks = data
    total = 0
    # process data
    for bank in banks:

        max_digit_index = max(range(len(bank)-1), key=lambda i: int(bank[i]))
        next_max_digit_index = max(
            range(max_digit_index + 1, len(bank)),
            key=lambda i: int(bank[i]),
            default=None
        ) if max_digit_index + 1 < len(bank) else None
        voltage = int(bank[max_digit_index])*10+ (int(bank[next_max_digit_index]) if next_max_digit_index is not None else 0)
        total += voltage

print(total)
