import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day4/inputA.txt', 'r') as file:
    # read a 2D array
    data = [list(line) for line in file.read().splitlines()]
    
def scan_area(data, x, y):
    width = len(data[0])
    length = len(data)
    if data[y][x] == '.':
        # no roll
        return 0
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
    return rolls


with PerformanceMonitor(name="Computation"):
    answer = 0
    removed_rolls = 1
    width = len(data[0])
    length = len(data)

    next_data = [row[:] for row in data]

    # process data
    while removed_rolls != 0:
        removed_rolls = 0
        data = [row[:] for row in next_data]
        for y in range(length):
            for x in range(width):
                if data[y][x] == '.':
                    # no roll
                    continue
                rolls = scan_area(data, x, y)
                # print(f"At {x},{y} rolls={rolls}")
                if rolls < 4:
                    # can be removed
                    next_data[y][x] = '.'
                    removed_rolls += 1
        # print(f"removed rolls: {removed_rolls}")
        answer += removed_rolls

print(answer)
