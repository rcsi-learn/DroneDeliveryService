using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryService {
    public class Calculate {
        private static List<Drone> Drones;
        private static List<Location> Locations;
        private static List<DroneTrips> DronesTrips;

        //Drone class
        private class Drone {
            public string Name { get; set; } = string.Empty;
            public int Capacity { get; set; }
        }
        //Location class
        private class Location {
            public string Name { get; set; } = string.Empty;
            public int Weight { get; set; }
            public bool Covered { get; set; }
        }
        //DroneTripos calss
        public class DroneTrips {
            public string DroneName { get; set; } = string.Empty;
            public string Trip { get; set; } = string.Empty;
            public string[] Locations { get; set; } = { };
        }
        //General process 
        public static void Process(string InputFile) {
            GetInformation(InputFile);
            CalculatingTripsByWeight();
            OutputResults(InputFile.Replace("input", "output"));
        }
        //Method for get Information from file.
        private static void GetInformation(string FileInput) {
            Drones = new List<Drone>();
            Locations = new List<Location>();

            Console.WriteLine("Reading Input File.....");
            string[] Input = File.Input(FileInput);

            Console.WriteLine("Getting Information.....");
            string[] InputDrones = Input.First().Replace("[", "").Replace("]", "").Split(",");
            for (int i = 0; i < InputDrones.Length; i += 2) {
                Drones.Add(new Drone() { Name = InputDrones[i].Trim(), Capacity = int.Parse(InputDrones[i + 1].Trim()) });
            }

            string[] InputLocations = { };
            for (int i = 1; i < Input.Length; i++) {
                InputLocations = Input[i].Replace("[", "").Replace("]", "").Split(",");
                Locations.Add(new Location() { Name = InputLocations[0].Trim(), Weight = int.Parse(InputLocations[1].Trim()), Covered = false });
            }
        }

        //Method for drone trips calculation.
        private static void CalculatingTripsByWeight() {
            DronesTrips = new List<DroneTrips>();
            Console.WriteLine("Calculating.....");
            //Sort de Drones by Capacity
            Drone[] SortedDrones = Drones.OrderByDescending(x => x.Capacity).ToArray();
            //Sort de Locations by Weight
            Location[] SortedLocations = Locations.OrderByDescending(x => x.Weight).ToArray();
            //Auxiliar list for accumulate Locations for drone trips
            List<string> AccLocations = new List<string>(); ;
            //Auxiliar int for accumulate Weight
            int AccWeight = 0;
            //Iterate until all every location until all are covered
            while (SortedLocations.Any(x => x.Covered == false)) {
                //Find locations combination order by weight and look if exist a drone with the accumulate wheight
                for (int i = 0; i < SortedLocations.Length; i++) {
                    //If the location is covered go to the next
                    if (SortedLocations[i].Covered) continue;
                    //If exists a drone with aviable capacty iterate again
                    //In the first itearation will the first SortedLocation will be added to AccLocations.
                    if (SortedDrones.Any(x => x.Capacity > AccWeight + SortedLocations[i].Weight)) {
                        AccLocations.Add(SortedLocations[i].Name);
                        AccWeight += SortedLocations[i].Weight;
                        continue;
                    }
                    //If NOT exist a drone with aviable capacity maybe a drone with the same capacity
                    if (Drones.Any(x => x.Capacity == AccWeight + SortedLocations[i].Weight)) {
                        AccLocations.Add(SortedLocations[i].Name);
                        AccWeight += SortedLocations[i].Weight;
                        break;
                    }
                    //If AccWeight + the current location exceed the drones capacity try with the next Location.
                }
                //Look for the optimal drone in the SortedDrones array.
                Drone OptDrone = SortedDrones[0];
                for (int i = 0; i < SortedDrones.Length; i++) {
                    //if the Capacity exceed the AccWeight try with the next
                    if (SortedDrones[i].Capacity > AccWeight) OptDrone = SortedDrones[i];
                    //if the Capacity are equals to AccWeight  is the optimus drone
                    else if (SortedDrones[i].Capacity == AccWeight) {
                        OptDrone = SortedDrones[i];
                        break;
                    }
                    //AccWeight is more than the capcity, use the last OptDrone saved.
                    else break;
                }

                //Add to the List of Drone  Trips
                DronesTrips.Add(new DroneTrips() {
                    DroneName = $"[{OptDrone.Name}]",//Save the Drone name with format
                    Trip = $"Trip #{DronesTrips.FindAll(x => x.DroneName == $"[{OptDrone.Name}]").Count() + 1}", //Count the Trips saved of the drone and add 1 (the current trip)
                    Locations = AccLocations.ToArray() //Save the accumulated Locations.
                });

                //turn to covered true to all locations accumulated for the next iteration
                for (int i = 0; i < SortedLocations.Length; i++) {
                    if (AccLocations.Contains(SortedLocations[i].Name)) SortedLocations[i].Covered = true;
                }
                //reset the Accumulated Locations.
                AccLocations = new List<string>();
                //reset the accumulated weight.
                AccWeight = 0;
            }
        }
        //Method for the result.
        private static void OutputResults(string OutputFile) {
            Console.WriteLine("Format output.....");
            DroneTrips[] SortedDronesTrips = DronesTrips.OrderBy(x => x.DroneName).ToArray();
            string CurrentDroneName = string.Empty;
            var OutputContent = new List<string>();
            //Add to OutputContent List in the correct order
            for (int i = 0; i < SortedDronesTrips.Length; i++) {
                if (CurrentDroneName != SortedDronesTrips[i].DroneName) {
                    CurrentDroneName = SortedDronesTrips[i].DroneName;
                    OutputContent.Add(string.Empty);
                    OutputContent.Add(SortedDronesTrips[i].DroneName);
                }
                OutputContent.Add(SortedDronesTrips[i].Trip);
                OutputContent.Add(string.Join(", ", SortedDronesTrips[i].Locations));
            }
            Console.WriteLine("Write output.....");
            File.Output(OutputFile, OutputContent.ToArray());
        }
    }
}
