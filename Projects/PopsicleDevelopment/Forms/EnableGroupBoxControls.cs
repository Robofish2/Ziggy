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
    public partial class EnableGroupBoxControls : Form
    {

        public bool ShowContols { get { return _sc; } set { _sc = value; } }
        private bool _sc;

        public EnableGroupBoxControls()
        {
            InitializeComponent();
        }

        private void checkBoxEnableFields_CheckedChanged(object sender, EventArgs e)
        {
            ShowContols = checkBoxEnableFields.Checked;
        }

        private void button1Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
