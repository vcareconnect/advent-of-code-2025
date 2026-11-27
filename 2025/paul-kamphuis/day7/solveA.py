import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day7/inputA.txt', 'r') as file:
    # read a 2D array
    data = [list(line) for line in file.read().splitlines()]

result = 0
def trace_path(data, x, y, count):
    # print("*********************")
    # print(count)
    # [print(row) for row in data]
    if data[y][x] != '^':
        if data[y][x] == '|':
            # safe guard if we reach already entered path
            return count
        # count += 1
        data[y][x] = '|'  # mark as visited
        if y+1 >= len(data):
            return count
        return trace_path(data, x, y+1, count)
    else:
        count += 1
        if x-1 >= 0:
            count += trace_path(data, x-1, y, 0)
        if x+1 < len(data[y]):
            count += trace_path(data, x+1, y, 0)
    return count


with PerformanceMonitor(name="Computation"):
    for col in range(len(data[0])):
        if data[0][col] == 'S':
            total_count = trace_path(data, col, 1, result)
            break
print(f"Total paths from column {col}: {total_count}")