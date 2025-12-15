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

    def reset_calculated_routes(target_server):
        global calculated_routes
        calculated_routes = {}
        calculated_routes[target_server] = 1  # Base case

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

answer = 0

with PerformanceMonitor(name="Computation"):
    calculated_routes = {}
    calculated_routes["out"] = 0  # Base case

    reset_calculated_routes("fft")
    answer_dac_fft = find_routes("dac")

    reset_calculated_routes("dac")
    answer_fft_dac = find_routes("fft")

    reset_calculated_routes("out")
    answer_fft_out = find_routes("fft")

    reset_calculated_routes("dac")
    answer_srv_dac = find_routes("svr")
    reset_calculated_routes("out")
    answer_dac_out = find_routes("dac")
    reset_calculated_routes("fft")
    answer_srv_fft = find_routes("svr")

    print(f"{answer_dac_fft=}, {answer_fft_dac=}, {answer_fft_out=}, {answer_srv_dac=}, {answer_dac_out=}, {answer_srv_fft=}")

    if answer_dac_fft == 0:
        answer = answer_fft_dac * answer_dac_out * answer_srv_fft
    elif answer_fft_dac == 0:
        answer = answer_dac_fft * answer_fft_out * answer_srv_dac

print(f"Result: {answer}")
