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

with PerformanceMonitor(name="Computation"):
    red_tiles = [(int(row[0]), int(row[1])) for row in data]


    # Group y positions by column (x value)
    y_positions_by_x = {}
    for x, y in red_tiles:
        if x not in y_positions_by_x:
            y_positions_by_x[x] = []
        y_positions_by_x[x].append(y)
    # check if any x has more than 2 red tiles
    for x in y_positions_by_x:
        tiles = len(y_positions_by_x[x])
        if tiles == 0 or tiles==2:
            continue
        print("More than 2 red tiles in a column, skipping")

    # order y positions in each column
    for x in y_positions_by_x:
        y_positions_by_x[x] = sorted(y_positions_by_x[x])
    # order y_positions_by_x by x value
    y_positions_by_x = dict(sorted(y_positions_by_x.items()))

    print("determining green tiles...")
    green_positions = {}
    # for entry in y_positions_by_x, give me all values between min and max y for each x
    for x in y_positions_by_x:
        min_y = min(y_positions_by_x[x])
        max_y = max(y_positions_by_x[x])
        green_positions[x] = list(range(min_y+1, max_y))

    print("A")
    # Group x positions by column (y value)
    x_positions_by_y = {}
    for x, y in red_tiles:
        if y not in x_positions_by_y:
            x_positions_by_y[y] = []
        x_positions_by_y[y].append(x)

    # order x positions in each column
    for y in x_positions_by_y:
        x_positions_by_y[y] = sorted(x_positions_by_y[y])
    # order x_positions_by_y by y value
    x_positions_by_y = dict(sorted(x_positions_by_y.items()))

    print("B")
    # Update green_positions: for each y, get all x between min and max x for that y
    green_positions2 = {}
    for y in x_positions_by_y:
        if len(x_positions_by_y[y]) < 2:
            continue
        if len(x_positions_by_y[y]) > 2:
            print("More than 2 red tiles in a row, skipping")
            continue
        min_x = min(x_positions_by_y[y])
        max_x = max(x_positions_by_y[y])
        green_positions2[y] = list(range(min_x + 1, max_x))

    # now we have all tiled positions (red + green)
    print("Tiled positions calculated.")

    def is_area_tiled(tile1, tile2):
        x_min = min(tile1[0], tile2[0])
        x_max = max(tile1[0], tile2[0])

        y_min = min(tile1[1], tile2[1])
        y_max = max(tile1[1], tile2[1])
        for x in range(x_min+1, x_max):
            if x in green_positions:
                # check if green_positions[x] contains a value between y_min and y_max
                if any(y_min < y < y_max for y in green_positions[x]):
                    return False
        for y in range(y_min+1, y_max):
            if y in green_positions2:
                if any(x_min < x < x_max for x in green_positions2[y]):
                    return False
        return True

    max_area = 0
    areas = {}
    for i, tile1 in enumerate(red_tiles):
        for j, tile2 in enumerate(red_tiles):
            if i != j and (tile2, tile1) not in areas:
                area = calculate_area(tile1, tile2)
                areas[(tile1, tile2)] = area
                # if area > max_area:
                #     if is_area_tiled(tile1, tile2):
                #         max_area = area
                # areas[(tile1, tile2)] = calculate_area(tile1, tile2)

    print("Areas calculated.")
    # sort areas by minimal value
    areas = dict(sorted(areas.items(), key=lambda item: item[1], reverse=True))
    # areas = dict(sorted(areas.items(), key=lambda item: item[1]))
    print("Areas sorted.")
    for (tile1, tile2), area in areas.items():
        # print(f"Checking area {area} between {tile1} and {tile2}...")
        if is_area_tiled(tile1, tile2):
            max_area = area
            break

# answer = list(areas.values())[-1]
answer = max_area
print(f"Result: {answer}")
