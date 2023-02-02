using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryService {
    internal class File {
        //Read a file and returns the content in an array.
        public static string[] Input(string FilePath) {
            string[] Content = { };
            try {
                Content = System.IO.File.ReadAllLines(FilePath);
            }
            catch (Exception ex) {
                Console.WriteLine("Reading File: " + ex.Message);
            }
            return Content;
        }
        //Write a file from a array
        public static void Output(string FilePath, string[] OutputContent) {
            try {
                System.IO.File.WriteAllLines(FilePath, OutputContent);
            }
            catch(Exception ex) {
                Console.WriteLine("OutputFiles: " + ex.Message);
            }
        }
    }
}
