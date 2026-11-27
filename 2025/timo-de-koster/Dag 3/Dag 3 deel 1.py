voltages = []
total = 0

with open('input.txt', 'r') as file:
    for line in file:

        #er zit nog een \n achter die wel even weg mag
        line = line.strip()

        firstLargest = 0
        secondLargest = 0

        #opslaan waar ik het eerste grote getal heb gevonden
        indexFirst = 0

        for index, char in enumerate(line):

            #laatste getal in de reeks kan nooit de eerste helft van mijn voltage zijn
            if index == len(line) -1:
                break

            #zoek zoek
            if int(char) > firstLargest:
                firstLargest = int(char)
                indexFirst = index

                #als je een 9 hebt gevonden hoef je sowieso niet verder te zoeken, dat is het hoogst mogelijke
                if firstLargest == 9:
                    break

        #doorgaan met zoeken naar het tweede getal in het overgebleven stukje van de reeks
        for char in line[indexFirst+1:]:
            if int(char) > secondLargest:
                secondLargest = int(char)

                if secondLargest == 9:
                    break

        #plak plak, cast cast
        voltage = int(str(firstLargest) + str(secondLargest))
        voltages.append(voltage)

#optelsom
for v in voltages:
    total += v

#presto
print(f"Total: {total}")