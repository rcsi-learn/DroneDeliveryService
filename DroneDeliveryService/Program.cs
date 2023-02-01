Console.WriteLine("*************     Drone Delivery Service     **************");
Console.WriteLine("");

string FilesPath = "c:\\input\\";
if (!Directory.Exists(FilesPath)) Directory.CreateDirectory(FilesPath);
if (!Directory.Exists(FilesPath.Replace("input", "output"))) Directory.CreateDirectory(FilesPath.Replace("input", "output"));

IEnumerable<string> InputFolder = Directory.EnumerateFiles(FilesPath);
if (InputFolder is null || InputFolder.Count() == 0) {
    Console.WriteLine("Input Folder Empty.");
    return;
}

foreach (var file in InputFolder) {
    DroneDeliveryService.Calculate.Process(file);
}

Console.ReadKey();
