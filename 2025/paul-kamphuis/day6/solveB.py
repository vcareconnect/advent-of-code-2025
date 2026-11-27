import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day6/inputA.txt', 'r') as file:
    data = [file.read().splitlines()]

    # convert each row in data to an array of characters, keeping all spaces
    data = [list(row) for row in data[0]]

    while len(data[-1]) < len(data[0]):
        data[-1].append(' ')

numbers = []
final_answer = 0

with PerformanceMonitor(name="Computation"):
    for i in reversed(range(len(data[0]))):
        # loop over the indexed rows
        number = 0
        for j in range(len(data)):
            operation = data[j][i]
            if data[j][i].isdigit():
                number = number*10 + int(data[j][i])
        if number != 0:
            numbers.append(number)

        if operation == '*':
            answer = 1
            for n in numbers:
                answer *= n
            final_answer += answer
            print(answer)
            numbers = []
        elif operation == '+':
            answer = sum(numbers)
            final_answer += answer
            print(answer)
            numbers = []    

print(final_answer)
