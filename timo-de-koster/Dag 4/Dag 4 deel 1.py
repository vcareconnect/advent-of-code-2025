total = 0
lines = []

with open('input.txt', 'r') as file:
    for line in file:
        lines.append(line.strip())

length = len(lines[0])
curLines = [str('.'*length), str('.'*length), lines[0]]

for idx, line in enumerate(lines):

    curLines[0] = curLines[1]
    curLines[1] = curLines[2]
    
    if (idx == len(lines)-1):
        curLines[2] = str('.'*length)
    else:
        curLines[2] = lines[idx+1]

    for index, char in enumerate(curLines[1]):
        if(char == '.'):
            continue
        perimeter = []
        count = 0

        perimeter.append(curLines[0][index])
        perimeter.append(curLines[2][index])

        if(index > 0):
            perimeter.append(curLines[0][index-1])
            perimeter.append(curLines[1][index-1])
            perimeter.append(curLines[2][index-1])

        if(index < length-1):
            perimeter.append(curLines[0][index+1])
            perimeter.append(curLines[1][index+1])
            perimeter.append(curLines[2][index+1])

        for c in perimeter:
            if c == '@':
                count += 1

        if count < 4:
            total += 1

print(f"Total: {total}")