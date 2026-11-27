points = []
max_area = 0

with open('input.txt', 'r') as file:
    for line in file:
        left, right = line.split(',')
        points.append((int(left), int(right)))

#ik kon niks beters verzinnen dan een brute force oplossing helaas
#gewoon alle punten met elkaar vergelijken, als je een groter gebied vindt dan update je max_area
for point1 in points:
    for point2 in points:
        if point1 == point2:
            continue

        #met de test input kreeg ik als max 36 (4x9) maar moest eig 50 zijn
        #snap nog niet helemaal waarom maar beide afstanden moeten dus +1 hebben
        area = (abs(point1[0]-point2[0])+1) * (abs(point1[1]-point2[1])+1)
        
        if area > max_area:
            max_area = area

print(max_area)