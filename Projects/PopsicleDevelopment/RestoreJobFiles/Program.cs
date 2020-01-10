using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RestoreJobFiles
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string path = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs");

                string[] allFiles = Directory.GetFiles(@"C:\MikeTemp\", "*.*");
                string myFile;


                foreach (string item in allFiles)
                {
                    myFile = Path.GetFileName(item.ToString());
                    File.Copy(string.Concat(@"C:\MikeTemp\", myFile), string.Concat(path, "\\", myFile), true);
                }

                Directory.Delete(@"C:\MikeTemp");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in 'RestoreJobFiles' :" + ex.ToString());
                Console.Read();
            }
        }
    }
}
