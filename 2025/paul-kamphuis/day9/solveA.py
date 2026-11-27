import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day9/inputA.txt', 'r') as file:
    # read a 2D array
    data = [list(line.split(',')) for line in file.read().splitlines()]

def calculate_area(tile1, tile2):
    width = abs(tile1[0] - tile2[0]) + 1
    height = abs(tile1[1] - tile2[1]) + 1
    return width * height

answer = 0
with PerformanceMonitor(name="Computation"):
    red_tiles = [(int(row[0]), int(row[1])) for row in data]
    circuits = []


    # I need calculate the euclidean distance between each pair of boxes in junction_boxes
    areas = {}
    for i, tile1 in enumerate(red_tiles):
        for j, tile2 in enumerate(red_tiles):
            if i != j and (tile2, tile1) not in areas:
                areas[(tile1, tile2)] = calculate_area(tile1, tile2)

    # sort areas by minimal value
    areas = dict(sorted(areas.items(), key=lambda item: item[1]))

    answer = list(areas.values())[-1]

print(f"Result: {answer}")
