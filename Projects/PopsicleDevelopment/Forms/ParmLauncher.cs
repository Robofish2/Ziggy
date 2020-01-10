using System;
using System.Windows.Forms;
using Cognex.VisionPro.QuickBuild;
using AbbCom.Forms;

namespace AbbCom
{
    public partial class ParmLauncher : Form
    {
        MainForm mainForm;
        RuntimeParameters runtimeParameters;
        MachineConfig machineParameters;
        CogJobManager mcjmAcq;

        public ParmLauncher(MainForm mf, RuntimeParameters rp, CogJobManager cjm, MachineConfig mc)
        {

            InitializeComponent();

            mainForm = mf;
            runtimeParameters = rp;
            machineParameters = mc;
            mcjmAcq = cjm;


        }

        private void buttonParmLauncherClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            mainForm.StartSystem();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            mainForm.StopSystem(true);
        }

        private void buttonVision_Click(object sender, EventArgs e)
        {
            bool success;
            runtimeParameters = mainForm.DeserializeRuntimeParamters(mainForm.RecipeName + ".xml", out success);
            if (!success)
            {
                MessageBox.Show("Error reading recipe file.", "Warning!");
                return;
            }

            using (Vision vis = new Vision(mainForm, runtimeParameters, mcjmAcq, machineParameters))
            {
                vis.ShowDialog();
            }

            // Force the form to redraw (Paint event will fire)
            mainForm.Invalidate();

        }


        private void buttonStats_Click(object sender, EventArgs e)
        {
            using (Stats st = new Stats(mainForm, runtimeParameters))
            {
                st.ShowDialog();
            }
        }

        /// <summary>
        /// Check for F key presses
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            bool handled = false;        // we haven't handled this key

            if ((keyData == Keys.F5) && (mainForm.RecipeName != string.Empty) && (!mainForm.VisionOnline))
            {
                mainForm.RunQuickBuildJob();
                handled = true;       // we handled the key press
            }
            else if (keyData == Keys.F6)
            {
                handled = true;       // we handled the key press
                Close();
            }

            return (handled || base.ProcessCmdKey(ref msg, keyData));
        }

        private void buttonTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                mainForm.RunQuickBuildJob();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Runtime Exception @buttonTrigger");
            }
        }

        private void ParmLauncher_Load(object sender, EventArgs e)
        {
            if (runtimeParameters.VisMode == "Simulation" || mainForm.VisionOnline)
            {
                buttonManualTrigger.Enabled = false;
            }
            else
            {
                buttonManualTrigger.Enabled = true;
            }

            if (mainForm.VisionOnline)
            {
                buttonStart.Enabled = true;
                buttonStop.Enabled = true;
            }
            else
            {
                buttonStart.Enabled = false;
                buttonStop.Enabled = false;
            }
        }
    }
}
