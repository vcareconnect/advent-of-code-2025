import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
with open('day2/inputA.txt', 'r') as file:
    data = file.read().splitlines()
    products = data[0].strip().split(',')

result = 0

with PerformanceMonitor(name="Computation"):
    # process data
    for productRange in products:
        print("Product range:", productRange)
        startID, endID = map(int, productRange.split('-'))
        for productID in range(startID, endID + 1):
            productID_str = str(productID)
            idLength = len(productID_str)
            if idLength %2 == 0:
                x = productID_str[:idLength // 2]
                y = productID_str[idLength // 2:] 
                if  x==y:
                    result += productID

print(result)
