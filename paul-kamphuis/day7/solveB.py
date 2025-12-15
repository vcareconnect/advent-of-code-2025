import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day7/inputA.txt', 'r') as file:
    # read a 2D array
    data = [list(line) for line in file.read().splitlines()]

def trace_path(data, x, y, count):

    if data[y][x] != '^':
        if (x, y) in lookup_table:
            # safe guard if we reach already entered path
            return lookup_table[(x, y)]
        # count += 1
        # data[y][x] = '|'  # mark as visited
        if y+1 >= len(data):
            result = count
        else:
            result = trace_path(data, x, y+1, count)
        lookup_table[(x, y+1)] = result
        return result
    else:
        # count += 1
        sub_count = 0
        if x-1 >= 0:
            sub_count = trace_path(data, x-1, y, 1)
        if x+1 < len(data[y]):
            sub_count += trace_path(data, x+1, y, 1)
    return sub_count


with PerformanceMonitor(name="Computation"):
    visited = set()
    lookup_table = {}
    result = 0

    for col in range(len(data[0])):
        if data[0][col] == 'S':
            total_count = trace_path(data, col, 1, result)
            break

print(f"Total paths from column {col}: {total_count}")