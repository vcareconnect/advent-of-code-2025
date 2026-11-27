import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day1/inputA.txt', 'r') as file:
    data = file.read().splitlines()

with PerformanceMonitor(name="Computation"):
    # process data
    dialer = 50
    password = 0

    for line in data:
        # print("Processing line:", line)
        direction = line[0]
        positions = int(line[1:])
        # print(f"Direction: {direction}, Positions: {positions}")
        rounds = positions // 100
        if rounds:
            password += rounds
        positions = positions % 100
        if direction == 'L':
            if dialer == 0:
                dialer -= positions
            else:
                dialer -= positions
                if dialer <= 0:
                    password += 1
        elif direction == 'R':
            if dialer == 0:
                dialer += positions
            else:        
                dialer += positions
                if dialer >= 100:
                    password += 1
        #print("Current dialer position:", dialer)
        dialer %= 100

        # print(f"The dial is rotated {line} to point at {dialer}")
        # print("Current password value:", password)


print("Final dialer position:", dialer)
print("Password:", password)
