using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace AbbCom.Forms
{

    public partial class Engineering : Form
    {

        AbbCom.Client logClient;

        public delegate void PropertyChangedHeartbeatControlEventHandler(string _efgc);
        public event PropertyChangedHeartbeatControlEventHandler HeartbeatEvent;

        /// <summary>
        ///  Property to enable Frame Grabber check when running QuickBuild Job.
        /// </summary>
        public bool EnableFrameGrabberCheck { get { return _efgc; } set { _efgc = value; } }
        private bool _efgc = false;

        /// <summary>
        /// Property used to fire event to inform subscriber to start/stop the heartbeat timer 
        /// </summary>
        public string HeartbeatTimerControl
        {
            get { return _hbm; }
            set
            {
                _hbm = value;
                if (this.HeartbeatEvent != null)
                {
                    this.HeartbeatEvent(_hbm);      // Fire event for subscribers
                }
            }
        }
        private string _hbm = string.Empty;

        MachineConfig machineParameters;
        RuntimeParameters runtimeParameters;
        MainForm mainForm;

        private string lastBlobName = string.Empty;
        private string lastPatternName = string.Empty;

        public Engineering(AbbCom.Client lc, MachineConfig mc, RuntimeParameters rp, MainForm mf)
        {
            logClient = lc;
            machineParameters = mc;
            runtimeParameters = rp;
            mainForm = mf;
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

            if (string.Compare(lastBlobName, textBoxCogJobBlob.Text) != 0 || string.Compare(lastPatternName, textBoxCogJobPattern.Text) != 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNo))
                {
                    runtimeParameters.VisCogJobBlobName = textBoxCogJobBlob.Text;
                    runtimeParameters.VisCogJobPatternName = textBoxCogJobPattern.Text;
                    mainForm.SerializeRuntimeParameters(runtimeParameters);
                }
            }

            Close();
        }

        private void buttonStartLogger_Click(object sender, EventArgs e)
        {

            if (!Maf.Tools.Utilities.IsProcessRunning("PopsicleDataLogger"))
            {
                string path = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                    "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs", "Engineering");

                if (!File.Exists(path + "\\PopsicleDataLogger.exe"))
                {
                    MessageBox.Show("The file '" + "PopsicleDataLogger.exe' Not Found at " + path + " \n\nApplication must be installed on target machine.");
                    return;
                }
                Process.Start(@path + "\\PopsicleDataLogger.exe");
                System.Threading.Thread.Sleep(100);
            }

            logClient.StartClient();
            System.Threading.Thread.Sleep(100);
            buttonStartLogger.Enabled = false;
            buttonStopLogger.Enabled = true;
            buttonSend.Enabled = true;
            logClient.SendMessage(false, "Data logger started");
            this.HeartbeatTimerControl = "StartHeartbeatTimer";

        }
        private void buttonStopLoggerThread_Click(object sender, EventArgs e)
        {

            logClient.SendMessage(false, "Data logger stopped");
            System.Threading.Thread.Sleep(100);

            Process[] myProcesses = Process.GetProcessesByName("PopsicleDataLogger");
            if (Maf.Tools.Utilities.IsProcessRunning("PopsicleDataLogger"))
            {
                foreach (Process myProcess in myProcesses)
                {
                    myProcess.CloseMainWindow();
                }
            }

            logClient.StopClient();
            buttonStartLogger.Enabled = true;
            buttonStopLogger.Enabled = false;
            buttonSend.Enabled = false;
            this.HeartbeatTimerControl = "StopHeartbeatTimer";
        }

        private void Engineering_Load(object sender, EventArgs e)
        {

            bool success;
            runtimeParameters = mainForm.DeserializeRuntimeParamters(mainForm.RecipeName + ".xml", out success);
            if (!success)
            {
                MessageBox.Show("Error reading recipe file.", "Warning!");
                return;
            }

            if (logClient.Running)
            {
                buttonStartLogger.Enabled = false;
                buttonStopLogger.Enabled = true;
                buttonSend.Enabled = true;
            }
            else
            {
                buttonStartLogger.Enabled = true;
                buttonStopLogger.Enabled = false;
                buttonSend.Enabled = false;
            }

            textBoxCogJobBlob.Text = runtimeParameters.VisCogJobBlobName;
            textBoxCogJobPattern.Text = runtimeParameters.VisCogJobPatternName;
            lastBlobName = textBoxCogJobBlob.Text;
            lastPatternName = textBoxCogJobPattern.Text;
            textBoxCogJobBlob.Enabled = false;
            textBoxCogJobPattern.Enabled = false;
            buttonSave.Enabled = false;
            groupBoxDataLogger.Enabled = false;
            checkBoxCogJobEnable.Enabled = false;

            if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
            {
                groupBoxDataLogger.Enabled = true;
                checkBoxCogJobEnable.Enabled = true;
            }

            // Disable certain items if vision is online
            if (mainForm.VisionOnline)
            {
                checkBoxCogJobEnable.Enabled = false;
            }

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (logClient.Running && logClient.Connected)
            {
                logClient.SendMessage(false, textBoxMessageText.Text);
            }
        }

        private void checkBoxCogJobEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCogJobEnable.Checked)
            {
                textBoxCogJobBlob.Enabled = true;
                textBoxCogJobPattern.Enabled = true;
                buttonSave.Enabled = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNo))
            {
                runtimeParameters.VisCogJobBlobName = textBoxCogJobBlob.Text;
                runtimeParameters.VisCogJobPatternName = textBoxCogJobPattern.Text;
                mainForm.SerializeRuntimeParameters(runtimeParameters);
                lastBlobName = textBoxCogJobBlob.Text;
                lastPatternName = textBoxCogJobPattern.Text;
            }
            else
            {
                textBoxCogJobBlob.Text = lastBlobName;
                textBoxCogJobPattern.Text = lastPatternName;
            }

        }

    }
}
