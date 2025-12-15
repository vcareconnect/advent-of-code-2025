import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day6/inputA.txt', 'r') as file:
    data = [line.strip().split() for line in file.read().splitlines()]

# the last row are the operands
operands = data[-1]
# remove operands from data
data = data[:-1]

with PerformanceMonitor(name="Computation"):
    total_assignmets = len(operands)
    final_answer = 0
    # loop over each operation also with its index
    for idx, operation in enumerate(operands):
        answer = None
        for row in data:
            if answer is None:
                answer = int(row[idx])
            else:
                if operation == '*':
                    answer *= int(row[idx])
                elif operation == '-':
                    answer -= int(row[idx])
                elif operation == '+':
                    answer += int(row[idx])
                elif operation == '/':
                    answer //= int(row[idx])
        # print(answer)
        final_answer += answer

print(final_answer)
