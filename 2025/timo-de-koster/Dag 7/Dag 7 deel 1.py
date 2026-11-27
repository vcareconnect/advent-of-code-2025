beamCoords = []
splits = 0

with open('input.txt', 'r') as file:
    
    first = file.readline()
    beamCoords.append(first.find('S'))

    for line in file:
        newCoords = set()
        removedCoords = []

        for coord in beamCoords:
            if(line[coord] == '^'):
                removedCoords.append(coord)
                left = coord - 1
                right = coord + 1

                if not beamCoords.__contains__(left):
                    newCoords.add(left)
                if not beamCoords.__contains__(right):
                    newCoords.add(right)
                
                splits += 1
        
        for coord in removedCoords:
              beamCoords.remove(coord)
        for coord in newCoords:
             beamCoords.append(coord)
                    
print(f'Total splits: {splits}')