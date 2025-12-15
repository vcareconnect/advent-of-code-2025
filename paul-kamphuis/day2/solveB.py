import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day2/inputA.txt', 'r') as file:
    data = file.read().splitlines()
    products = data[0].strip().split(',')

result = 0

def IsInvalidID(productID_str):
    idLength = len(productID_str)
    for seq_length in range(1, idLength // 2 + 1):
        digits = productID_str[:seq_length]
        # repeat digits string until lenght matches
        repeated = digits * (idLength // seq_length)

        if repeated == productID_str:
            # print(f"Invalid ID found: {productID_str}")
            return True
    return False

with PerformanceMonitor(name="Computation"):
    # process data
    for productRange in products:
        # print("Product range:", productRange)
        startID, endID = map(int, productRange.split('-'))
        for productID in range(startID, endID + 1):
            productID_str = str(productID)
            if IsInvalidID(productID_str):
                result += productID

print(result)
