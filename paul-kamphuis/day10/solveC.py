import sys
import os
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
from helper.PerformanceMonitor import PerformanceMonitor

# load from file
import itertools

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
        for combo in itertools.combinations(range(n), r):
            state = 0
            for idx in combo:
                state ^= button_masks[idx]
            if state == signal_mask:
                return [button_masks[i] for i in combo]
    return []

answer = 0

def button_to_flag_mask(buttons, signal_length):
    mask_list = {}
    for button in buttons:
        flags = [False] * signal_length
        for idx in button:
            if 0 <= idx < len(flags):
                flags[idx] = True

        l = list(flags)
        mask_list[list_to_int(l)] = button
    return mask_list

with PerformanceMonitor(name="Computation"):
    for index, signal in enumerate(signals):
        available_buttons = buttons[index]
        button_masks = button_to_flag_mask(available_buttons, len(signal))
        buttons_to_press = find_buttons_to_press_optimized(signal, list(button_masks.keys()))
        pressed_buttons = [button_masks[mask] for mask in buttons_to_press]
        answer += len(pressed_buttons)

print("Answer:", answer)