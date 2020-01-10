using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Maf.Tools;
using System.IO;

namespace AbbCom.Forms
{
    /// <summary>
    /// Form class used to display com messages. The intent is to let the window capture messages until the 
    /// predefined message limit is exceeded so the user can view a history of messages.
    /// </summary>
    public partial class ComIO : Form
    {

        const int MAX_APP_LOG_ENTRIES = 3000;

        public ComIO()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Update the Comm I/O window
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void UpdateComWindow(string name, string msg)
        {
            EnterIntoComIOLog(name, DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), msg);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == Utilities.AreYouSure())
                listViewComIOMsgs.Items.Clear();
        }

        /// <summary>
        /// Check for F7 key press and hide the window.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.F7)
            {
                this.Hide();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void saveTextToFile()
        {
            StreamWriter myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (myStream = new StreamWriter(saveFileDialog1.FileName))
                    {
                        if (myStream != null)
                        {
                            for (int i = 0; i < listViewComIOMsgs.Items.Count; i++)
                            {
                                myStream.WriteLine(GetListViewRow(listViewComIOMsgs, i));
                            }

                            myStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Exception @saveTextToFile: " + ex.Message, "Exception Error"); }
            finally
            {
                if (myStream != null)
                {
                    myStream.Close();
                }
            }

        }
        /// <summary>
        /// Return an entire row in a listview.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private string GetListViewRow(ListView list, int currentRow)
        {
            string entireRow = "";
            for (int ii = 0; ii < list.Items[currentRow].SubItems.Count; ii++)
            {
                entireRow += list.Items[currentRow].SubItems[ii].Text + "|";
            }
            entireRow = entireRow.TrimEnd('|');
            return entireRow;
        }
        private void buttonSaveToDisk_Click(object sender, EventArgs e)
        {
            saveTextToFile();
        }

        private void ComIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            // No operation
        }
        private void EnterIntoComIOLog(string name, string time, string msg)
        {
            //check that size hasn't been exceeded and truncate if necessary
            if (listViewComIOMsgs.Items.Count > MAX_APP_LOG_ENTRIES)
            {
                listViewComIOMsgs.Items.RemoveAt(MAX_APP_LOG_ENTRIES);
            }

            //make entry           
            string[] entry = { name, time, msg };
            ListViewItem newItem = new ListViewItem(entry);
            listViewComIOMsgs.Items.Insert(0, newItem);
        }
    }
}
