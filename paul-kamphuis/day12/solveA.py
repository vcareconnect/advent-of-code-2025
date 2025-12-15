import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
def parse_input(filename):
    with open(filename, 'r') as file:
        lines = [line.rstrip('\n') for line in file]

    shapes = []
    regions = []
    i = 0
    # Parse shapes
    while i < len(lines):
        if lines[i].strip() == '':
            i += 1
            continue
        # Parse regions
        if 'x' in lines[i]:
            size, *counts = lines[i].split(':')
            width, height = map(int, size.split('x'))
            counts = list(map(int, ' '.join(counts).split()))
            # print(f"Parsed region: {width}x{height} with counts {counts}")
            regions.append({'width': width, 'height': height, 'counts': counts})
        elif ':' in lines[i]:
            # Start of a new shape
            shape = []
            i += 1
            while i < len(lines) and lines[i] and ':' not in lines[i]:
                shape.append([c == '#' for c in lines[i]])
                i += 1
            shapes.append(shape)

        i += 1

    return shapes, regions


with PerformanceMonitor(name="Computation"):
    shapes, regions = parse_input('day12/inputA.txt')

    answer = 0

    for region in regions:
        width = region['width']
        height = region['height']
        counts = region['counts']
        area = width * height
        presents_area = 0
        for shape_index, count in enumerate(counts):
            if count == 0:
                continue
            shape = shapes[shape_index]
            # count True cells in shape
            presents_area += count*sum(cell for row in shape for cell in row)
            # print(f"Adding shape {shape_index} with area {sum(cell for row in shape for cell in row)}")
        if presents_area <= area:
            answer += 1
            # print(f"Region {width}x{height} with counts {counts} can be fully covered. {presents_area}")
        # else:
            # print(f"Region {width}x{height} with counts {counts} cannot be fully covered. {presents_area}")


print(f"Result: {answer=}")
