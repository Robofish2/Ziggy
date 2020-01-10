using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maf.Tools;

namespace AbbCom.Forms
{
    public partial class UserAccessSettings : Form
    {

        MainForm mainForm;
        MachineConfig machineParameters;
        string selectedUser = string.Empty;

        public UserAccessSettings(MainForm mf, MachineConfig mc)
        {
            InitializeComponent();

            mainForm = mf;
            machineParameters = mc;
        }

        private void listView1_Click(object sender, EventArgs e)
        {

            ListViewItem item;
            item = listView1.FocusedItem;
            buttonEdit.Enabled = false;
            buttonAdd.Enabled = false;
            buttonDelete.Enabled = false;

            selectedUser = listView1.SelectedItems[0].Text;

            if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
            {
                if ((selectedUser == "bpr") && (mainForm.CurrentLoggedOnUser != "bpr"))
                {
                    buttonAdd.Enabled = true;
                    buttonEdit.Enabled = false;
                }
                else
                {
                    buttonAdd.Enabled = true;
                    buttonEdit.Enabled = true;

                    // Don't allow some users to be deleted.
                    if (selectedUser == "monitor" || selectedUser =="bpr" || selectedUser == "admin")
                    {
                        buttonDelete.Enabled = false;
                    }
                    else
                    {
                        buttonDelete.Enabled = true;
                    }
                }
            }

        }

        private void userAccessSettings_Load(object sender, EventArgs e)
        {

            buttonEdit.Enabled = false;
            buttonAdd.Enabled = false;
            buttonDelete.Enabled = false;

            if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
            {
                buttonAdd.Enabled = true;
                buttonDelete.Enabled = true;
            }

            populateListview();

        }

        private void populateListview()
        {
            //Add items in the listview
            string[] arr = new string[2];
            ListViewItem itm;

            listView1.Items.Clear();

            // Populate the listview
            try
            {
                for (int i = 0; i < machineParameters.UserAccountName.Length; i++)
                {
                    if (machineParameters.UserAccountName[i] != null)
                    {
                        if (!machineParameters.UserAccountName[i].Equals(string.Empty))
                        {
                            arr[0] = machineParameters.UserAccountName[i];
                            arr[1] = mainForm.GetUserAccountLevelName(mainForm.GetUserAccountLevel(machineParameters.UserAccountName[i]));
                            itm = new ListViewItem(arr);
                            listView1.Items.Add(itm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {

            using (FormUserSetPassword u = new FormUserSetPassword(mainForm, machineParameters, selectedUser))
            {
                u.ShowDialog();

                populateListview();
            }

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            UserAccessAdd uaa = new UserAccessAdd();
            uaa.ShowDialog();

            if (uaa.AddSuccess)
            {
                try
                {
                    int nextIndex = mainForm.GetNextUserAccountIndex();

                    machineParameters.UserAccountName[nextIndex] = uaa.NewUser;
                    machineParameters.UserAccountPassword[nextIndex] = string.Empty;
                    machineParameters.UserAccountLevel[nextIndex] = 1;

                    populateListview();

                    mainForm.SaveMachineParameters(machineParameters);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

            if ((DialogResult.Yes != Utilities.AreYouSure()))
            {
                return;
            }

            int index = mainForm.GetUserAccountIndex(selectedUser);
            machineParameters.UserAccountName[index] = string.Empty;
            populateListview();
        }
    }
}
