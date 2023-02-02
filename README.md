# Drone Delivery Service
### Explanation
The structre is not complex, apart from the program file, two classes were added, File for reading and writing input/outputs, and the Calculate class, is the important class, where is located the logic.

### Solution
The proposed solution consists on prioritizing get fewest trips and as second using the drones that need the least amount of capacity possible.

### Steps
To achieve this, the proposed solution is the following:
1. Read the information and classify it and bring it to a class structure to be able to operate it easily.
2. Sort the list of drones and locations descending by weight and capacity.
3. Search for many locations as possible per trip, starting with the locations with heaviest packages first and without exceeding the capacity of the drones.
4. Search for the drone with the lowest capacity that can make the trip for each locations trip obtained in the previous step.
6. Save the result obtained in a class structure.
7. Write a solution file.

### Result
Comparing the output obtained with the pdf document output, they are not the same, but they have the same number of trips, and the drones used are the drones with the lower capacity.