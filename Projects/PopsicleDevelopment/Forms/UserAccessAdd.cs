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
    public partial class UserAccessAdd : Form
    {

        /// <summary>
        ///  Property to indicate Add success 
        /// </summary>
        public bool AddSuccess { get { return _as; } set { _as = value; } }
        private bool _as = false;

        /// <summary>
        ///  Property to indicate new user
        /// </summary>
        public string NewUser { get { return _nu; } set { _nu = value; } }
        private string _nu = string.Empty;

        public UserAccessAdd()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

            NewUser = textBoxNewUser.Text;
            AddSuccess = true;

            Close();

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
