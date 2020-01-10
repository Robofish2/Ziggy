using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace AbbCom
{
    [Serializable]
    public class MachineConfig
    {

        XmlSerializer machineXmlSerializer = null;
        MachineConfig mc;
        string LogPath = string.Empty;
        
        public MachineConfig(string path)
        {
            LogPath = path;
            this.machineXmlSerializer = new XmlSerializer(typeof(MachineConfig));
        }
        public MachineConfig()
        {
            this.machineXmlSerializer = new XmlSerializer(typeof(MachineConfig));
        }
        public MachineConfig ShallowCopy()
        {
            return (MachineConfig)this.MemberwiseClone();
        }

        public string ComPortName = "COM1";
        public int ComPortBaudRate = 9600;
        public int ComPortDataBits = 8;
        public Parity ComPortParity = Parity.None;
        public StopBits ComPortStopBits = StopBits.One;
        public Handshake ComPortHandshake = Handshake.None;
        public bool ComPortEnable = false;
        public int ComPortReadTimeout = 500;
        public int ComPortDataMode = 1;
        public int ComPortPLC_HearBeatFreq = 30000;
        public int ComPortBytesToExpect = 2;
        public int ComSendDelay = 100;
        public int VisRobAngleOffs = 0;
        public int VisArrowAngleOffs = 0;
        public string DefaultRecipe = "";
        public int ThermalMinFrameCountForNUC = 3;
        public int ManualNUCtimeMinutes = 14;
        public bool EnableThermalAutoNUC = false;
        public int PasswordResetTimeoutInMinutes = 15;
        public string StartupUserName = "admin";
        public int AutoThermalEnableTime = 5000;
        public string DefaultBackupPath = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs","Backup\\");
        public string RestoredDefaultBackupPath = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs", "Backup\\");
        public string MachineName = string.Empty;

        public string[] UserAccountName = new string[10];
        public string[] UserAccountPassword = new string[10];
        public int[] UserAccountLevel = new int[10];

        public void SerializeMachineConfigParameters(MachineConfig mc, out bool success)
        {
            TextWriter writeFileStream = null;
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
            "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs", "Config");

            success = false;

            if (!Directory.Exists(path) || !Directory.Exists(@"C:\Program Files\Blueprint Robotics"))
            {
                path = Application.StartupPath + "\\Jobs\\Config";

                // Determine if running from the IDE and change the path
                if (path.Contains("\\bin\\Debug"))
                {
                    path = path.Replace("\\bin\\Debug", "");
                }

                if (path.Contains("\\bin\\Release"))
                {
                    path = path.Replace("\\bin\\Release", "");
                }
            }

            // Serialize the object

            try
            {
                using (writeFileStream = new StreamWriter(path + "\\machineConfig.xml"))
                {
                    XmlWriter writer = XmlWriter.Create(writeFileStream);

                    machineXmlSerializer.Serialize(writer, mc);

                    writeFileStream.Close();

                    success = true;
                }
            }
            catch (FileNotFoundException)
            {
                StringBuilder st = new StringBuilder();
                st.Append("**** Machine Configuration file not found!****\n");
                st.Append("\n");
                st.Append("Ensure the file 'machineConfig.xml exists @path: " + path + "\\");

                MessageBox.Show(st.ToString(), "Error!");
            }
            catch (Exception ex) { MessageBox.Show("Exception @SerializeMachineConfigParamters: " + ex.Message, "Exception Error"); }
            finally
            {
                if(writeFileStream != null)
                    writeFileStream.Close();
            }

        }
        public MachineConfig DeserializeMachineConfigParameters(out bool success)
        {
            FileStream readFileStream=null;
            success = false;

            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
            "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs", "Config");

            if (!Directory.Exists(path)|| !Directory.Exists(@"C:\Program Files\Blueprint Robotics"))
            {
                path = Application.StartupPath + "\\Jobs\\Config";

                // Determine if running from the IDE and change the path
                if (path.Contains("\\bin\\Debug"))
                {
                    path = path.Replace("\\bin\\Debug", "");
                }

                if (path.Contains("\\bin\\Release"))
                {
                    path = path.Replace("\\bin\\Release", "");
                }
            }

            // Deserialize the object
            try
            {
                // Create a new file stream for reading the XML file
                using (readFileStream = new FileStream(path + "\\machineConfig.xml", FileMode.Open))
                {
                    XmlReader reader = XmlReader.Create(readFileStream);

                    // Deserialize object
                    this.mc = (MachineConfig)this.machineXmlSerializer.Deserialize(reader);
                    readFileStream.Close();
                    success = true;

                }
            }
            catch (FileNotFoundException)
            {
                StringBuilder st = new StringBuilder();
                st.Append("**** Machine Configuration file not found!****\n");
                st.Append("\n");
                st.Append("Ensure the file 'machineConfig.xml exists @path: " + path + "\\");

                MessageBox.Show(st.ToString(), "Error!");

            }
            catch (Exception ex) { MessageBox.Show("Exception @DeserializeMachineConfigParamters: " + ex.Message, "Exception Error"); }
            finally
            {
                if(readFileStream != null)
                    readFileStream.Close();
            }

            return mc;
        }

    }
}
