import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day5/inputA.txt', 'r') as file:
    data = file.read().splitlines()

fresh_ids = []

with PerformanceMonitor(name="Computation"):
    for line in data:
        if line=='':
            break
        else:
            next_batch = line.split('-')
            next_batch = (int(next_batch[0]), int(next_batch[1]))
            fresh_ids.append(next_batch)

    # order fresh ids based on min value
    fresh_ids.sort(key=lambda x: x[0])

    answer = 0
    found_id = 0
    for batch in fresh_ids:
        if batch[0] > found_id:
            # there is a gap
            found_id = batch[1]
            answer += batch[1] - batch[0] + 1
        elif batch[1] > found_id:
            answer += batch[1] - found_id
            found_id = batch[1]

print(answer)
