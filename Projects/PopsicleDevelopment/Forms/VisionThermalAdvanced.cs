using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AbbCom.Forms
{
    public partial class VisionThermalAdvanced : Form
    {

        MainForm mainform;
        MachineConfig machineParameters;

        public VisionThermalAdvanced(MainForm mf, MachineConfig mc)
        {
            InitializeComponent();
            mainform = mf;
            machineParameters = mc;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

            mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.DisableNUC);
            mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.PerformNUC);

            // Serialize to update machine parameters;
            mainform.SaveMachineParameters(machineParameters);

            Close();
        }

        private void buttonEnableNuc_Click(object sender, EventArgs e)
        {
            mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.EnableNUC);
        }

        private void buttonDisableNUC_Click(object sender, EventArgs e)
        {
            mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.DisableNUC);
        }

        private void buttonPerformNUC_Click(object sender, EventArgs e)
        {
            mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.PerformNUC);
        }

        private void VisionThermalAdvanced_Load(object sender, EventArgs e)
        {

            numericUpDownMinFrameCount.Value = machineParameters.ThermalMinFrameCountForNUC;
            numericUpDownManualNUCtime.Value = machineParameters.ManualNUCtimeMinutes;

            if (machineParameters.EnableThermalAutoNUC)
            {
                checkBoxEnableAutoNUC.Checked = true;
              
                numericUpDownManualNUCtime.Enabled = false;
                numericUpDownMinFrameCount.Enabled = false;
                buttonPerformNUC.Enabled = false;
            }
            else
            {
                checkBoxEnableAutoNUC.Checked = false;

                numericUpDownManualNUCtime.Enabled = true;
                numericUpDownMinFrameCount.Enabled = true;
                buttonPerformNUC.Enabled = true;
            }


        }

        private void numericUpDownMinFrameCount_ValueChanged(object sender, EventArgs e)
        {
            machineParameters.ThermalMinFrameCountForNUC = (int)numericUpDownMinFrameCount.Value;
        }

        private void numericUpDownManualNUCtime_ValueChanged(object sender, EventArgs e)
        {
            machineParameters.ManualNUCtimeMinutes = (int)numericUpDownManualNUCtime.Value;
        }

        private void checkBoxEnableAutoNUC_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnableAutoNUC.Checked)
            {
                machineParameters.EnableThermalAutoNUC = true;
                mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.EnableNUC);

                numericUpDownManualNUCtime.Enabled = false;
                numericUpDownMinFrameCount.Enabled = false;
                buttonPerformNUC.Enabled = false;
            }
            else
            {
                machineParameters.EnableThermalAutoNUC = false;
                mainform.FlirNUC_operation((int)MainForm.FLIR_Operation.DisableNUC);

                numericUpDownManualNUCtime.Enabled = true;
                numericUpDownMinFrameCount.Enabled = true;
                buttonPerformNUC.Enabled = true;
            }
        }

        private void button1AutoFocus_Click(object sender, EventArgs e)
        {
            mainform.FlirAutoFocus();

        }
    }
}
