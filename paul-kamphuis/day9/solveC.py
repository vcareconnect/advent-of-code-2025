
# load from file
with open('day9/sample.txt', 'r') as file:
    # read a 2D array
    data = [list(line.split(',')) for line in file.read().splitlines()]

red_tiles = [(int(row[0]), int(row[1])) for row in data]

def calculate_area(tile1, tile2):
    width = abs(tile1[0] - tile2[0]) + 1
    height = abs(tile1[1] - tile2[1]) + 1
    return width * height

# Group y positions by column (x value)
y_positions_by_x = {}
for x, y in red_tiles:
    if x not in y_positions_by_x:
        y_positions_by_x[x] = []
    y_positions_by_x[x].append(y)


# order y positions in each column
for x in y_positions_by_x:
    y_positions_by_x[x] = sorted(y_positions_by_x[x])
# order y_positions_by_x by x value
y_positions_by_x = dict(sorted(y_positions_by_x.items()))

minimum_x = min(y_positions_by_x.keys())
maximum_x = max(y_positions_by_x.keys())

tiled_positions = {}

repeat_ranges = [(min(y_positions_by_x[minimum_x]), max(y_positions_by_x[minimum_x]))]
tiled_positions[minimum_x] = repeat_ranges
for x in range(minimum_x+1, maximum_x + 1):
    if x not in y_positions_by_x:
        tiled_positions[x] = repeat_ranges
    else:
        min_y = min(y_positions_by_x[x])
        max_y = max(y_positions_by_x[x])

        non_overlapping = []
        overlapping = []
        for r in repeat_ranges:
            # Check for overlap
            if max_y < r[0] or min_y > r[1]:
                non_overlapping.append(r)
            else:
                overlapping.append(r)
        if len(overlapping) == 0:
            # no overlap, add new range
            non_overlapping.append((min_y, max_y))
            repeat_ranges = non_overlapping
            tiled_positions[x] = repeat_ranges
        else:
            # merge overlapping ranges
            if len(overlapping) >= 2:
                print("More than one overlapping range found, unexpected.")
                continue
            for r in overlapping:
                # if either end of (min_y, max_y) matches the end of r do special handling
                if min_y <= r[0] and max_y >= r[1]:
                    # (min_y, max_y) fully covers r
                    continue


# now we have all tiled positions (red + green)
print("Tiled positions calculated.")
def is_tiled(tile, y_max):
    x, y = tile
    if x not in tiled_positions:
        return False
    ranges = tiled_positions[x]
    for range in ranges:
        if range[0] <= y <= range[1] and range[0] <= y_max <= range[1]:
            return True
    return False

def is_area_tiled(tile1, tile2):
    for x in range(min(tile1[0], tile2[0]), max(tile1[0], tile2[0]) + 1):
        y = min(tile1[1], tile2[1])
        y_max = max(tile1[1], tile2[1])
        # length = y_max - y + 1
        if not is_tiled((x, y), y_max):
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
    print(f"Checking area {area} between {tile1} and {tile2}...")
    if is_area_tiled(tile1, tile2):
        max_area = area
        break

# answer = list(areas.values())[-1]
answer = max_area
print(f"Result: {answer}")
