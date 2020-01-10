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
    public partial class FormUserSetPassword : Form
    {

        MainForm mainForm;
        MachineConfig machineParameters;
        string user;
        Icon warn;
        Icon blank;
        bool toggle = false;
        bool iconHit = false;
        Point iconPoint = new Point(325, 90);

        public FormUserSetPassword(MainForm mf, MachineConfig mc, string u)
        {
            InitializeComponent();

            mainForm = mf;
            machineParameters = mc;
            user = u;
        }

        private void UserSetPassword_Load(object sender, EventArgs e)
        {

            textBoxUser.Text = user;
            textBoxPassword.Text = mainForm.GetUserAccountPassword(user);
            textBoxVerifyPassword.Text = mainForm.GetUserAccountPassword(user);
            int level = mainForm.GetUserAccountLevel(user);
            comboBoxAccesLevel.Text = comboBoxAccesLevel.Items[level - 1].ToString();

            checkBoxShowPassword.Enabled = false;
            if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
            {
                checkBoxShowPassword.Enabled = true;
            }

            try
            {
                warn = new Icon(Application.StartupPath + "\\Warning.ico");
                blank = new Icon(Application.StartupPath + "\\Blank.ico");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            if (textBoxPassword.Text == textBoxVerifyPassword.Text)
            {

                int index = mainForm.GetUserAccountIndex(user);

                machineParameters.UserAccountName[index] = user;
                machineParameters.UserAccountPassword[index] = textBoxVerifyPassword.Text;
                machineParameters.UserAccountLevel[index] = comboBoxAccesLevel.SelectedIndex + 1;

                mainForm.SaveMachineParameters(machineParameters);
                Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match!");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UserSetPassword_Paint(object sender, PaintEventArgs e)
        {
            if (!textBoxPassword.Text.Equals(textBoxVerifyPassword.Text))
            {
                if (iconHit)
                {
                    toolTip1.SetToolTip(this, "Passwords do not match!");
                }

                toggle = !toggle;
                if (toggle)
                {
                    e.Graphics.DrawIcon(warn, iconPoint.X, iconPoint.Y);
                }
                else
                {
                    e.Graphics.DrawIcon(blank, iconPoint.X, iconPoint.Y);
                }
                e.Dispose();
                timer1.Start();
            }
            else
            {
                toolTip1.SetToolTip(textBoxVerifyPassword, "");
                e.Graphics.DrawIcon(blank, iconPoint.X, iconPoint.Y);
                e.Dispose();
                timer1.Stop();
            }

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void textBoxVerifyPassword_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            this.Invalidate();

        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
            {
                textBoxPassword.PasswordChar = '\0';
                textBoxPassword.Text = mainForm.GetUserAccountPassword(user);
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
            }

        }

        private void FormUserSetPassword_MouseMove(object sender, MouseEventArgs e)
        {
            //Text = e.Location.X + ":" + e.Location.Y; 

            if (e.Location.X < iconPoint.X + 32 && e.Location.X > iconPoint.X &&
                e.Location.Y < iconPoint.Y + 32 && e.Location.Y > iconPoint.Y)
            {
                if (!textBoxPassword.Text.Equals(textBoxVerifyPassword.Text))
                {
                    iconHit = true;
                    this.Invalidate();
                }
            }
            else
            {
                iconHit = false;
                toolTip1.RemoveAll();
            }
        }



    }
}
