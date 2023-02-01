using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryService {
    public class Calculate {
        private static string FileName = string.Empty;
        private static List<Drone> Drones = new List<Drone>();
        private static List<Location> Locations = new List<Location>();
        private static List<DroneTrips> DronesTrips = new List<DroneTrips>();

        private class Drone {
            public string Name { get; set; } = string.Empty;
            public int Capacity { get; set; }
        }
        private class Location {
            public string Name { get; set; } = string.Empty;
            public int Weight { get; set; }
            public bool Covered { get; set; }
        }
        public class DroneTrips {
            public string DroneName { get; set; } = string.Empty;
            public string Trip { get; set; } = string.Empty;
            public string[] Locations { get; set; } = { };
        }
        public static void Process(string InputFile) {
            GetInformation(InputFile);
            CalculatingTripsByWeight();
            WriteResults(InputFile.Replace("input", "output"));
        }
        public static void GetInformation(string FileInput) {
            Console.WriteLine("Reading Input File.....");
            string[] Input = File.Input(FileInput);

            Console.WriteLine("Getting Information.....");
            string[] InputDrones = Input.First().Replace("[", "").Replace("]", "").Split(",");
            for (int i = 0; i < InputDrones.Length; i += 2) {
                Drones.Add(new Drone() { Name = InputDrones[i].Trim(), Capacity = int.Parse(InputDrones[i + 1].Trim()) });
            }

            string[] InputLocations = { };
            for (int i = 1; i < Input.Length; i += 2) {
                InputLocations = Input[i].Replace("[", "").Replace("]", "").Split(",");
                Locations.Add(new Location() { Name = InputLocations[0].Trim(), Weight = int.Parse(InputLocations[1].Trim()), Covered = false });
            }
        }
        public static void CalculatingTripsByWeight() {
            Console.WriteLine("Calculating.....");
            //sort de Drones and Locations.
            Drone[] SortedDrones = Drones.OrderByDescending(x => x.Capacity).ToArray();
            Location[] SortedLocations = Locations.OrderByDescending(x => x.Weight).ToArray();
            int CountTrips;
            List<string> AccLocations = new List<string>(); ;
            int AccWeight;
            for (int i = 0; i < SortedDrones.Length; i++) {
                CountTrips = 0; // CounterTrips
                AccWeight = 0; // Weight Accumulated
                AccLocations = new List<string>(); //Accumulated locations.
                for (int j = 0; j < SortedLocations.Length; j++) {
                    if (SortedLocations[j].Covered) continue; // if a location is covered continue whit next
                    if (SortedDrones[i].Capacity < (AccWeight + SortedLocations[j].Weight)) continue; //Calculate if the drone has the capacity, if not go to the next location.

                    if (SortedDrones[i].Capacity == (AccWeight + SortedLocations[j].Weight)) { // if exist a conicidence add a new Trip for the drone
                        CountTrips += 1;
                        AccLocations.Add(SortedLocations[j].Name); //add to accumulated locations.

                        for (int k = 0; k < SortedLocations.Length; k++) {
                            if (AccLocations.Contains($"[{SortedLocations[k].Name}]")) SortedLocations[k].Covered = true; // set covered the locations accumulated.
                        }
                        // Add to the list of drone trips all the accumulated locations.
                        DronesTrips.Add(new DroneTrips() {
                            DroneName = $"[{SortedDrones[i].Name}]",
                            Trip = $"Trip #{CountTrips}",
                            Locations = AccLocations.ToArray()
                        });
                        AccLocations = new List<string>(); //reset the Accumulated Locations.
                        AccWeight = 0; //reset the accumulated weight.
                        j = 0; //back for another iteration.
                        continue;
                    }
                    AccLocations.Add($"[{SortedLocations[j].Name}]"); // if is not the same capacity or the drone has more capacity then accumulate the location.
                    AccWeight += SortedLocations[j].Weight; // accumulate weight.
                }
            }
        }
        public static void WriteResults(string OutputFile) {
            Console.WriteLine("Writing output.....");
            DroneTrips[] SortedDronesTrips = DronesTrips.OrderBy(x => x.DroneName).ToArray();
            string CurrentDroneName = string.Empty;
            var OutputContent = new List<string>();
            for (int i = 0; i < SortedDronesTrips.Length; i++) {
                if (CurrentDroneName != SortedDronesTrips[i].DroneName) {
                    CurrentDroneName = SortedDronesTrips[i].DroneName;
                    OutputContent.Add(string.Empty);
                    OutputContent.Add(SortedDronesTrips[i].DroneName);
                }
                OutputContent.Add(SortedDronesTrips[i].Trip);
                OutputContent.Add(string.Join(", ", SortedDronesTrips[i].Locations));
            }
            File.Output(OutputFile, OutputContent.ToArray());
        }
    }
}
