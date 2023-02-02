Console.WriteLine("*************     Drone Delivery Service     **************");
Console.WriteLine("");

string FilesPath = "c:\\input\\";

//look for input and output folders
if (!Directory.Exists(FilesPath)) Directory.CreateDirectory(FilesPath);
if (!Directory.Exists(FilesPath.Replace("input", "output"))) Directory.CreateDirectory(FilesPath.Replace("input", "output"));

Console.WriteLine("Put the input file in: " + FilesPath);
Console.WriteLine("when you are ready press any key....");
Console.ReadKey();

//Look for all files in the input folder.
IEnumerable<string> InputFolder = Directory.EnumerateFiles(FilesPath);
if (InputFolder is null || InputFolder.Count() == 0) {
    Console.WriteLine("Input Folder Empty.");
    return;
}

//Process every file in folder.
foreach (var file in InputFolder) {
    Console.Clear();
    Console.WriteLine("Current File: " + file);
    DroneDeliveryService.Calculate.Process(file);
}

Console.WriteLine("Output folder: " + FilesPath.Replace("input", "output"));
Console.WriteLine("Press any key to exit.....");
Console.ReadKey();
