import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day5/inputA.txt', 'r') as file:
    data = file.read().splitlines()

available_lines = False

with PerformanceMonitor(name="Computation"):

    answer = 0
    fresh_ids = []

    for line in data:
        if available_lines == False:
            if line=='':
                available_lines = True
                continue
            else:
                next_batch = line.split('-')
                next_batch = (int(next_batch[0]), int(next_batch[1]))
                fresh_ids.append(next_batch)
        else:
            # process availible products
            product_id = int(line)
            for batch in fresh_ids:
                if batch[0] <= product_id <= batch[1]:
                    answer += 1
                    break

print(answer)
