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
    public partial class SystemOptions : Form
    {

        MainForm mainForm;
        MachineConfig machineParameters;
        MachineConfig restoreParameters;

        bool saveParameters = false;
        bool normalExit = false;

        public SystemOptions(MainForm mf, MachineConfig mc)
        {
            InitializeComponent();

            mainForm = mf;
            machineParameters = mc;
            restoreParameters = mc.ShallowCopy();
        }

        private void SystemOptions_Load(object sender, EventArgs e)
        {

            // Populate the comboBox
            for (int i = 0; i < machineParameters.UserAccountName.Length; i++)
            {
                if (machineParameters.UserAccountName[i] == null)
                {
                    continue;
                }
                if (machineParameters.UserAccountName[i].Equals(string.Empty))
                {
                    continue;
                }
                comboBoxUserName.Items.Add(machineParameters.UserAccountName[i]);
            }

            // Populate the fields
            textBoxMachineName.Text = machineParameters.MachineName;
            comboBoxUserName.Text = machineParameters.StartupUserName;
            textBoxDefaultBackupPath.Text = machineParameters.DefaultBackupPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            normalExit = true;
            saveParameters = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            normalExit = true;
            Close();
        }

        private void SystemOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool success;

            if (!normalExit)
            {
                if (DialogResult.Yes == MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNo))
                {
                    saveParameters = true;
                }
            }

            if (saveParameters)
            {
                machineParameters.StartupUserName = comboBoxUserName.Text;
                machineParameters.MachineName = textBoxMachineName.Text;
                machineParameters.DefaultBackupPath = textBoxDefaultBackupPath.Text;

                machineParameters.SerializeMachineConfigParameters(machineParameters, out success);
                if (!success) { Environment.Exit(0); }
            }
            else
            {
                machineParameters.SerializeMachineConfigParameters(restoreParameters, out success);
                if (!success) { Environment.Exit(0); }

            }
        }

        private void buttonSelectDefaultBackupPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.RootFolder = Environment.SpecialFolder.MyComputer;
            fb.ShowNewFolderButton = false;
            fb.ShowDialog();

            if (fb.SelectedPath == "") { return; }
            textBoxDefaultBackupPath.Text = fb.SelectedPath;
        }

        private void buttonRestoreDefaultBackupPath_Click(object sender, EventArgs e)
        {
            machineParameters.DefaultBackupPath = machineParameters.RestoredDefaultBackupPath;
            textBoxDefaultBackupPath.Text = machineParameters.DefaultBackupPath;

        }
    }
}
