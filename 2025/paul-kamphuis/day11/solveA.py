import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file

connections = {}
with open('day11/inputA.txt', 'r') as file:
    for line in file:
        server, conns = line.strip().split(':', 1)
        connections[server.strip()] = [c.strip() for c in conns.strip().split()]

calculated_routes = {}
calculated_routes["out"] = 1  # Base case

def find_routes(from_server):
    if from_server in calculated_routes:
        return calculated_routes[from_server]
    out_conns = connections.get(from_server, [])
    server_routes = 0
    for to_server in out_conns:
        routes = find_routes(to_server)
        server_routes += routes
    calculated_routes[from_server] = server_routes
    return server_routes

answer = find_routes("you")
print(f"Result: {answer}")
