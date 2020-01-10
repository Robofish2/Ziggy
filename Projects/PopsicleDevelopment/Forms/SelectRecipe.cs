using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AbbCom.Forms
{
    public partial class SelectRecipe : Form
    {

        /// <summary>
        ///  RecipeName Property.
        /// </summary>
        public string SelectedRecipe { get { return _sr; } set { _sr = value; } }
        private string _sr = "";

        MainForm mainForm;
        MachineConfig machineParamters;

        public SelectRecipe(MainForm mf, MachineConfig mc)
        {
            InitializeComponent();

            mainForm = mf;
            machineParamters = mc;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SelectedRecipe = string.Empty;
            Close();
        }

        private void SelectRecipe_Load(object sender, EventArgs e)
        {

            try
            {
                // Populate the combobox
                string[] files = Directory.GetFiles(mainForm.JobsPath, "*.vpp");
                foreach (string s in files)
                {
                    comboBox1.Items.Add(Path.GetFileNameWithoutExtension(s));
                }

                if (machineParamters.DefaultRecipe == string.Empty)
                {
                    if (comboBox1.Items.Count > 0)
                    {
                        comboBox1.Text = comboBox1.Items[0].ToString(); //Default to first entry in list
                    }
                    else
                    {
                        comboBox1.Text = string.Empty;

                        string str = "No recipe files exist!. You must place a backup 'vpp' file and 'xml' file\n ";
                        str = string.Concat(str, "in the path: ", mainForm.JobsPath);
                        MessageBox.Show(str, "Error");
                        this.Close();
                    }
                }
                else
                {
                    comboBox1.Text = machineParamters.DefaultRecipe;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectedRecipe = comboBox1.Text;
            Close();

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool handled = false;        // we haven't handled this key

            if (keyData == Keys.F4)
            {
                Close();
                handled = true;       // we handled the key press
            }

            return (handled || base.ProcessCmdKey(ref msg, keyData));
        }
    }
}
