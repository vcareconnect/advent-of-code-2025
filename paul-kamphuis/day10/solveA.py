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

asnwer = 0


with PerformanceMonitor(name="Computation"):
    # convert each entry in buttons to a list of flags indicating of length of the corresponding signal indicating if the button toggles a flag when pressed
    button_flags_per_button = []
    for signal, button_groups in zip(signals, buttons):
        per_button_flags = []
        for group in button_groups:
            flags = [False] * len(signal)
            for idx in group:
                if 0 <= idx < len(flags):
                    flags[idx] = True
            per_button_flags.append(flags)
        button_flags_per_button.append(per_button_flags)

    # print("Button Flags Per Button:", button_flags_per_button)


    for index, signal in enumerate(signals):
        buttons = button_flags_per_button[index]
        # how many buttons are there    
        # print("signal:", signal)
        for n in range(1, len(buttons)+1):
            found = False
            if n == 0:
                continue
            use_buttons = itertools.permutations(buttons, n)
            # print(f"Number of button sequences for n={n}: {len(list(use_buttons))}")
            use_buttons = itertools.permutations(buttons, n)  # recreate iterator after consuming
            for button in use_buttons:
                state = [False] * len(signal)
                # xor each flag state with the flag in button
                # print(f"Trying button sequence: {len(button)}")
                for btn in button:
                    # print(f"  Pressing button: {btn}")
                    for i in range(len(signal)):
                        state[i] = state[i] ^ btn[i]
                    # print(f"    State after pressing: {state}")
                if state == signal:
                    button_sequence = list(button)
                    # print(f"Found matching button sequence for signal {signal}: {n}")
                    asnwer += n
                    found = True
                    break
            if found:
                break

print("Answer:", asnwer)
