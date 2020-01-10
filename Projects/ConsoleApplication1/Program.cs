using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
            "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs");

            string[] allFiles = Directory.GetFiles(path, "*.*");
            string myFile;

            foreach (string item in allFiles)
            {
                myFile = Path.GetFileName(item.ToString());
                File.Copy(string.Concat(path, "\\", myFile), string.Concat(@"C:\temp\", myFile), true);
            }

        }
    }
}
