import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day4/inputA.txt', 'r') as file:
    data = file.read().splitlines()

result = 0

with PerformanceMonitor(name="Computation"):
    total = 0
    answer = 0
    width = len(data[0])
    length = len(data)
    # process data
    for y in range(length):
        for x in range(width):
            if data[y][x] == '.':
                # no roll
                continue
            rolls = 0
            if x + 1 < width and data[y][x + 1] == '@':
                rolls += 1  # vertical roll
            if y + 1 < length and data[y + 1][x] == '@':
                rolls += 1
            if x + 1 < width and y + 1 < length and data[y + 1][x + 1] == '@':
                rolls += 1
            if x - 1 >= 0 and y + 1 < length and data[y + 1][x - 1] == '@':
                rolls += 1
            if x - 1 >= 0 and data[y][x - 1] == '@':
                rolls += 1
            if y - 1 >= 0 and data[y - 1][x] == '@':
                rolls += 1
            if x + 1 < width and y - 1 >= 0 and data[y - 1][x + 1] == '@':
                rolls += 1
            if x - 1 >= 0 and y - 1 >= 0 and data[y - 1][x - 1] == '@':
                rolls += 1
            if rolls < 4:
                # can be accessed
                answer += 1

print(answer)
