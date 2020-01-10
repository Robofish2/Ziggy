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
    public partial class UserLogOnOff : Form
    {

        string currentUser;
        MainForm mainForm;
        MachineConfig machineParameters;
        /// <summary>
        ///  Property to indicate Log On success 
        /// </summary>
        public bool LogOnSuccess { get { return _los; } set { _los = value; } }
        private bool _los = false;

        /// <summary>
        ///  Property to indicate new user
        /// </summary>
        public string NewUser { get { return _nu; } set { _nu = value; } }
        private string _nu = string.Empty;

        public UserLogOnOff(MainForm mf, string user, MachineConfig mc)
        {
            InitializeComponent();
            currentUser = user;
            mainForm = mf;
            machineParameters = mc;
        }

        private void buttonLogOn_Click(object sender, EventArgs e)
        {

            bool userandPasswordMatched = false;

            if (mainForm.GetUserAccountPassword(comboBoxUserName.Text).Equals(textBoxPassword.Text))
            {
                userandPasswordMatched = true;
            }

            if (userandPasswordMatched)
            {
                NewUser = comboBoxUserName.Text;
                LogOnSuccess = true;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid Password", "Warning");
            }

        }

        private void buttonLogOff_Click(object sender, EventArgs e)
        {

            if (currentUser == "monitor")
            {
                NewUser = "monitor";
            }
            else
            {
                NewUser = machineParameters.StartupUserName;
            }
            LogOnSuccess = true;
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UserLogOnOff_Load(object sender, EventArgs e)
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


            comboBoxUserName.Text = currentUser;
            

            if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) == 1)
            {
                buttonLogOn.Enabled = false;
                buttonLogOff.Enabled = false;
            }
            
        }

        private void comboBoxUserName_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBoxPassword.Enabled = true;
            buttonLogOn.Enabled = true;
            buttonLogOff.Enabled = true;

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if ((textBoxPassword.Text.Length > 0) || (comboBoxUserName.Text == "admin"))
            {
                buttonLogOn.Enabled = true;
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLogOn_Click(null, null);
            }

        }
    }
}
