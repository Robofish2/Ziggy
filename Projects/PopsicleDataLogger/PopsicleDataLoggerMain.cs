using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PopsicleDataLogger
{
    public partial class PopsicleDataLoggerMain : Form
    {
        AbbCom.Server srv;

        const int MAX_APP_LOG_ENTRIES = 1000;

        string LogPath = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                   "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs");

        public PopsicleDataLoggerMain()
        {
            InitializeComponent();

            srv = new AbbCom.Server();

            // Subscribe to Server events
            srv.ServerStatusMessageUpdate += new EventHandler<AbbCom.ServerCustomEventArgs>(Server_ServerStatusMessageUpdate);
            srv.ServerTCP_MessageUpdate += new EventHandler<AbbCom.ServerCustomEventArgs>(Server_ServerTCP_MessageUpdate);
            srv.ServerConnected += new EventHandler<AbbCom.ServerCustomEventArgs>(Server_ServerConnected);

        }

        /// <summary>
        /// Custom event when OnServerStatus_MessageUpdate is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Server_ServerStatusMessageUpdate(object sender, AbbCom.ServerCustomEventArgs args)
        {
            EnterIntoLogWindow("DataLog: ", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss ") + ": ", args.Message);

            string s = string.Concat("DataLog:", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss "), ": ", args.Message);
            Maf.Tools.DiskLogger.LogData(LogPath, s);

        }
        /// <summary>
        /// Custom event when OnServerTCP_MessageUpdate is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Server_ServerTCP_MessageUpdate(object sender, AbbCom.ServerCustomEventArgs args)
        {
        }

        /// <summary>
        /// Custom event when OnServerConnected is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Server_ServerConnected(object sender, AbbCom.ServerCustomEventArgs args)
        {
            // Force the form to redraw
            this.Invalidate();

        }
        private void EnterIntoLogWindow(string name, string time, string msg)
        {
            //check that size hasn't been exceeded and truncate if necessary
            if (listViewEthernetMsgs.Items.Count > MAX_APP_LOG_ENTRIES)
            {
                listViewEthernetMsgs.Items.RemoveAt(0);
            }

            //make entry           
            string[] entry = { name, time, msg };
            ListViewItem newItem = new ListViewItem(entry);
            listViewEthernetMsgs.Items.Add(newItem);
        }

        private void PopsicleDataLoggerMain_Load(object sender, EventArgs e)
        {
            srv.ListenPort = 1235;
            srv.StartServer();
        }

    }
}
