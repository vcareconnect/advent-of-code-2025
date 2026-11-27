import sys
import os
import numpy as np
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

import operator as op
import functools as ft
import itertools as it

def parse_sample_file(filepath):
    signals = []
    buttons = []
    voltages = []
    with open(filepath, 'r') as f:
        for line in f:
            # Parse boolean signals
            signal_str = line.split(']')[0][1:]
            signal = [c == '#' for c in signal_str]
            signals.append(signal)

            # Parse button groups (all (...) before the first '{')
            button_groups = []
            parts = line.split('{')[0].split(']')
            for part in parts[1:]:
                for btn in part.split('('):
                    btn = btn.strip(') ')
                    if btn and btn[0].isdigit():
                        button_groups.append(tuple(int(x) for x in btn.split(',')))
            buttons.append(button_groups)

            # Parse voltages
            if '{' in line:
                volt_str = line.split('{')[1].split('}')[0]
                voltage = [int(x) for x in volt_str.split(',')]
                voltages.append(voltage)
            else:
                voltages.append([])

    return signals, buttons, voltages

# Example usage:
signals, buttons, voltages = parse_sample_file('day10/inputA.txt')


def list_to_int(lst):
    """Convert a list of booleans to an integer bitmask."""
    return sum(1 << i if v else 0 for i, v in enumerate(lst))

# def int_to_list(x, length):
#     """Convert an integer bitmask to a list of booleans."""
#     return [(x >> i) & 1 == 1 for i in range(length)]

def find_buttons_to_press_optimized(signal, button_masks):
    n = len(button_masks)
    signal_mask = list_to_int(signal)
    # button_masks = [list_to_int(btn) for btn in buttons]
    for r in range(1, n + 1):
        for combo in it.combinations(range(n), r):
            state = 0
            for idx in combo:
                state ^= button_masks[idx]
            if state == signal_mask:
                return [button_masks[i] for i in combo]
    return []

answer = 0

# def button_to_flag_mask(buttons, signal_length):
#     mask_list = {}
#     for button in buttons:
#         flags = [False] * signal_length
#         for idx in button:
#             if 0 <= idx < len(flags):
#                 flags[idx] = True

#         l = list(flags)
#         mask_list[list_to_int(l)] = button
#     return mask_list

# def mask_voltage(voltage):
#     return [v % 2 == 1 for v in voltage]

# def update_result(result_buttons, btn, count):
#     if btn in result_buttons:
#         result_buttons[btn] += count
#     else:
#         result_buttons[btn] = count

# def decrement_joltage(joltage_state, button, count):
#     for i in button:
#         joltage_state[i] -= count
#         if joltage_state[i] < 0:
#             joltage_state[i] = 0


# with PerformanceMonitor(name="Computation"):
#     for index, voltage in enumerate(voltages):
#         available_buttons = buttons[index]
#         result_buttons = {}
#         target_voltage = voltage
#         while target_voltage != [0] * len(voltage):
#             voltage_masked = mask_voltage(target_voltage)
#             button_masks = button_to_flag_mask(available_buttons, len(voltage))
#             buttons_to_press = find_buttons_to_press_optimized(voltage_masked, list(button_masks.keys()))
#             pressed_buttons = [button_masks[mask] for mask in buttons_to_press]
#             # result_buttons += pressed_buttons
#             for btn in pressed_buttons:
#                 update_result(result_buttons, btn, 1)
#                 decrement_joltage(target_voltage, btn, 1)
#         answer += len(result_buttons)

# print("Answer:", answer)

def increment_joltage(joltage_state, button, required, count):
    for i in button:
        joltage_state[i] += count
        if joltage_state[i] > required[i]:
            return False
    return True

def buttons_to_matrix(buttons, signal_length):
    A = np.zeros((signal_length, len(buttons)), dtype=int)
    for j, button in enumerate(buttons):
        # for button in buttons:
        for i in button:
            A[i][j] = 1
    return A

def compose(f, g):
    """Return a function composed of two functions."""
    def h(*args, **kwargs):
        return f(g(*args, **kwargs))
    return h

def vector_combs(v):
    """Return a Cartesian product of unpacked elements from `v`."""
    plus_one = ft.partial(op.add, 1)
    range_plus_one = compose(range, plus_one)
    # res = list()
    return it.product(*map(range_plus_one, v))

def select_combinations(base, n):
    ranges = [range(val, val + n + 1) for val in base]
    return it.product(*ranges)
    # return it.combinations_with_replacement(base, n)

def select_buttons(buttons, required_joltage):
    button_list = {}
    for btn in buttons:
        repeat_counts = []
        for index in btn:
            repeat_counts.append(required_joltage[index])
        
        # add button repeat times to the button list
        button_list[btn] = min(repeat_counts)
    return button_list

with PerformanceMonitor(name="Computation"):
    for index, voltage in enumerate(voltages):
        available_buttons = buttons[index]
        A = buttons_to_matrix(available_buttons, len(voltage))
        max_usable = select_buttons(available_buttons, voltage)
        # t = vector_combs(max_usable.values())
        req_voltage = np.array(voltage)
        print("Required Voltage:", voltage)
        max_presses = sum(max_usable.values()) + 1
        d = np.linalg.lstsq(A, req_voltage, rcond=None)
        print("raw Least squares estimate:", d)
        d = d[0].astype(int)
        min_d = min(d)
        # if min_d >= 0:
        #     min_d = 0
        # else:
        #     min_d = -2
        # d = d + min_d
        d = np.where(d < 0, 0, d)
        # d = [max(0, x - min_d) for x in d]
        print("Least squares estimate:", d)
        found_r = None
        for r in select_combinations(d,3):
        # for r in vector_combs(max_usable.values()):
            x = np.linalg.matmul(A, r)
            if np.array_equal(x, req_voltage):
                # print("Found combination:", r)
                # print("With buttons:", available_buttons)
                presses = sum(r)
                if presses < max_presses:
                    max_presses = presses
                    found_r = r
                # break
        # C = np.transpose(A)
        # columnv = np.array(voltage)
        # r = [1, 3, 0, 3, 1, 2]
        # state = [0] * len(voltage)
        # for i, v in enumerate(r):
        #     btn = available_buttons[i]
        #     print("Incrementing voltage with button:", btn, "by", v)
        #     increment_joltage(state, btn, voltage, v)
        # x = np.linalg.matmul(A, found_r)
        # print("Matrix A:\n", A)
        # print("Voltage Vector:\n", voltage)
        print(f"{voltage=} {found_r}")
        answer += sum(found_r)
        # break
        # print("Result from matrix solve:", result)
        # answer += sum(result)

print("Answer:", answer)
