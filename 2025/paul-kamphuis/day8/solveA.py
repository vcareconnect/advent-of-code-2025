import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day8/inputA.txt', 'r') as file:
    # read a 2D array
    data = [list(line.split(',')) for line in file.read().splitlines()]


def euclidean_distance(box1, box2):
    return ((float(box1[0]) - float(box2[0])) ** 2 + (float(box1[1]) - float(box2[1])) ** 2 + (float(box1[2]) - float(box2[2])) ** 2) ** 0.5    

with PerformanceMonitor(name="Computation"):
    junction_boxes = [(row[0], row[1],row[2]) for row in data]
    circuits = []

    # I need calculate the euclidean distance between each pair of boxes in junction_boxes
    distances = {}
    for i, box1 in enumerate(junction_boxes):
        for j, box2 in enumerate(junction_boxes):
            if i != j and (box2, box1) not in distances:
                distances[(box1, box2)] = euclidean_distance(box1, box2)

    # sort distance by minimal value
    distances = dict(sorted(distances.items(), key=lambda item: item[1]))

    count = 0
    count2 = 0
    for (box1, box2) in distances:
        count2 += 1
        if count2 == 999:
            break
        # if count == 999:
        #     break
        # print(f"Closest boxes: {box1},{box2} with distance {distances[(box1, box2)]}")
        if box1 in junction_boxes and box2 in junction_boxes:
            # found a new circuit
            junction_boxes.remove(box1)
            junction_boxes.remove(box2)
            circuits.append([box1, box2])
            count+=1
            continue
        else:
            if box1 not in junction_boxes and box2 not in junction_boxes:
                # both boxes are already in circuits
                # see if they are in the same circuit
                circuit1 = [circuit for circuit in circuits if box1 in circuit]
                circuit2 = [circuit for circuit in circuits if box2 in circuit]
                if circuit1 != circuit2:
                    # merge the two circuits
                    circuit1[0].extend(circuit2[0])
                    circuits.remove(circuit2[0])
                    count+=1
                continue
            # one of the boxes is already in a circuit
            # locate the circuit and add the other box to it
            for circuit in circuits:
                if box1 in circuit and box2 not in circuit:
                    circuit.append(box2)
                    junction_boxes.remove(box2)
                    count+=1
                    break
                elif box2 in circuit and box1 not in circuit:
                    circuit.append(box1)
                    junction_boxes.remove(box1)
                    count+=1
                    break


    print(f"Junction boxes left: {len(junction_boxes)}")

    # order circuits by length descending
    circuits = sorted(circuits, key=lambda x: len(x), reverse=True)
    # for circuit in circuits:
    #     print(circuit, len(circuit))

result = len(circuits[0]) * len(circuits[1]) * len(circuits[2])
print(f"Result: {result}")
