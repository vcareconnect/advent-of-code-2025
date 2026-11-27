import random
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
signals, button_groups, voltages = parse_sample_file('day10/inputA.txt')


def select_buttons(buttons, required_joltage):
    button_list = {}
    for btn in buttons:
        repeat_counts = []
        for index in btn:
            repeat_counts.append(required_joltage[index])
        
        # add button repeat times to the button list
        button_list[btn] = min(repeat_counts)
    return button_list

def get_buttons_per_index(buttons, signal_length):
    index_buttons = [[] for _ in range(signal_length)]
    for btn in buttons:
        for index in btn:
            index_buttons[index].append(btn)
    return index_buttons

answer = 0

def increment_joltage(joltage_state, button, required):
    for i in button:
        joltage_state[i] += 1
        if joltage_state[i] > required[i]:
            return False
    return True

def decrement_joltage(joltage_state, button, count):
    for i in button:
        joltage_state[i] -= count
        if joltage_state[i] < 0:
            joltage_state[i] = 0

def remove_single_use_buttons(buttons, available_presses):
    filtered_buttons = []
    single_voltage_buttons = []
    for btn in buttons:
        if len(btn) != 1:
            if available_presses[btn] > 0:
                filtered_buttons.append(btn)
        else:
            single_voltage_buttons.append(btn)
    return filtered_buttons, single_voltage_buttons


def update_result(result_buttons, btn, count):
    if btn in result_buttons:
        result_buttons[btn] += count
    else:
        result_buttons[btn] = count

def select_impact_button(check_buttons, target_voltage, max_usable):
    # check_buttons, single_press = remove_single_use_buttons(check_buttons, max_usable)
    check_buttons = max_usable.keys()
    p = 1/(len(target_voltage)*sum(target_voltage)) if sum(target_voltage) > 0 else 0
    # p = 1/len(target_voltage)
    if p == 0:
        return None
    max_impact = -1
    selected_button = None
    for btn in check_buttons:
        impact = 0
        if max_usable[btn] == 0:
            continue
        for index in btn:
            impact += target_voltage[index]*p
        if impact == 0:
            continue
        # impact = impact/len(btn)
        if impact > max_impact:
            max_impact = impact
            selected_button = btn
    return selected_button

def estimate_probability(available_buttons, target_voltage):
    s = [btn for btn, count in available_buttons.items() if count > 0]
    probs = {}
    for i in range(len(target_voltage)):
        counter = 0
        for btn in s:   
            if i in btn:
                counter += 1
        probs[i] = counter
    return probs

with PerformanceMonitor(name="Computation"):
    for voltage_idx, voltage in enumerate(voltages):
        result_buttons = {}
        index_buttons = get_buttons_per_index(button_groups[voltage_idx], len(voltage))

        target_voltage = voltage
        while target_voltage != [0] * len(voltage):
            # calculate how many times a button can be pressed to not exceed the target voltage
            max_usable = select_buttons(button_groups[voltage_idx], target_voltage)
            prob = estimate_probability(max_usable, target_voltage)
            found = False
            for index in range(len(voltage)):
                target_joltage = target_voltage[index]
                if target_joltage == 0:
                    continue
                check_buttons = index_buttons[index]
                check_buttons, single_press = remove_single_use_buttons(check_buttons, max_usable)
                if len(check_buttons) == 0:
                    # only single use buttons remain
                    if len(single_press) == 0:
                        print("Failed, No buttons available to press, but target voltage not met!")
                        break
                    btn = single_press[0]
                    min_btn_presses = target_joltage
                    update_result(result_buttons, btn, min_btn_presses)
                    decrement_joltage(target_voltage, btn, min_btn_presses)
                    found = True
                    break
                if len(check_buttons) == 1:
                    btn = check_buttons[0]
                    min_btn_presses = min(target_joltage, max_usable[btn])
                    update_result(result_buttons, btn, min_btn_presses)
                    decrement_joltage(target_voltage, btn, min_btn_presses)
                    found = True
                    break
                for idx, btn in enumerate(check_buttons):
                    max_btn_presses = max_usable[btn]
                    if max_btn_presses == 0:
                        continue
                    # sum max usable for other buttons that affect the same index
                    other_sum = 0
                    for other_btn in check_buttons:
                        if other_btn != btn:
                            other_sum += max_usable[other_btn]
                    if other_sum < target_joltage:
                        if other_sum < target_joltage:
                            # we need to use this button
                            min_btn_presses = target_joltage - other_sum
                            min_btn_presses = min(min_btn_presses, max_btn_presses)
                            update_result(result_buttons, btn, min_btn_presses)
                            decrement_joltage(target_voltage, btn, min_btn_presses)
                            found = True
                            break
                if found:
                    break
            if found == False:
                # no buttons found, pick the first available button and press it once
                btn = select_impact_button(check_buttons, target_voltage, max_usable)
                if btn is None:
                    print("No button selected, but target voltage not met!")
                    break
                update_result(result_buttons, btn, 1)
                decrement_joltage(target_voltage, btn, 1)
                    # break
                # # Sort max_usable by length of key (button) ascending, then by value descending
                # sorted_buttons = sorted(
                #     max_usable.items(),
                #     key=lambda item: (len(item[0]), -item[1])
                # )
                # for btn, count in max_usable.items():
                #     if count > 0:
                #         update_result(result_buttons, btn, 1)
                #         decrement_joltage(target_voltage, btn, 1)
                #         break
        min_button_presses = sum(result_buttons.values())
        answer += min_button_presses
        print(f"Result Buttons: {target_voltage=} with {min_button_presses=}:", result_buttons)

print("Answer:", answer)
