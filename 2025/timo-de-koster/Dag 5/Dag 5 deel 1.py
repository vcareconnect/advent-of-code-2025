ranges = []
IDs = []

fresh = 0

with open('input.txt', 'r') as file:

    #check of regel leeg is, tot die tijd heb ik te maken met ID ranges
    line = file.readline()
    while line != '\n':
        left, right = line.split('-')
        ranges.append((int(left), int(right)))
        line = file.readline()

    #daarna IDs ophalen
    line = file.readline()
    while line:
        IDs.append(int(line))
        line = file.readline()

for ID in IDs:
    for range in ranges:
        if ID >= range[0] and ID <= range[1]:
            fresh += 1
            break

print(f'amount fresh: {fresh}')