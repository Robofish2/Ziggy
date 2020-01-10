
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.FGGigE;
using Cognex.VisionPro.QuickBuild;
using Maf.Tools;
using Cognex.VisionPro.PMAlign;
using Microsoft.Win32;
using Cognex.VisionPro.ToolGroup;

namespace AbbCom.Forms
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {

        # region field definitions...

        // Imports from PInvoke.net
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
             int hWnd,           // window handle
             int hWndInsertAfter,    // placement-order handle
             int X,          // horizontal position
             int Y,          // vertical position
             int cx,         // width
             int cy,         // height
             uint uFlags);       // window positioning flags
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);
        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        private const Int32 MF_BYPOSITION = 0x400;
        private const Int32 MF_REMOVE = 0x1000;

        // Variable used for debugging
#if DEBUG
        public bool formDebug = true;
#else
        public bool formDebug = false;
#endif

        private bool NUCwasDone = false;

        CogJobManager mcjmAcq;
        CogJob mcjAcqGrey;
        CogJobIndependent mcjiAcqGrey;
        private System.Windows.Forms.Button buttonManualTrigger;
        private CogRecordDisplay cogRecFlirLane1Raw;
        private CogRecordDisplay cogRecFlirLane2Raw;
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Allows program to keep track of first pass
        /// </summary>
        private bool FirstPass
        {
            get;
            set;
        }
        /// <summary>
        ///  Name of current selected recipe.
        /// </summary>
        public string RecipeName { get { return _rn; } set { _rn = value; } }
        private string _rn = string.Empty;

        /// <summary>
        ///  Path location for all *.vpp Quickbuild and *.xml Parameter files.
        /// </summary>
        public string JobsPath { get { return _jp; } set { _jp = value; } }
        private string _jp = Path.Combine(Environment.ExpandEnvironmentVariables("%PUBLIC%").ToString(),
                "BluePrint Robotics", "PopsicleImaging", "PopsicleImagingJobs\\");

        /// <summary>
        ///  Property to indicate vision Online status.
        /// </summary>
        public bool VisionOnline { get { return _vol; } set { _vol = value; } }
        private bool _vol = true;

        /// <summary>
        /// Time interval between image acquires.
        /// </summary>
        public int VisionPictureFrequency { get { return _vpf; } set { _vpf = value; } }
        private int _vpf = 1000;

        /// <summary>
        ///  Property to indicate recipe changeover. 
        /// </summary>
        public bool RecipeChangeover { get { return _rc; } set { _rc = value; } }
        private bool _rc = false;

        /// <summary>
        ///  Property to indicate the job is Pattern Match compatible. 
        /// </summary>
        public bool PatternMatchCompatible { get { return _pmc; } set { _pmc = value; } }
        private bool _pmc = false;

        /// <summary>
        /// CogJob Name
        /// </summary>
        public string CogJobName { get { return _cjn; } set { _cjn = value; } }
        private string _cjn = string.Empty;

        /// <summary>
        /// Current User
        /// </summary>
        public string CurrentLoggedOnUser { get { return _cun; } set { _cun = value; } }
        private string _cun = "monitor";

        // Declare Xml serializer variable for serializing
        public XmlSerializer runtimeParmsXmlSerializer;

        // Declare the server objects
        AbbCom.Server R1Server, R2Server, R3Server, R4Server;
        AbbCom.Client LogClient;

        // Declare the MachineConfig class
        MachineConfig machineParameters = null;

        // Initialize
        RuntimeParameters runtimeParameters;
        private CheckBox checkBoxOnLine;
        private Button buttonAdjustParameters;
        private Button buttonShowComIO;

        private int VisItemId = 0;
        private Label label1;
        private Label label2;
        private Button buttonSelectRecipe;
        private string formTitle = "Current Selected Recipe:";
        private Label labelRecipeNotSelected;
        private Label labelSimMode;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem defaultRecipeToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem deleteRecipeToolStripMenuItem;
        private Label labelThermDisabled;
        private ToolStripMenuItem advancedToolStripMenuItem;
        private ToolStripMenuItem saveVisionParametersToQuickBuildToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;

        // Declare the comIO window
        AbbCom.Forms.ComIO comWindow;

        /// <summary>
        /// Robot target.
        /// </summary>
        struct RobTarget
        {
            public double X;
            public double Y;
            public double Z;
            public double Theta;
            public int ID;
        };

        enum RecipeFault
        {
            MissingJobfile = 1,
            DuplicateJobFiles = 2,
            MissingQuickBuildFile = 3,
            GeneralException = 4,
        }

        public enum FLIR_Operation
        {
            EnableNUC,
            DisableNUC,
            PerformNUC
        }


        #region Machine Stats

        /// <summary>
        /// Quickbuild processing time.
        /// </summary>
        public Int64 VisionProcessTime { get { return _vpt; } set { _vpt = value; } }
        private Int64 _vpt = 0;

        /// <summary>
        /// Number of items located.
        /// </summary>
        public int VisionItemsLocated { get { return _vil; } set { _vil = value; } }
        private int _vil = 0;

        /// <summary>
        /// Number of Bar angle failures.
        /// </summary>
        public int VisionAngleFailures { get { return _vaf; } set { _vaf = value; } }
        private int _vaf = 0;

        /// <summary>
        /// Number of Bar Thermal failures.
        /// </summary>
        public int VisionThermalFailures { get { return _vtf; } set { _vtf = value; } }
        private int _vtf = 0;

        #endregion


        /// <summary>
        /// Vision process time.
        /// </summary>
        Stopwatch visionTimer = new Stopwatch();
        public Timer timerPictureTimer;
        public Timer timerFlirAcqFifo;
        private Label label3;
        private Label label4;
        private TextBox textBoxBaslerStatus;
        private TextBox textBoxFlirStatus;
        private ToolStripMenuItem selectRecipeToolStripMenuItem;

        Stopwatch statusUpdateGreyScale = new Stopwatch();
        Stopwatch timerFlirNUCaction = new Stopwatch();
        Stopwatch timerFlirNUCenable = new Stopwatch();
        Stopwatch statusUpdateThermal = new Stopwatch();
        public Timer timerRunRecipeChange;

        private bool runQuickBuildException = false;
        private bool exitApp = false;
        private ToolStripMenuItem engineeringToolStripMenuItem;
        private bool jobManagerInitialized = false;
        private Label labelVisionOffline;
        private Timer timerDataLoggerHeartbeat;
        private int TotalOddCountThisFrame = 0;
        private int TotalEvenCountThisFrame = 0;

        CogFrameGrabberGigEs frameGrabbers = null;
        private ToolStripMenuItem saveRobotDataModulesToolStripMenuItem;
        private ToolStripMenuItem openRecipeFolderToolStripMenuItem;
        ICogFrameGrabber myFlirFg = null;
        private ToolStripMenuItem userAccessSettingsToolStripMenuItem;
        private ToolStripMenuItem systemToolStripMenuItem;
        private ToolStripMenuItem logonToolStripMenuItem;
        private Timer timerUserLogon;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemBackup;
        List<IntPtr> ProcessIdList = new List<IntPtr>();

        #endregion

        /// <summary>
        /// MainForm contructor
        /// </summary>
        public MainForm()
        {

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Create the runtime parameters object
            runtimeParameters = new RuntimeParameters();

            // Create the comIO window object, do not dispose of this object while app is running
            comWindow = new ComIO();

            // Create a new XmlSerializer instance with the type of 'RuntimeParameters'
            this.runtimeParmsXmlSerializer = new XmlSerializer(typeof(RuntimeParameters));

            // Create the server objects
            R1Server = new Server();
            R2Server = new Server();
            R3Server = new Server();
            R4Server = new Server();

            // Create the Client for data logging
            LogClient = new Client();

            // Set the server names
            R1Server.Name = Properties.Settings.Default.R1ServerName;
            R2Server.Name = Properties.Settings.Default.R2ServerName;
            R3Server.Name = Properties.Settings.Default.R3ServerName;
            R4Server.Name = Properties.Settings.Default.R4ServerName;

            // Set the listen ports
            R1Server.ListenPort = Properties.Settings.Default.R1ListenPort;
            R2Server.ListenPort = Properties.Settings.Default.R2ListenPort;
            R3Server.ListenPort = Properties.Settings.Default.R3ListenPort;
            R4Server.ListenPort = Properties.Settings.Default.R4ListenPort;

            // Set the logging IP and Port
            LogClient.ServerIPaddress = "127.0.0.1";
            LogClient.ServerPort = 1235;

            // Set the heartbeat flag
            R1Server.SendHeartbeat = Properties.Settings.Default.SendHeartbeat;
            R2Server.SendHeartbeat = Properties.Settings.Default.SendHeartbeat;
            R3Server.SendHeartbeat = Properties.Settings.Default.SendHeartbeat;
            R4Server.SendHeartbeat = Properties.Settings.Default.SendHeartbeat;

            // Set the heartbeat frequency
            R1Server.HeartbeatFrequency = Properties.Settings.Default.HeartbeatFrequency;
            R2Server.HeartbeatFrequency = Properties.Settings.Default.HeartbeatFrequency;
            R3Server.HeartbeatFrequency = Properties.Settings.Default.HeartbeatFrequency;
            R4Server.HeartbeatFrequency = Properties.Settings.Default.HeartbeatFrequency;

            // Subscribe to Server events
            R1Server.ServerStatusMessageUpdate += new EventHandler<ServerCustomEventArgs>(R1_ServerStatusMessageUpdate);
            R1Server.ServerTCP_MessageUpdate += new EventHandler<ServerCustomEventArgs>(R1_ServerTCP_MessageUpdate);
            R1Server.ServerConnected += new EventHandler<ServerCustomEventArgs>(R1_ServerConnected);

            R2Server.ServerStatusMessageUpdate += new EventHandler<ServerCustomEventArgs>(R2_ServerStatusMessageUpdate);
            R2Server.ServerTCP_MessageUpdate += new EventHandler<ServerCustomEventArgs>(R2_ServerTCP_MessageUpdate);
            R2Server.ServerConnected += new EventHandler<ServerCustomEventArgs>(R2_ServerConnected);

            R3Server.ServerStatusMessageUpdate += new EventHandler<ServerCustomEventArgs>(R3_ServerStatusMessageUpdate);
            R3Server.ServerTCP_MessageUpdate += new EventHandler<ServerCustomEventArgs>(R3_ServerTCP_MessageUpdate);
            R3Server.ServerConnected += new EventHandler<ServerCustomEventArgs>(R3_ServerConnected);

            // Create the Machine Config object
            machineParameters = new MachineConfig(JobsPath);

            // Load in the Machine Config parameters
            bool success;
            machineParameters = machineParameters.DeserializeMachineConfigParameters(out success);
            if (!success) { Environment.Exit(0); }

            // Serialize to update any new object members (developer change);
            machineParameters.SerializeMachineConfigParameters(machineParameters, out success);
            if (!success) { Environment.Exit(0); }

            // If install directory does not exist yet, use the local \\Jobs\\ directory. Note that any files changed
            // in local folders will be included in the installer. If the target machine has the application installed,
            // the files are NOT updated with successive installs.

            if (!Directory.Exists(JobsPath) || !Directory.Exists(@"C:\Program Files\Blueprint Robotics"))
            {
                // Determine if running from the IDE and change the JobsPath
                if (Application.StartupPath.Contains("\\bin\\Debug"))
                {
                    JobsPath = Environment.CurrentDirectory.Replace("\\bin\\Debug", "\\Jobs\\");
                }

                if (Application.StartupPath.Contains("\\bin\\Release"))
                {
                    JobsPath = Environment.CurrentDirectory.Replace("\\bin\\Release", "\\Jobs\\");
                }
            }

        }

        #region Server Event Handlers

        /// <summary>
        /// Custom event when OnServerStatus_MessageUpdate is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void R1_ServerStatusMessageUpdate(object sender, ServerCustomEventArgs args)
        {
            comWindow.UpdateComWindow("R1 Server Status Message", args.Message);
            if (args.Message.Contains("Server Exception"))
            {
                LogClient.SendMessage(false, "In R1_ServerStatusMessageUpdate, Server Exception:" + args.Message.ToString());
            }

        }
        void R2_ServerStatusMessageUpdate(object sender, ServerCustomEventArgs args)
        {
            comWindow.UpdateComWindow("R2 Server Status Message", args.Message);
            if (args.Message.Contains("Server Exception"))
            {
                LogClient.SendMessage(false, "In R2_ServerStatusMessageUpdate, Server Exception:" + args.Message.ToString());
            }

        }
        void R3_ServerStatusMessageUpdate(object sender, ServerCustomEventArgs args)
        {
            comWindow.UpdateComWindow("R3 Server Status Message", args.Message);
            if (args.Message.Contains("Server Exception"))
            {
                LogClient.SendMessage(false, "In R3_ServerStatusMessageUpdate, Server Exception:" + args.Message.ToString());
            }

        }

        /// <summary>
        /// Custom event when OnServerTCP_MessageUpdate is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void R1_ServerTCP_MessageUpdate(object sender, ServerCustomEventArgs args)
        {

            if (args.Message.Contains("KillLoc"))
            {
                // Send KillLoc message to all downstream robots
                R3Server.Write(false, args.Message + R3Server.Terminator);
            }

        }
        void R2_ServerTCP_MessageUpdate(object sender, ServerCustomEventArgs args)
        {

            // Send KillLoc message to all downstream robots
            if (args.Message.Contains("KillLoc"))
            {
                R4Server.Write(false, args.Message + R4Server.Terminator);
            }
        }
        void R3_ServerTCP_MessageUpdate(object sender, ServerCustomEventArgs args)
        {
        }
        void R4_ServerTCP_MessageUpdate(object sender, ServerCustomEventArgs args)
        {
        }

        /// <summary>
        /// Custom event when OnServerConnected is called in the Server class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void R1_ServerConnected(object sender, ServerCustomEventArgs args)
        {

            // Force the form to redraw (Paint event will fire)
            this.Invalidate();

        }
        void R2_ServerConnected(object sender, ServerCustomEventArgs args)
        {

            // Force the form to redraw (Paint event will fire)
            this.Invalidate();

        }
        void R3_ServerConnected(object sender, ServerCustomEventArgs args)
        {

            // Force the form to redraw (Paint event will fire)
            this.Invalidate();

        }
        void R4_ServerConnected(object sender, ServerCustomEventArgs args)
        {

            // Force the form to redraw (Paint event will fire)
            this.Invalidate();

        }
        #endregion

        /// <summary>
        /// Check for Servers connected or disconnected
        /// </summary>
        /// <param name="mode=1 - Check for all connected. mode=2  Check for any disconnected."></param>
        private void CheckServersConnected(int mode)
        {

            bool connected = false;

            connected = R1Server.Connected;
            connected = connected && R2Server.Connected;
            connected = connected && R3Server.Connected;
            connected = connected && R4Server.Connected;

        }

        /// <summary>
        /// Traverse the JobsPath and get the recipe name based on the recipe job number
        /// </summary>
        /// <param name="recipeNumber"></param>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        private int GetRecipeName(int recipeNumber, out string recipeName, out string err)
        {

            recipeName = string.Empty;
            err = string.Empty;
            int erCode = 0;
            int filesFound = 0;

            // Put all xml files into an array.
            string[] recipeFiles = Directory.GetFiles(JobsPath, "*.xml");

            foreach (string s in recipeFiles)
            {
                // Object type for the Runtime Parameters
                RuntimeParameters rp;

                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(RuntimeParameters));
                    using (TextReader textReader = new StreamReader(@s))
                    {
                        rp = (RuntimeParameters)deserializer.Deserialize(textReader);
                        //MessageBox.Show(rp.VisionJobNumber.ToString());
                        if (recipeNumber == rp.VisionJobNumber)
                        {
                            recipeName = Path.GetFileNameWithoutExtension(s);
                            filesFound++;
                        }
                        textReader.Close();
                    }

                }
                catch (Exception ex)
                {

                    LogClient.SendMessage(false, "In GetRecipeName, Exception: " + ex.Message);

                    //MessageBox.Show(e.Message);
                    err = ex.Message;
                    erCode = (int)RecipeFault.GeneralException;
                }

            }

            // If more than one file found, we have duplicate job numbers
            if ((filesFound > 1) && (erCode == 0))
            {
                err = "Duplicate job number found in recipe file.";
                erCode = (int)RecipeFault.DuplicateJobFiles;
            }

            // Validate the matching QuickBuild files exists
            if (!File.Exists(JobsPath + recipeName + ".vpp"))
            {
                err = string.Concat("QuickBuild File does not exist.");
                erCode = (int)RecipeFault.MissingQuickBuildFile;
            }

            return erCode;

        }


        public void UpdateRobotSingleMessage(int robot, string [] message)
        {
            UpdateRobot(robot, message, R1Server.Terminator);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            byte[] cmd = { 0 };

            int x = 25;              // Close to right side of form
            int y = this.Height - 75; // Close to bottom of form

            if (R1Server.Connected)
                ShowConnected(true, x, y, R1Server.Name + ": connected", Color.Black);
            else
                ShowConnected(false, x, y, R1Server.Name + ": connected", Color.DimGray);

            y = this.Height - 55;
            if (R2Server.Connected)
                ShowConnected(true, x, y, R2Server.Name + ": connected", Color.Black);
            else
                ShowConnected(false, x, y, R2Server.Name + ": connected", Color.DimGray);

            x = x + 150;
            y = this.Height - 75;
            if (R3Server.Connected)
                ShowConnected(true, x, y, R3Server.Name + ": connected", Color.Black);
            else
                ShowConnected(false, x, y, R3Server.Name + ": connected", Color.DimGray);

            y = this.Height - 55;
            if (R4Server.Connected)
                ShowConnected(true, x, y, R4Server.Name + ": connected", Color.Black);
            else
                ShowConnected(false, x, y, R4Server.Name + ": connected", Color.DimGray);


            if (runtimeParameters.VisMode == "Simulation")
            {
                labelSimMode.Visible = true;

            }
            else
            {
                labelSimMode.Visible = false;
                if (!checkBoxOnLine.Checked)
                    buttonManualTrigger.Enabled = true;
            }

            if (runtimeParameters.VisDisableTherm)
                labelThermDisabled.Visible = true;
            else
                labelThermDisabled.Visible = false;

            if (this.VisionOnline)
            {
                selectRecipeToolStripMenuItem.Enabled = false;
                labelVisionOffline.Visible = false;
                timerPictureTimer.Start();
                checkBoxOnLine.Checked = true;

            }
            else
            {
                selectRecipeToolStripMenuItem.Enabled = true;
                labelVisionOffline.Visible = true;
                timerPictureTimer.Stop();
                checkBoxOnLine.Checked = false;
            }

            // Allow user to select a job if for some reason the job manager is not initialized
            if (!jobManagerInitialized)
            {
                buttonSelectRecipe.Enabled = true;
            }

            // Update picture frequency
            timerPictureTimer.Interval = this.VisionPictureFrequency;

            // Check for shutdown flag
            if (exitApp)
            {
                Application.Exit();
            }

            // Update Main form title
            this.Text = string.Concat(CurrentLoggedOnUser, " - ", formTitle, this.RecipeName);

            // Enable the controls for the logged on user
            EnableControls(CurrentLoggedOnUser);

        }

        /// <summary>
        /// Display the graphics indicator to show if robot is connected.
        /// </summary>
        /// <param name="connected"></param>
        /// <param name="label"></param>
        /// <param name="textColor"></param>
        private void ShowConnected(bool connected, int x, int y, string label, Color textColor)
        {

            int diameterPix = 10;     // Size of circle 
            Color rim = Color.Empty;

            Graphics graphicsObj = this.CreateGraphics();
            graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            System.Drawing.SolidBrush myCircleBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Empty);
            System.Drawing.SolidBrush myTextBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Empty);

            if (connected)
            {
                rim = Color.Black;
                myCircleBrush.Color = Color.LimeGreen;
                myTextBrush.Color = textColor;
            }
            else
            {
                rim = Color.Gray;
                myCircleBrush.Color = Color.LightGray;
                myTextBrush.Color = textColor;
            }

            graphicsObj.DrawEllipse(new Pen(rim, 2), x, y, diameterPix, diameterPix);
            graphicsObj.FillEllipse(myCircleBrush, x, y, diameterPix, diameterPix);

            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                TextRenderer.DrawText(g, label, this.Font, new Point(x + 15, y), textColor);
            }

            // Clean up
            myCircleBrush.Dispose();
            myTextBrush.Dispose();
            graphicsObj.Dispose();

        }
        private void InitializeJobManager(string path, string file)
        {

            if (formDebug) { return; }

            jobManagerInitialized = false;
            string defaultJobName = string.Empty;

            try
            {
                Popup.Message("Initializing '" + file + "' vision, please wait...");

                textBoxBaslerStatus.Text = "Initializing";
                textBoxFlirStatus.Text = "Initializing";

                // Try to shutdown last Quickbuild job
                if (mcjmAcq != null)
                {
                    mcjmAcq.Stop();
                    mcjmAcq.Shutdown();
                }

                string filePath = path + file + ".vpp";
                if (!File.Exists(filePath))
                {
                    Popup.Close();
                    throw new InvalidOperationException("Cannot load job.  File not found: " + filePath);
                }

                // Assign the CogJob name for use through the application (may be overrwritten below)
                if (runtimeParameters.VisUseBlob)
                {
                    this.CogJobName = runtimeParameters.VisCogJobBlobName;
                }
                if (runtimeParameters.VisUsePatternMatch)
                {
                    this.CogJobName = runtimeParameters.VisCogJobPatternName;
                }

                // Depersist the QuickBuild session
                mcjmAcq = (CogJobManager)CogSerializer.LoadObjectFromFile(path + file + ".vpp");

                // Verify the CogJob exists in Quickbuild
                bool found = this.VerifyCogJobExists(path, file, runtimeParameters.VisCogJobPatternName, out defaultJobName);

                // If job not found, set to Quickbuild default name returned above
                if (!found)
                {
                    this.CogJobName = defaultJobName;
                }

                mcjAcqGrey = mcjmAcq.Job(this.CogJobName);
                mcjiAcqGrey = mcjAcqGrey.OwnedIndependent;


                //check for license and framegrabber
                VisProDongleAndFrameGrabberPresentCheck();

                // Flush queues
                mcjmAcq.UserQueueFlush();
                mcjmAcq.FailureQueueFlush();
                mcjAcqGrey.ImageQueueFlush();
                mcjiAcqGrey.RealTimeQueueFlush();

                // Subscribe to User results event
                mcjmAcq.UserResultAvailable += new CogJobManager.CogUserResultAvailableEventHandler(mcjmAcq_UserResultAvailable);

                // Subscribe to Stopped event
                mcjmAcq.Stopped += new CogJobManager.CogJobManagerStoppedEventHandler(mcjmAcq_Stopped);

                cogRecFlirLane1Raw.Fit(true);
                cogRecFlirLane2Raw.Fit(true);

                textBoxBaslerStatus.Text = "OK";
                textBoxFlirStatus.Text = "OK";

                jobManagerInitialized = true;
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                LogClient.SendMessage(false, "In InitializeJobManager, TargetInvocationException Exception: " + ex.Message);
                MessageBox.Show(ex.Message + " Verify that cameras are communicating, IP addresses are correct, and that eBusPro performance drivers have been installed");
            }
            catch (InvalidOperationException ex)
            {
                LogClient.SendMessage(false, "In InitializeJobManager, InvalidOperationException Exception: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In InitializeJobManager, Exception: " + ex.Message);
                //MessageBox.Show("Error, Make sure Quickbuild is not running and try again.", "Warning!");
                MessageBox.Show(ex.Message + " Stack trace: " + ex.StackTrace);
                Environment.Exit(1);
            }
            finally
            {
                Popup.Close();
            }

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonManualTrigger = new System.Windows.Forms.Button();
            this.cogRecFlirLane1Raw = new Cognex.VisionPro.CogRecordDisplay();
            this.cogRecFlirLane2Raw = new Cognex.VisionPro.CogRecordDisplay();
            this.checkBoxOnLine = new System.Windows.Forms.CheckBox();
            this.buttonAdjustParameters = new System.Windows.Forms.Button();
            this.buttonShowComIO = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectRecipe = new System.Windows.Forms.Button();
            this.labelRecipeNotSelected = new System.Windows.Forms.Label();
            this.labelSimMode = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRecipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRecipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultRecipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveVisionParametersToQuickBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRobotDataModulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engineeringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecipeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.userAccessSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelThermDisabled = new System.Windows.Forms.Label();
            this.timerPictureTimer = new System.Windows.Forms.Timer(this.components);
            this.timerFlirAcqFifo = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBaslerStatus = new System.Windows.Forms.TextBox();
            this.textBoxFlirStatus = new System.Windows.Forms.TextBox();
            this.timerRunRecipeChange = new System.Windows.Forms.Timer(this.components);
            this.labelVisionOffline = new System.Windows.Forms.Label();
            this.timerDataLoggerHeartbeat = new System.Windows.Forms.Timer(this.components);
            this.timerUserLogon = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecFlirLane1Raw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecFlirLane2Raw)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonManualTrigger
            // 
            this.buttonManualTrigger.Location = new System.Drawing.Point(1085, 789);
            this.buttonManualTrigger.Name = "buttonManualTrigger";
            this.buttonManualTrigger.Size = new System.Drawing.Size(174, 69);
            this.buttonManualTrigger.TabIndex = 1;
            this.buttonManualTrigger.Text = "Manual Trigger (F5)";
            this.buttonManualTrigger.Click += new System.EventHandler(this.ManualTrigger_Click);
            // 
            // cogRecFlirLane1Raw
            // 
            this.cogRecFlirLane1Raw.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecFlirLane1Raw.ColorMapLowerRoiLimit = 0D;
            this.cogRecFlirLane1Raw.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecFlirLane1Raw.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecFlirLane1Raw.ColorMapUpperRoiLimit = 1D;
            this.cogRecFlirLane1Raw.DoubleTapZoomCycleLength = 2;
            this.cogRecFlirLane1Raw.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecFlirLane1Raw.Location = new System.Drawing.Point(25, 31);
            this.cogRecFlirLane1Raw.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecFlirLane1Raw.MouseWheelSensitivity = 1D;
            this.cogRecFlirLane1Raw.Name = "cogRecFlirLane1Raw";
            this.cogRecFlirLane1Raw.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecFlirLane1Raw.OcxState")));
            this.cogRecFlirLane1Raw.Size = new System.Drawing.Size(900, 721);
            this.cogRecFlirLane1Raw.TabIndex = 6;
            // 
            // cogRecFlirLane2Raw
            // 
            this.cogRecFlirLane2Raw.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecFlirLane2Raw.ColorMapLowerRoiLimit = 0D;
            this.cogRecFlirLane2Raw.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecFlirLane2Raw.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecFlirLane2Raw.ColorMapUpperRoiLimit = 1D;
            this.cogRecFlirLane2Raw.DoubleTapZoomCycleLength = 2;
            this.cogRecFlirLane2Raw.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecFlirLane2Raw.Location = new System.Drawing.Point(752, 31);
            this.cogRecFlirLane2Raw.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecFlirLane2Raw.MouseWheelSensitivity = 1D;
            this.cogRecFlirLane2Raw.Name = "cogRecFlirLane2Raw";
            this.cogRecFlirLane2Raw.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecFlirLane2Raw.OcxState")));
            this.cogRecFlirLane2Raw.Size = new System.Drawing.Size(900, 721);
            this.cogRecFlirLane2Raw.TabIndex = 7;
            // 
            // checkBoxOnLine
            // 
            this.checkBoxOnLine.Location = new System.Drawing.Point(1360, 0);
            this.checkBoxOnLine.Name = "checkBoxOnLine";
            this.checkBoxOnLine.Size = new System.Drawing.Size(67, 29);
            this.checkBoxOnLine.TabIndex = 14;
            this.checkBoxOnLine.Text = "Online";
            this.checkBoxOnLine.UseVisualStyleBackColor = true;
            this.checkBoxOnLine.CheckedChanged += new System.EventHandler(this.checkBoxEnableRobotComm_CheckedChanged);
            // 
            // buttonAdjustParameters
            // 
            this.buttonAdjustParameters.Location = new System.Drawing.Point(1266, 704);
            this.buttonAdjustParameters.Name = "buttonAdjustParameters";
            this.buttonAdjustParameters.Size = new System.Drawing.Size(174, 69);
            this.buttonAdjustParameters.TabIndex = 15;
            this.buttonAdjustParameters.Text = "Adjust Parameters (F6)";
            this.buttonAdjustParameters.UseVisualStyleBackColor = true;
            this.buttonAdjustParameters.Click += new System.EventHandler(this.buttonAdjustParameters_Click);
            // 
            // buttonShowComIO
            // 
            this.buttonShowComIO.Location = new System.Drawing.Point(1266, 789);
            this.buttonShowComIO.Name = "buttonShowComIO";
            this.buttonShowComIO.Size = new System.Drawing.Size(174, 69);
            this.buttonShowComIO.TabIndex = 16;
            this.buttonShowComIO.Text = "Show Com IO (F7)";
            this.buttonShowComIO.UseVisualStyleBackColor = true;
            this.buttonShowComIO.Click += new System.EventHandler(this.buttonShowComIO_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 628);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Cam 1 Image Display (Lane 1)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(982, 628);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Cam 2 Image Display (Lane 2)";
            // 
            // buttonSelectRecipe
            // 
            this.buttonSelectRecipe.Location = new System.Drawing.Point(1085, 704);
            this.buttonSelectRecipe.Name = "buttonSelectRecipe";
            this.buttonSelectRecipe.Size = new System.Drawing.Size(174, 69);
            this.buttonSelectRecipe.TabIndex = 19;
            this.buttonSelectRecipe.Text = "Select Recipe (F4)";
            this.buttonSelectRecipe.UseVisualStyleBackColor = true;
            this.buttonSelectRecipe.Click += new System.EventHandler(this.buttonSelectRecipe_Click);
            // 
            // labelRecipeNotSelected
            // 
            this.labelRecipeNotSelected.AutoSize = true;
            this.labelRecipeNotSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecipeNotSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelRecipeNotSelected.Location = new System.Drawing.Point(678, 803);
            this.labelRecipeNotSelected.Name = "labelRecipeNotSelected";
            this.labelRecipeNotSelected.Size = new System.Drawing.Size(300, 36);
            this.labelRecipeNotSelected.TabIndex = 20;
            this.labelRecipeNotSelected.Text = "Recipe Not selected";
            // 
            // labelSimMode
            // 
            this.labelSimMode.AutoSize = true;
            this.labelSimMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSimMode.ForeColor = System.Drawing.Color.Red;
            this.labelSimMode.Location = new System.Drawing.Point(611, 735);
            this.labelSimMode.Name = "labelSimMode";
            this.labelSimMode.Size = new System.Drawing.Size(448, 36);
            this.labelSimMode.TabIndex = 21;
            this.labelSimMode.Text = "Vision Simulation Mode Active";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.advancedToolStripMenuItem,
            this.systemToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1244, 28);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectRecipeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.deleteRecipeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectRecipeToolStripMenuItem
            // 
            this.selectRecipeToolStripMenuItem.Name = "selectRecipeToolStripMenuItem";
            this.selectRecipeToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.selectRecipeToolStripMenuItem.Text = "Select Recipe";
            this.selectRecipeToolStripMenuItem.Click += new System.EventHandler(this.selectRecipeToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.saveToolStripMenuItem.Text = "Save Parameters";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.saveAsToolStripMenuItem.Text = "Save Recipe As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // deleteRecipeToolStripMenuItem
            // 
            this.deleteRecipeToolStripMenuItem.Enabled = false;
            this.deleteRecipeToolStripMenuItem.Name = "deleteRecipeToolStripMenuItem";
            this.deleteRecipeToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.deleteRecipeToolStripMenuItem.Text = "Delete Recipe";
            this.deleteRecipeToolStripMenuItem.Click += new System.EventHandler(this.deleteRecipeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultRecipeToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // defaultRecipeToolStripMenuItem
            // 
            this.defaultRecipeToolStripMenuItem.Name = "defaultRecipeToolStripMenuItem";
            this.defaultRecipeToolStripMenuItem.Size = new System.Drawing.Size(186, 26);
            this.defaultRecipeToolStripMenuItem.Text = "DefaultRecipe";
            this.defaultRecipeToolStripMenuItem.Click += new System.EventHandler(this.defaultRecipeToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveVisionParametersToQuickBuildToolStripMenuItem,
            this.saveRobotDataModulesToolStripMenuItem,
            this.engineeringToolStripMenuItem,
            this.openRecipeFolderToolStripMenuItem,
            this.toolStripSeparator3,
            this.userAccessSettingsToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // saveVisionParametersToQuickBuildToolStripMenuItem
            // 
            this.saveVisionParametersToQuickBuildToolStripMenuItem.Name = "saveVisionParametersToQuickBuildToolStripMenuItem";
            this.saveVisionParametersToQuickBuildToolStripMenuItem.Size = new System.Drawing.Size(337, 26);
            this.saveVisionParametersToQuickBuildToolStripMenuItem.Text = "Save Vision Parameters to QuickBuild";
            this.saveVisionParametersToQuickBuildToolStripMenuItem.Click += new System.EventHandler(this.saveVisionParametersToQuickBuildToolStripMenuItem_Click);
            // 
            // saveRobotDataModulesToolStripMenuItem
            // 
            this.saveRobotDataModulesToolStripMenuItem.Name = "saveRobotDataModulesToolStripMenuItem";
            this.saveRobotDataModulesToolStripMenuItem.Size = new System.Drawing.Size(337, 26);
            this.saveRobotDataModulesToolStripMenuItem.Text = "Save Robot Data Modules";
            this.saveRobotDataModulesToolStripMenuItem.Click += new System.EventHandler(this.saveDataModulesToolStripMenuItem_Click);
            // 
            // engineeringToolStripMenuItem
            // 
            this.engineeringToolStripMenuItem.Name = "engineeringToolStripMenuItem";
            this.engineeringToolStripMenuItem.Size = new System.Drawing.Size(337, 26);
            this.engineeringToolStripMenuItem.Text = "Engineering";
            this.engineeringToolStripMenuItem.Click += new System.EventHandler(this.engineeringToolStripMenuItem_Click);
            // 
            // openRecipeFolderToolStripMenuItem
            // 
            this.openRecipeFolderToolStripMenuItem.Name = "openRecipeFolderToolStripMenuItem";
            this.openRecipeFolderToolStripMenuItem.Size = new System.Drawing.Size(337, 26);
            this.openRecipeFolderToolStripMenuItem.Text = "Open Recipe Folder";
            this.openRecipeFolderToolStripMenuItem.Click += new System.EventHandler(this.openRecipeFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(334, 6);
            // 
            // userAccessSettingsToolStripMenuItem
            // 
            this.userAccessSettingsToolStripMenuItem.Name = "userAccessSettingsToolStripMenuItem";
            this.userAccessSettingsToolStripMenuItem.Size = new System.Drawing.Size(337, 26);
            this.userAccessSettingsToolStripMenuItem.Text = "User Access Settings";
            this.userAccessSettingsToolStripMenuItem.Click += new System.EventHandler(this.userAccessSettingstoolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logonToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItemBackup,
            this.optionsToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // logonToolStripMenuItem
            // 
            this.logonToolStripMenuItem.Name = "logonToolStripMenuItem";
            this.logonToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.logonToolStripMenuItem.Text = "Log On/Off";
            this.logonToolStripMenuItem.Click += new System.EventHandler(this.logonToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(164, 6);
            // 
            // toolStripMenuItemBackup
            // 
            this.toolStripMenuItemBackup.Name = "toolStripMenuItemBackup";
            this.toolStripMenuItemBackup.Size = new System.Drawing.Size(167, 26);
            this.toolStripMenuItemBackup.Text = "Backup";
            this.toolStripMenuItemBackup.Click += new System.EventHandler(this.toolStripMenuItemBackup_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // labelThermDisabled
            // 
            this.labelThermDisabled.AutoSize = true;
            this.labelThermDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelThermDisabled.ForeColor = System.Drawing.Color.Red;
            this.labelThermDisabled.Location = new System.Drawing.Point(611, 770);
            this.labelThermDisabled.Name = "labelThermDisabled";
            this.labelThermDisabled.Size = new System.Drawing.Size(418, 36);
            this.labelThermDisabled.TabIndex = 23;
            this.labelThermDisabled.Text = "Thermal Inspection Disabled";
            // 
            // timerPictureTimer
            // 
            this.timerPictureTimer.Interval = 400;
            this.timerPictureTimer.Tick += new System.EventHandler(this.timerPictureTimer_Tick);
            // 
            // timerFlirAcqFifo
            // 
            this.timerFlirAcqFifo.Interval = 60000;
            this.timerFlirAcqFifo.Tick += new System.EventHandler(this.timerFlirAcqFifo_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 698);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Cam 1 status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 747);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 18);
            this.label4.TabIndex = 26;
            this.label4.Text = "Cam 2 status:";
            // 
            // textBoxBaslerStatus
            // 
            this.textBoxBaslerStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxBaslerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBaslerStatus.Location = new System.Drawing.Point(209, 704);
            this.textBoxBaslerStatus.Multiline = true;
            this.textBoxBaslerStatus.Name = "textBoxBaslerStatus";
            this.textBoxBaslerStatus.ReadOnly = true;
            this.textBoxBaslerStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxBaslerStatus.Size = new System.Drawing.Size(318, 58);
            this.textBoxBaslerStatus.TabIndex = 27;
            this.textBoxBaslerStatus.Text = "OK";
            // 
            // textBoxFlirStatus
            // 
            this.textBoxFlirStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxFlirStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFlirStatus.Location = new System.Drawing.Point(209, 771);
            this.textBoxFlirStatus.Multiline = true;
            this.textBoxFlirStatus.Name = "textBoxFlirStatus";
            this.textBoxFlirStatus.ReadOnly = true;
            this.textBoxFlirStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFlirStatus.Size = new System.Drawing.Size(318, 57);
            this.textBoxFlirStatus.TabIndex = 28;
            this.textBoxFlirStatus.Text = "OK";
            // 
            // timerRunRecipeChange
            // 
            this.timerRunRecipeChange.Interval = 1000;
            this.timerRunRecipeChange.Tick += new System.EventHandler(this.timerRunRecipeChange_Tick);
            // 
            // labelVisionOffline
            // 
            this.labelVisionOffline.AutoSize = true;
            this.labelVisionOffline.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionOffline.ForeColor = System.Drawing.Color.Red;
            this.labelVisionOffline.Location = new System.Drawing.Point(728, 698);
            this.labelVisionOffline.Name = "labelVisionOffline";
            this.labelVisionOffline.Size = new System.Drawing.Size(225, 36);
            this.labelVisionOffline.TabIndex = 24;
            this.labelVisionOffline.Text = "Vision Off Line";
            // 
            // timerDataLoggerHeartbeat
            // 
            this.timerDataLoggerHeartbeat.Interval = 10000;
            this.timerDataLoggerHeartbeat.Tick += new System.EventHandler(this.timerDataLoggerHeartbeat_Tick);
            // 
            // timerUserLogon
            // 
            this.timerUserLogon.Interval = 1000;
            this.timerUserLogon.Tick += new System.EventHandler(this.timerUserLogon_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1244, 761);
            this.Controls.Add(this.labelRecipeNotSelected);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelThermDisabled);
            this.Controls.Add(this.labelVisionOffline);
            this.Controls.Add(this.labelSimMode);
            this.Controls.Add(this.textBoxBaslerStatus);
            this.Controls.Add(this.buttonSelectRecipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFlirStatus);
            this.Controls.Add(this.buttonShowComIO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonAdjustParameters);
            this.Controls.Add(this.checkBoxOnLine);
            this.Controls.Add(this.cogRecFlirLane2Raw);
            this.Controls.Add(this.cogRecFlirLane1Raw);
            this.Controls.Add(this.buttonManualTrigger);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Current Selected Recipe:";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecFlirLane1Raw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecFlirLane2Raw)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Exit if we're running
                if (Utilities.IsThisProcessRunning())
                {
                    return;
                }

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                //LogClient.SendMessage(false, "In Main, Exception: " + ex.Message);
                MessageBox.Show(ex.Message + " Stack Trace: " + ex.StackTrace);
            }
        }

        private void ManualTrigger_Click(object sender, System.EventArgs e)
        {
            try
            {
                RunQuickBuildJob();
            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In ManualTrigger_Click, Exception: " + ex.Message);
                MessageBox.Show(ex.Message, "Runtime Exception @ManualTrigger");
            }
        }

        private bool IsFrameGrabberNull(CogAcqFifoTool fifo)
        {
            return (fifo.Operator.FrameGrabber == null);
        }
        private bool DongleIsPresent()
        {
            CogStringCollection features = CogLicense.GetLicensedFeatures(true,false);
            return (features.Count != 0);

        }
        private bool AllFrameGrabbersPresent()
        {

            CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(CogJobName).VisionTool;
            CogAcqFifoTool myBaslerAcqTool = (CogAcqFifoTool)lctg.Tools["BaslerAcqFiFo"];
            CogAcqFifoTool myFlirAcqTool = (CogAcqFifoTool)lctg.Tools["FlirAcqFiFo"];

            return !IsFrameGrabberNull(myBaslerAcqTool)
            || !IsFrameGrabberNull(myFlirAcqTool);

        }
        private void VisProDongleAndFrameGrabberPresentCheck()
        {
            //if dongle or framegrabber is not present, ask user if we shold bail or continue - only in debug


            if (FirstPass)
            {
                //flag that the first pass has occurred and does not need to be checked
                FirstPass = false;

                //check for the dongle

                bool DongleFound = DongleIsPresent();
                bool FrameGrabbersReady = AllFrameGrabbersPresent();


                //if either is missing bail if in RELEASE or prompt user in DEBUG if continue
                if (!DongleFound || !FrameGrabbersReady)
                {
                    string msg = string.Empty;
                    if (!DongleFound)
                    {
                        msg += "No license found.  Check USB Dongle.  ";
                    }

                    if (!FrameGrabbersReady)
                    {
                        //msg += "Frame Grabber(s) Missing.  Check connections on both cameras.  Verify IP address of camera matches port it's connected to.  Also check that AcqFIFOs are initialized. ";
                        msg += "Camera communication failure. Check to ensure voltage is supplied to both cameras and check that both RJ-45 Communication cables are securely connected.";
                    }
#if !DEBUG
                    //release section
                    MessageBox.Show(msg);
                    Environment.Exit(1);
#else
                    //debug section

                    //ask user if we should continue
                    //if (MessageBox.Show(msg + " Continue? ", "VisPro not ready. Continue? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    DisableFaultsInDebug = true;
                    //}
                    //else
                    //{
                    //    DisableFaultsInDebug = false;
                    //}


#endif
                }

                // Enable/disable Flir NUC
                if (machineParameters.EnableThermalAutoNUC)
                {
                    FlirNUC_operation((int)FLIR_Operation.EnableNUC);
                }
                else
                {
                    FlirNUC_operation((int)FLIR_Operation.DisableNUC);
                }

            }

        }

        /// <summary>
        /// Perform NUC operations on the FLIR camera
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool FlirNUC_operation(int mode)
        {

            bool success = false;

            try
            {
                frameGrabbers = new CogFrameGrabberGigEs();
                foreach (ICogFrameGrabber fg in frameGrabbers)
                {
                    if (fg.Name.Contains("FLIR"))
                    {
                        myFlirFg = fg;
                    }
                }

                if (myFlirFg != null)
                {
                    if (mode == (int)FLIR_Operation.EnableNUC) // Enable automatic NUC mode
                    {
                        myFlirFg.OwnedGigEAccess.SetFeature("NUCMode", "Automatic");
                        success = true;
                    }
                    else if (mode == (int)FLIR_Operation.DisableNUC) // Disable automatic NUC mode
                    {
                        myFlirFg.OwnedGigEAccess.SetFeature("NUCMode", "Off");
                        success = true;
                    }
                    else if (mode == (int)FLIR_Operation.PerformNUC) // Perform the NUC action
                    {
                        myFlirFg.OwnedGigEAccess.SetFeature("NUCAction", "1");
                        success = true;
                        timerFlirNUCaction.Restart();
                    }
                    else
                    {
                        MessageBox.Show("Invalid mode @FlirNUC_operation", "Warning");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error performing NUC operation: "+ex.Message,"Warning");
                LogClient.SendMessage(false, "In FlirNUC_operation, Exception: " + ex.Message);
                comWindow.UpdateComWindow("Exception in 'FlirNUC_operation':", ex.Message);

                if (!this.VisionOnline)
                {
                    MessageBox.Show("Exception in 'FlirNUC_operation':" + ex.Message, "Error");
                }
            }

            return success;

        }

        /// <summary>
        /// Perform an auto focus on the thermal camera
        /// </summary>
        /// <returns></returns>
        public bool FlirAutoFocus()
        {
            bool success = false;

            if (this.VisionOnline)
            {
                MessageBox.Show("Vision must be offline", "Error");
                return success;
            }

            try
            {
                frameGrabbers = new CogFrameGrabberGigEs();
                foreach (ICogFrameGrabber fg in frameGrabbers)
                {
                    if (fg.Name.Contains("FLIR"))
                    {
                        myFlirFg = fg;
                    }
                }

                if (myFlirFg != null)
                {
                    myFlirFg.OwnedGigEAccess.SetFeature("AutoFocus", "");
                    success = true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error performing NUC operation: "+ex.Message,"Warning");
                LogClient.SendMessage(false, "In FlirAutoFocus, Exception: " + ex.Message);
                comWindow.UpdateComWindow("Exception in 'FlirAutoFocus':", ex.Message);
            }

            return success;
        }

        public void RunQuickBuildJob()
        {
            if (formDebug) { return; }
            if (runQuickBuildException) { return; } // Prevent window stacking

            CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(this.CogJobName).VisionTool;
            CogJob myJob = mcjmAcq.Job(this.CogJobName);

            VisProDongleAndFrameGrabberPresentCheck();

            LogClient.SendMessage(false, "In RunQuickBuildJob:");

            try
            {

                // Send flag to QuickBuild to indicate VisualStudio app is running
                lctg.SetScriptTerminalData("VS_Running", true);


                if (runtimeParameters.VisMode == "Simulation")
                {
                    cogRecFlirLane1Raw.InteractiveGraphics.Clear();
                    cogRecFlirLane2Raw.InteractiveGraphics.Clear();
                }

                // Start vision timer (results in Stopped event)
                visionTimer.Reset();
                visionTimer.Start();

                // Run the job
                LogClient.SendMessage(false, "In RunQuickBuildJob start myJob.Run()");
                myJob.Run();
                LogClient.SendMessage(false, "In RunQuickBuildJob finish myJob.Run()");

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In RunQuickBuildJob, Exception: " + ex.Message);

                // ## Temp until interlock implemented
                if (!ex.Message.Contains("is not runnable"))
                {
                    runQuickBuildException = true;
                    string str = string.Concat("Exception @runQuickBuildJob: ", ex.Message, "\n");
                    str = string.Concat(str, " 1) Check to ensure cameras are powered up.\n");
                    str = string.Concat(str, " 2) Check cameras for other problems.\n");
                    str = string.Concat(str, " 3) Press <Abort> to exit application.");
                    if (DialogResult.Abort == MessageBox.Show(str, "Error", MessageBoxButtons.AbortRetryIgnore))
                    {
                        this.Invalidate();      // Force paint event
                        exitApp = true;         // Exit app in paint event
                        return;
                    }

                    runQuickBuildException = false;
                }
            }


        }

        private void mainForm_Load(object sender, EventArgs e)
        {

            string recipe = string.Empty;

            try
            {
                // Dim controls until recipe is selected
                labelRecipeNotSelected.Visible = true;
                buttonAdjustParameters.Enabled = false;
                checkBoxOnLine.Enabled = false;

                // Start the robot server threads
                StartRobotServerThreads(true);

                // Set to false until recipe is selected
                saveAsToolStripMenuItem.Enabled = false;
                deleteRecipeToolStripMenuItem.Enabled = false;

                // Try to auto select the recipe
                recipe = machineParameters.DefaultRecipe;
                if (recipe != "None" && recipe != string.Empty)
                {
                    SelectRecipeRuntime(true, true, recipe, true);
                }


                // Start the background timer to aquire one image per frequency
                // do not prevent this timer from starting i.e. return prior to this execution point
                this.VisionPictureFrequency = runtimeParameters.VisPictureFrequency;
                timerPictureTimer.Interval = Math.Max(100, this.VisionPictureFrequency);
                timerPictureTimer.Start();

                //timerFlirAcqFifo.Start();   // Uncomment to show the FocusPos parameter

                statusUpdateGreyScale.Start();
                statusUpdateThermal.Start();

                // Start the NUC action timer
                timerFlirNUCaction.Start();

                //timer1.Start(); // Debug

                //InitializeABBControllerManager();

                // Remove the top right close button
                RemoveCloseButton(this);

                // Set the password reset timeout (to milliseconds)
                timerUserLogon.Interval = machineParameters.PasswordResetTimeoutInMinutes * 60000;

                // Set default accounts for new installs
                if (this.GetUserAccountName(0) != "bpr")
                {
                    machineParameters.PasswordResetTimeoutInMinutes = 15;
                    machineParameters.StartupUserName = "admin";

                    this.SetUserAccountName(0, "bpr");
                    this.SetUserAccountPassword(0, "bprsbprs");
                    this.SetUserAccountLevel(0, 3);

                    this.SetUserAccountName(1, "monitor");
                    this.SetUserAccountPassword(1, "");
                    this.SetUserAccountLevel(1, 1);

                    this.SetUserAccountName(2, "operator");
                    this.SetUserAccountPassword(2, "12");
                    this.SetUserAccountLevel(2, 2);

                    this.SetUserAccountName(3, "admin");
                    this.SetUserAccountPassword(3, "2013");
                    this.SetUserAccountLevel(3, 3);

                    this.SaveMachineParameters(machineParameters);
                }

                // Set default logon name
                if (!machineParameters.StartupUserName.Equals(string.Empty))
                {
                    CurrentLoggedOnUser = machineParameters.StartupUserName;
                }

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In mainForm_Load, Exception: " + ex.Message);
                MessageBox.Show("Exception @mainForm_Load:" + ex.Message);
            }

        }
        public double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public double RadiansToDegrees(double rad)
        {
            return rad * 180 / Math.PI;
        }


        /// <summary>
        /// Sort a collection of Robot Locations contained in an ArrayList based on an input sort angle.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private ArrayList SortRobLocs(ArrayList list, int angle)
        {
            int ox = 400;   //Arbitrary point
            int oy = 400;   //Arbitrary point
            double X;
            double Y;

            ArrayList targListToSort = new ArrayList();
            ArrayList targFinalSortedList = new ArrayList();
            List<int> listID = new List<int>();

            RobTarget rts = new RobTarget();
            RobTarget rtFinal = new RobTarget();

            foreach (RobTarget rt in list)
            {

                // Rotate point about ox origin
                X = Math.Cos(DegreesToRadians(angle)) * (rt.X - ox) - Math.Sin(DegreesToRadians(angle)) * (rt.Y - oy) + ox;
                Y = Math.Sin(DegreesToRadians(angle)) * (rt.X - ox) + Math.Cos(DegreesToRadians(angle)) * (rt.Y - oy) + oy;

                // Add to list to be sorted
                rts.X = X;
                rts.Y = Y;
                rts.ID = rt.ID;
                listID.Add(rt.ID);  // Add the ID to a separate list for re-indexing later on
                rts.Theta = rt.Theta;
                targListToSort.Add(rts);

            }

            // Sort from top to bottom (after the points have been rotated)
            var targSortedList = from locAny in targListToSort.Cast<RobTarget>()
                                 orderby locAny.Y ascending
                                 select locAny;

            targFinalSortedList.Clear();

            // Re-index the item ID
            int listCntr = 0;
            listID.Sort();
            foreach (RobTarget rtsCheck in targSortedList)
            {
                foreach (RobTarget rt in list)
                {
                    if (rtsCheck.ID == rt.ID)
                    {
                        rtFinal.X = rt.X;
                        rtFinal.Y = rt.Y;
                        rtFinal.Theta = rt.Theta;
                        rtFinal.ID = listID[listCntr];  //Re-index with previously compiled list
                        listCntr++;
                        break;
                    }
                }
                targFinalSortedList.Add(rtFinal);
            }

            return targFinalSortedList;

        }

        #region Vision process finished

        delegate void mcjmAcq_StoppedDelegate(object sender, CogJobManagerActionEventArgs e);
        private void mcjmAcq_Stopped(object sender, CogJobManagerActionEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new mcjmAcq_StoppedDelegate(mcjmAcq_Stopped), new object[] { sender, e });
                    return;
                }

                // Update timer for stats
                this.VisionProcessTime = visionTimer.ElapsedMilliseconds;
                visionTimer.Stop();

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In mcjmAcq_Stopped, Exception: " + ex.Message);
                MessageBox.Show("mcjmAcq_Stopped Exception: " + ex.Message);
            }
        }
        #endregion

        #region "Inspection"

        delegate void mcjmAcq_UserResultAvailableDelegate(object sender, CogJobManagerActionEventArgs e);
        private void mcjmAcq_UserResultAvailable(object sender, CogJobManagerActionEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new mcjmAcq_UserResultAvailableDelegate(mcjmAcq_UserResultAvailable), new object[] { sender, e });
                    return;
                }


                CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(this.CogJobName).VisionTool;

                bool consoleShowlist = false;   //Internal debug flag
                int totalOddCount = 0;
                int totalEvenCount = 0;
                int angle;
                ArrayList finalSortedOdd = new ArrayList();
                ArrayList finalSortedEven = new ArrayList();

                // strings used to send out data in frames
                string rob1Data = string.Empty;
                string rob2Data = string.Empty;
                string rob3Data = string.Empty;
                string rob4Data = string.Empty;

                // Capture last checkbox state, (safety in case checkbox is clicked during processing).
                bool sendToR1 = runtimeParameters.VisSendLocsToR1;
                bool sendToR2 = runtimeParameters.VisSendLocsToR2;
                bool sendToR3 = runtimeParameters.VisSendLocsToR3;
                bool sendToR4 = runtimeParameters.VisSendLocsToR4;

                // Set flags for NUC
                bool cond1 = false;
                bool cond2 = false;

                // Define display records
                Cognex.VisionPro.ICogRecord tmpRecord;
                Cognex.VisionPro.ICogRecord topRecord = mcjmAcq.UserResult();

                // Assume that the required "image" record is present, and go get it.
                // Update the final Basler results
                tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                tmpRecord = tmpRecord.SubRecords["LastRun"];
                tmpRecord = tmpRecord.SubRecords["CalBasler.OutputImage"];
                cogRecFlirLane1Raw.Record = tmpRecord;
                cogRecFlirLane1Raw.Fit(true);

                // Send sim locs if in simulation mode
                if (runtimeParameters.VisMode == "Simulation")
                {
                    SendSimLocs(sendToR1, sendToR2, sendToR3, sendToR4, consoleShowlist);
                }
                else
                {
                    // Get failure data from QuickBuild
                    this.VisionAngleFailures = this.VisionAngleFailures + (int)lctg.GetScriptTerminalData("AngleFailures");
                    this.VisionThermalFailures = this.VisionThermalFailures + (int)lctg.GetScriptTerminalData("ThermalFailures");

                    # region Basler/Flir Exception display

                    // Freeze exception for 10 secs. so it can be read
                    if ((string)lctg.GetScriptTerminalData("BaslerStatus") != "OK")
                    {
                        textBoxBaslerStatus.Text = (string)lctg.GetScriptTerminalData("BaslerStatus");
                        statusUpdateGreyScale.Restart();

                    }
                    else
                    {
                        if (statusUpdateGreyScale.ElapsedMilliseconds > 10000)
                        {
                            textBoxBaslerStatus.Text = (string)lctg.GetScriptTerminalData("BaslerStatus");
                            statusUpdateGreyScale.Restart();
                        }

                    }

                    // Freeze exception for 10 secs. so it can be read
                    if ((string)lctg.GetScriptTerminalData("FlirStatus") != "OK")
                    {
                        textBoxFlirStatus.Text = (string)lctg.GetScriptTerminalData("FlirStatus");
                        statusUpdateThermal.Restart();
                    }
                    else
                    {
                        if (statusUpdateThermal.ElapsedMilliseconds > 10000)
                        {
                            textBoxFlirStatus.Text = (string)lctg.GetScriptTerminalData("FlirStatus");
                            statusUpdateThermal.Restart();
                        }
                    }
                    # endregion

                    // Get blob collection from QuickBuild
                    ArrayList lPoses = (ArrayList)lctg.GetScriptTerminalData("BlobCollection");

                    LogClient.SendMessage(false, "In UserResultsAvailable: lPoses count:" + lPoses.Count.ToString());

                    // Create lists for sorting
                    ArrayList collectionOdd = new ArrayList();
                    ArrayList collectionOverLapOdd = new ArrayList();
                    ArrayList collectionEven = new ArrayList();
                    ArrayList collectionOverLapEven = new ArrayList();

                    // Collect blobs for the Odd and Even sides
                    foreach (CogTransform2DLinear trans in lPoses)
                    {
                        VisItemId = (VisItemId % Properties.Settings.Default.VisionItemIDLimit) + 1;

                        if (true)
                        {
                            //No operation, will collect middle lane blobs in next loop
                        }
   
                    }

                    // Collect blobs for the middle lane
                    foreach (CogTransform2DLinear trans in lPoses)
                    {
                        VisItemId = (VisItemId % Properties.Settings.Default.VisionItemIDLimit) + 1;

                         }

                    // Decide to send new frame message to odd side robots
                    totalOddCount = collectionOdd.Count + collectionOverLapOdd.Count;
                    TotalOddCountThisFrame = totalOddCount;
                    if (totalOddCount > 0)
                    {
                        if (sendToR1)
                        {
                            //R1Server.Write(false, string.Concat("VisionNewFrame,1,1", R1Server.Terminator));

                            rob1Data = string.Concat("VisionNewFrame,1,1", R1Server.Terminator);
                        }
                        if (sendToR3)
                        {
                            //R3Server.Write(false, string.Concat("VisionNewFrame,1,1", R3Server.Terminator));
                            rob3Data = string.Concat("VisionNewFrame,1,1", R3Server.Terminator);
                        }
                    }

                    // Decide to send new frame message
                    totalEvenCount = collectionEven.Count + collectionOverLapEven.Count;
                    TotalEvenCountThisFrame = totalEvenCount;
                    if (totalEvenCount > 0)
                    {
                        if (sendToR2)
                        {
                            //R2Server.Write(false, string.Concat("VisionNewFrame,2,2", R2Server.Terminator));
                            rob2Data = string.Concat("VisionNewFrame,2,2", R2Server.Terminator);
                        }
                        if (sendToR4)
                        {
                            //R4Server.Write(false, string.Concat("VisionNewFrame,2,2", R4Server.Terminator));
                            rob4Data = string.Concat("VisionNewFrame,2,2", R4Server.Terminator);
                        }
                    }

                    // Update stats
                    this.VisionItemsLocated = this.VisionItemsLocated + totalOddCount + totalEvenCount;

                    if (runtimeParameters.VisShowItemCount && ((totalOddCount + totalEvenCount) > 0))
                    {
                        comWindow.UpdateComWindow("Stats", "Total Odd count:" + collectionOdd.Count.ToString());
                        comWindow.UpdateComWindow("Stats", "Total Even count:" + collectionEven.Count.ToString());
                    }

                    // Sort the locations based on the sort angle
                    if (totalOddCount > 0)
                    {
                        angle = (int)runtimeParameters.VisOddSortAngle;
                        finalSortedOdd = SortRobLocs(collectionOdd, angle + machineParameters.VisRobAngleOffs);

                        // Append odd side overlap items
                        foreach (RobTarget rt in collectionOverLapOdd)
                        {
                            finalSortedOdd.Add(rt);
                        }
                    }
                    if (totalEvenCount > 0)
                    {
                        angle = (int)runtimeParameters.VisEvenSortAngle;
                        finalSortedEven = SortRobLocs(collectionEven, angle + machineParameters.VisRobAngleOffs);

                        // Append even side overlap items
                        foreach (RobTarget rt in collectionOverLapEven)
                        {
                            finalSortedEven.Add(rt);
                        }
                    }

                    // Assume that the required "image" record is present, and go get it.
                    // Update the final Flir results
                    tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
                    tmpRecord = tmpRecord.SubRecords["LastRun"];
                    tmpRecord = tmpRecord.SubRecords["CalFlir.OutputImage"];
                    cogRecFlirLane2Raw.Record = tmpRecord;
                    cogRecFlirLane2Raw.Fit(true);

                    // Create a graphics label
                    CogGraphicLabel idLabel = new CogGraphicLabel();

                    double r_degrees = 0;

                    #region Show Odd side arrow
                    // Show Odd side arrow
                    if (runtimeParameters.VisShowOddSortOrder)
                    {
                        CogLineSegment myLine = new CogLineSegment();
                        myLine.EndX = 350;
                        myLine.EndY = 100;
                        myLine.StartX = myLine.EndX - Math.Sin(DegreesToRadians((int)runtimeParameters.VisOddSortAngle + machineParameters.VisArrowAngleOffs)) * 35;
                        myLine.StartY = myLine.EndY - Math.Cos(DegreesToRadians((int)runtimeParameters.VisOddSortAngle + machineParameters.VisArrowAngleOffs)) * 35;
                        myLine.StartPointAdornment = CogLineSegmentAdornmentConstants.Arrow;
                        myLine.Color = CogColorConstants.Cyan;
                        myLine.LineWidthInScreenPixels = 5;
                        myLine.SelectedSpaceName = "*";  //Pixel space
                        cogRecFlirLane1Raw.StaticGraphics.Add(myLine, "test");
                    }
                    #endregion

                    // Send to odd side
                    foreach (RobTarget trans in finalSortedOdd)
                    {

                        // Display a label
                        if (runtimeParameters.VisShowItemID)
                        {
                            idLabel.Color = CogColorConstants.White;
                            idLabel.SetXYText(trans.X - 50, trans.Y - 50, "ID: " + trans.ID.ToString());
                            cogRecFlirLane1Raw.StaticGraphics.Add(idLabel, "main");
                        }

                        // Display sorted ID
                        if (runtimeParameters.VisShowOddSortOrder)
                        {
                            //comWindow.UpdateComWindow("Sort Order", "Odd ID: " + trans.ID.ToString());
                        }

                        r_degrees = trans.Theta * 180 / Math.PI; // Convert from radians to degrees
                        //comWindow.UpdateComWindow("R1R3 VisLoc",string.Concat("VL,1,1,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0")));

                        if (sendToR1)
                        {
                            //R1Server.Write(false, string.Concat("VL,1,1,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R1Server.Terminator));
                            rob1Data += string.Concat("VL,1,1,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R1Server.Terminator);

                        }
                        if (sendToR3)
                        {
                            //R3Server.Write(false, string.Concat("VL,1,1,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R3Server.Terminator));
                            rob3Data += string.Concat("VL,1,1,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R3Server.Terminator);
                        }

                    }

                    // Decide to send end frame message
                    if (totalOddCount > 0)
                    {
                        if (sendToR1)
                        {
                            rob1Data += string.Concat("VisionEndFrame,1,1", R1Server.Terminator);
                            R1Server.Write(false, rob1Data);

                            //R1Server.Write(false, string.Concat("VisionEndFrame,1,1", R1Server.Terminator));
                        }
                        if (sendToR3)
                        {
                            rob3Data += string.Concat("VisionEndFrame,1,1", R3Server.Terminator);
                            R3Server.Write(false, rob3Data);

                            //R3Server.Write(false, string.Concat("VisionEndFrame,1,1", R3Server.Terminator));
                        }
                    }

                    #region Show Even side arrow
                    if (runtimeParameters.VisShowEvenSortOrder)
                    {
                        CogLineSegment myLine = new CogLineSegment();
                        myLine.EndX = 350;
                        myLine.EndY = 400;
                        myLine.StartX = myLine.EndX - Math.Sin(DegreesToRadians((int)runtimeParameters.VisEvenSortAngle + machineParameters.VisArrowAngleOffs)) * 35;
                        myLine.StartY = myLine.EndY - Math.Cos(DegreesToRadians((int)runtimeParameters.VisEvenSortAngle + machineParameters.VisArrowAngleOffs)) * 35;
                        myLine.StartPointAdornment = CogLineSegmentAdornmentConstants.Arrow;
                        myLine.Color = CogColorConstants.Red;
                        myLine.LineWidthInScreenPixels = 5;
                        myLine.SelectedSpaceName = "*";  //Pixel space
                        cogRecFlirLane1Raw.StaticGraphics.Add(myLine, "test");
                    }
                    #endregion

                    // Send to even side
                    foreach (RobTarget trans in finalSortedEven)
                    {

                        // Display a label
                        if (runtimeParameters.VisShowItemID)
                        {
                            idLabel.Color = CogColorConstants.Yellow;
                            idLabel.SetXYText(trans.X + 100, trans.Y - 50, "ID: " + trans.ID.ToString());
                            cogRecFlirLane1Raw.StaticGraphics.Add(idLabel, "main");
                        }

                        // Display sorted ID
                        if (runtimeParameters.VisShowEvenSortOrder)
                        {
                            //comWindow.UpdateComWindow("Sort Order", "Even ID: " + trans.ID.ToString());
                        }

                        r_degrees = trans.Theta * 180 / Math.PI; // Convert from radians to degrees
                        //comWindow.UpdateComWindow("R2R4",string.Concat("VL,2,2,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0")));

                        if (sendToR2)
                        {
                            //R2Server.Write(false, string.Concat("VL,2,2,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R2Server.Terminator));
                            rob2Data += string.Concat("VL,2,2,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R2Server.Terminator);

                        }
                        if (sendToR4)
                        {
                            //R4Server.Write(false, string.Concat("VL,2,2,", VisItemId.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R4Server.Terminator));
                            rob4Data += string.Concat("VL,2,2,", trans.ID.ToString(), ",", trans.X.ToString("0"), ",", trans.Y.ToString("0"), ",", r_degrees.ToString(".0"), R4Server.Terminator);

                        }
                    }

                    // Decide to send end frame message
                    if (totalEvenCount > 0)
                    {
                        if (sendToR2)
                        {
                            rob2Data += string.Concat("VisionEndFrame,2,2", R2Server.Terminator);
                            R2Server.Write(false, rob2Data);

                            //R2Server.Write(false, string.Concat("VisionEndFrame,2,2", R2Server.Terminator));

                        }
                        if (sendToR4)
                        {
                            rob4Data += string.Concat("VisionEndFrame,2,2", R4Server.Terminator);
                            R4Server.Write(false, rob4Data);

                            //R4Server.Write(false, string.Concat("VisionEndFrame,2,2", R4Server.Terminator));

                        }
                    }
                }

                LogClient.SendMessage(false, "In UserResultsAvailable: TotalOddCountThisFrame=" + TotalOddCountThisFrame.ToString());
                LogClient.SendMessage(false, "In UserResultsAvailable: TotalEvenCountThisFrame=" + TotalEvenCountThisFrame.ToString());


                // Perform a NUC operation after xx minutes and not a full frame (it was found that
                // a full frame is missed after manual NUC) The idea here is to do the NUC when little or no bars are in 
                // a vision frame so that we don't miss a whole row of product.

                // Set condition for timer done
                cond1 = (timerFlirNUCaction.ElapsedMilliseconds > (60 * machineParameters.ManualNUCtimeMinutes * 1000));

                // If frame is empty, perform the NUC
                cond2 = ((TotalOddCountThisFrame + TotalEvenCountThisFrame) == 0);

                if (cond1 && cond2)
                {

                    comWindow.UpdateComWindow("NUC", "Perfoming NUC");

                    // Perform the NUC operation
                    FlirNUC_operation((int)FLIR_Operation.PerformNUC);

                    // Disable again
                    FlirNUC_operation((int)FLIR_Operation.DisableNUC);

                }

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In mcjmAcq_UserResultAvailable, Exception: " + ex.Message);
                MessageBox.Show("mcjmAcq_UserResultAvailable Exception: " + ex.Message);
            }

        }

        /// <summary>
        /// Method used to send simulation locs to the robots.
        /// </summary>
        /// <param name="sendToR1"></param>
        /// <param name="sendToR2"></param>
        /// <param name="sendToR3"></param>
        /// <param name="sendToR4"></param>
        /// <param name="consoleShowlist"></param>
        private void SendSimLocs(bool sendToR1, bool sendToR2, bool sendToR3, bool sendToR4, bool consoleShowlist)
        {
            int simTotal = 13;
            int numCols = 1;    // 1 or 2 only
            double popsicleWidth = 190;
            double popsicleLength = 75;

            Random rnd = new Random();
            List<RobTarget> randListOdd = new List<RobTarget>();
            List<RobTarget> randListEven = new List<RobTarget>();

            RobTarget simLoc;

            //cogRecGreyRaw.Zoom = .33;

            textBoxBaslerStatus.Text = "Simulation mode";
            textBoxFlirStatus.Text = "Simulation mode";

            for (int ii = 0; ii < numCols; ii++)
            {
                // Create a list of locations
                for (int i = 0; i < simTotal; i++)
                {
                    VisItemId = (VisItemId % Properties.Settings.Default.VisionItemIDLimit) + 1;
                    simLoc.ID = VisItemId;
                    if (ii == 0)
                    {
                        simLoc.X = rnd.Next(1, 30) + rnd.Next(999) / 1000;
                    }
                    else
                    {
                        simLoc.X = rnd.Next(-210, -200) + rnd.Next(999) / 1000;
                    }

                    // Start negative and accumulate to positive for max spread across the belt
                    simLoc.Y = -550 + (i * (popsicleLength + 10)) + rnd.Next(999) / 1000;
                    simLoc.Z = 0;
                    simLoc.Theta = rnd.Next(-5, 5) + rnd.Next(999) / 1000;

                    if (simLoc.Y < 0)  // Zero assumed to be center of belt
                    {
                        randListOdd.Add(simLoc);
                    }
                    else
                    {
                        randListEven.Add(simLoc);
                    }

                }

                # region Show unsorted Odd list
                //Show unsorted Odd list
                if (consoleShowlist)
                {
                    Console.WriteLine("Unsorted Odd");
                    foreach (RobTarget trans in randListOdd)
                    {
                        Console.WriteLine("{0} {1}", trans.X.ToString("000.000"), trans.Y.ToString("000.000"));
                    }
                }
                #endregion

                // Sort the list of locations for the Odd side (Pick from outside to center of belt)
                var collectionOdd = from locOdd in randListOdd
                                    orderby locOdd.Y ascending
                                    select locOdd;

                #region Show Sorted Odd list
                // Show Sorted Odd list
                if (consoleShowlist)
                {
                    Console.WriteLine("Sorted Odd");
                    foreach (RobTarget trans in collectionOdd)
                    {
                        Console.WriteLine("{0} {1}", trans.X.ToString("000.000"), trans.Y.ToString("000.000"));
                    }
                }
                #endregion

                #region Show unsorted Even list
                //Show unsorted Even list
                if (consoleShowlist)
                {
                    Console.WriteLine("Unsorted Even");
                    foreach (RobTarget trans in randListEven)
                    {
                        Console.WriteLine("{0} {1}", trans.X.ToString("000.000"), trans.Y.ToString("000.000"));
                    }
                }
                #endregion

                // Sort the list of locations for the Even side (Pick from center to outside of belt)
                var collectionEven = from locEven in randListEven
                                     orderby locEven.Y ascending
                                     select locEven;

                #region Show Sorted Even list
                // Show Sorted Even list
                if (consoleShowlist)
                {
                    Console.WriteLine("Sorted Even");
                    foreach (RobTarget trans in collectionEven)
                    {
                        Console.WriteLine("{0} {1}", trans.X.ToString("000.000"), trans.Y.ToString("000.000"));
                    }
                }
                #endregion

                //MessageBox.Show("count:" + collection.Count<RobTarget>());

                // Define graphics objects
                CogRectangleAffine rectAffine = new CogRectangleAffine();
                CogGraphicLabel idLabel = new CogGraphicLabel();

                // Define image for display
                //CogImage16Grey img = new CogImage16Grey();
                //cogRecGreyRaw.Image = img;
                //cogRecGreyRaw.StaticGraphics.Clear();

                if (sendToR1)
                {
                    R1Server.Write(false, string.Concat("VisionNewFrame,1,1", R1Server.Terminator));
                }
                if (sendToR2)
                {
                    R2Server.Write(false, string.Concat("VisionNewFrame,2,2", R2Server.Terminator));
                }
                if (sendToR3)
                {
                    R3Server.Write(false, string.Concat("VisionNewFrame,1,1", R3Server.Terminator));
                }
                if (sendToR4)
                {
                    R4Server.Write(false, string.Concat("VisionNewFrame,2,2", R4Server.Terminator));
                }

                // Show graphics and send to odd side
                foreach (RobTarget loc in collectionOdd)
                {

                    // Define label parameters
                    idLabel.Color = CogColorConstants.Green;
                    idLabel.SetXYText(loc.X, loc.Y, loc.ID.ToString());
                    cogRecFlirLane1Raw.StaticGraphics.Add(idLabel, "main");

                    // Define rectangle for display
                    rectAffine.SetOriginLengthsRotationSkew(loc.X, loc.Y, popsicleWidth, popsicleLength, loc.Theta * (Math.PI / 180), 0);
                    cogRecFlirLane1Raw.StaticGraphics.Add(rectAffine, "main");

                    if (sendToR1)
                    {
                        R1Server.Write(false, string.Concat("VL,1,1,", loc.ID.ToString(), ",", loc.X.ToString("0"), ",", loc.Y.ToString("0"), ",", loc.Theta.ToString(".0"), R1Server.Terminator));
                    }
                    if (sendToR3)
                    {
                        R3Server.Write(false, string.Concat("VL,1,1,", loc.ID.ToString(), ",", loc.X.ToString("0"), ",", loc.Y.ToString("0"), ",", loc.Theta.ToString(".0"), R3Server.Terminator));
                    }
                }

                // Show graphics and send to even side
                foreach (RobTarget loc in collectionEven)
                {

                    // Define label parameters
                    idLabel.Color = CogColorConstants.Red;
                    idLabel.SetXYText(loc.X, loc.Y, loc.ID.ToString());
                    cogRecFlirLane1Raw.StaticGraphics.Add(idLabel, "main");

                    // Define rectangle for display
                    rectAffine.SetOriginLengthsRotationSkew(loc.X, loc.Y, popsicleWidth, popsicleLength, loc.Theta * (Math.PI / 180), 0);
                    cogRecFlirLane1Raw.StaticGraphics.Add(rectAffine, "main");

                    if (sendToR2)
                    {
                        R2Server.Write(false, string.Concat("VL,2,2,", loc.ID.ToString(), ",", loc.X.ToString("0"), ",", loc.Y.ToString("0"), ",", loc.Theta.ToString(".0"), R2Server.Terminator));
                    }
                    if (sendToR4)
                    {
                        R4Server.Write(false, string.Concat("VL,2,2,", loc.ID.ToString(), ",", loc.X.ToString("0"), ",", loc.Y.ToString("0"), ",", loc.Theta.ToString(".0"), R4Server.Terminator));
                    }
                }

                if (sendToR1)
                {
                    R1Server.Write(false, string.Concat("VisionEndFrame,1,1", R1Server.Terminator));
                }
                if (sendToR2)
                {
                    R2Server.Write(false, string.Concat("VisionEndFrame,2,2", R2Server.Terminator));
                }
                if (sendToR3)
                {
                    R3Server.Write(false, string.Concat("VisionEndFrame,1,1", R3Server.Terminator));
                }
                if (sendToR4)
                {
                    R4Server.Write(false, string.Concat("VisionEndFrame,2,2", R4Server.Terminator));
                }
            }
        }

        #endregion

        /// <summary>
        /// Send stored messages to robots. Messages are updated when a user changes a value on a form. The messages
        /// will be sent when the Visual Studio application is started or when there is a recipe change or when a robot
        /// comes on line via the 'R*_ServerConnected' event. Where * is the robot number.
        /// The message is in the format:
        ///     'R1pk1|TrgtOffs,1,1,1,12,0,22,0'
        /// Where:
        ///     'R1pk1' is the unique string key, the 2nd character is the robot number.
        ///     'TrgtOffs,1,1,1,12,0,22,0' is the stored message.
        /// By preceeding the stored message with a unique key, the message can be stored based on certain criteria.
        /// For example: 'R1pk1' means the TrgtOffs message is associated with Robot 1 and Pick head 1.
        /// </summary>
        private void UpdateRobot(int robotToUpdate, string[] messages, string terminator)
        {
            string msg;
            int robot, keylen;

            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i] != null)
                {
                    if (messages[i] != string.Empty)
                    {
                        // Compose message and send to robots
                        msg = messages[i];                                  // Work on a copy
                        robot = Convert.ToInt32(msg.Substring(1, 1));       // Extract the robot numer
                        keylen = msg.IndexOf('|') + 1;                      // Compute length of the key
                        msg = msg.Substring(keylen, msg.Length - keylen);   // Extract the actual message

                        if (robot == robotToUpdate)
                        {
                            switch (robot)
                            {
                                case 1:
                                    R1Server.Write(false, msg + terminator);
                                    break;
                                case 2:
                                    R2Server.Write(false, msg + terminator);
                                    break;
                                case 3:
                                    R3Server.Write(false, msg + terminator);
                                    break;
                                case 4:
                                    R4Server.Write(false, msg + terminator);
                                    break;
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Write keystring and message string to RuntimeParameters, used for sending robot data when
        /// starting application or recipe change
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        /// <param name="header"></param>
        public void UpdateRuntimeKeyString(string[] messages, string key, string header, string msg)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i] == null)
                {
                    messages[i] = key + "|" + header + msg;
                    break;
                }
                if (messages[i] == string.Empty)
                {
                    messages[i] = key + "|" + header + msg;
                    break;
                }

                // Check for key and store message
                if (messages[i].Contains(key))
                {
                    messages[i] = key + "|" + header + msg;
                    break;
                }

            }
        }
        /// <summary>
        /// Save runtime parameters to disk
        /// </summary>
        /// <param name="runtimeParameters"></param>
        public void SerializeRuntimeParameters(RuntimeParameters runtimeParameters)
        {

            TextWriter writeFileStream = null;

            // Serialize the object
            try
            {
                using (writeFileStream = new StreamWriter(this.JobsPath + this.RecipeName + ".xml"))
                {
                    XmlWriter writer = XmlWriter.Create(writeFileStream);

                    runtimeParmsXmlSerializer.Serialize(writer, runtimeParameters);

                    writeFileStream.Close();
                }
            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In SerializeRuntimeParameters, Exception: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (writeFileStream != null)
                    writeFileStream.Close();
            }

        }

        /// <summary>
        /// Update the runtime parameters from disk
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public RuntimeParameters DeserializeRuntimeParamters(string recipe, out bool success)
        {
            FileStream readFileStream = null;
            success = false;

            // Deserialize the object
            try
            {
                // Create a new file stream for reading the XML file
                using (readFileStream = new FileStream(this.JobsPath + recipe, FileMode.Open))
                {
                    XmlReader reader = XmlReader.Create(readFileStream);

                    // Deserialize object
                    runtimeParameters = (RuntimeParameters)this.runtimeParmsXmlSerializer.Deserialize(reader);
                    readFileStream.Close();
                    success = true;

                }
            }
            catch (FileNotFoundException ex)
            {
                StringBuilder st = new StringBuilder();
                st.Append("**** Recipe file not found!****\n");
                st.Append("\n");
                st.Append("Ensure the selected Recipe file is in the folder '" + this.JobsPath + "' and try again.");

                LogClient.SendMessage(false, "In DeserializeRuntimeParamters, FileNotFoundException Exception: " + ex.Message);
                MessageBox.Show(st.ToString(), "Warning!");
            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In DeserializeRuntimeParamters, Exception: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (readFileStream != null)
                {
                    readFileStream.Close();
                }

            }

            return runtimeParameters;
        }

        /// <summary>
        /// Method used to send message to a robot. This method is public to allow access from other objects.
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="msg"></param>
        public void SendMessageToRobot(int robot, string msg)
        {

            //MessageBox.Show(msg);

            switch (robot)
            {
                case 1:
                    R1Server.Write(false, msg + R1Server.Terminator);
                    break;
                case 2:
                    R2Server.Write(false, msg + R2Server.Terminator);
                    break;
                case 3:
                    R3Server.Write(false, msg + R3Server.Terminator);
                    break;
                case 4:
                    R4Server.Write(false, msg + R4Server.Terminator);
                    break;
                default:
                    MessageBox.Show("Invalid robot @sendMessageToRobot()");
                    break;
            }

        }

        private void checkBoxEnableRobotComm_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxOnLine.Checked)
            {
                SetVisionOnline();
            }
            else
            {
                if (!this.RecipeChangeover)
                {
                    // Ask to go offline
                    if (DialogResult.No == Utilities.AreYouSure())
                    {
                        checkBoxOnLine.Checked = true;
                        return;
                    }
                }

                SetVisionOffline(true);
            }

            this.Invalidate();  // Force redraw
        }

        private void SetVisionOffline(bool send)
        {
            this.VisionOnline = false;
            StartRobotServerThreads(false);
            timerPictureTimer.Stop();
        }

        private void SetVisionOnline()
        {
            this.VisionOnline = true;
            StartRobotServerThreads(true);
            timerPictureTimer.Start();

            // Perform the Flir NUC action
            FlirNUC_operation((int)FLIR_Operation.PerformNUC);

            // Redraw the main form
            this.Invalidate();
        }

        private void StartRobotServerThreads(bool start)
        {
            if (start)
            {
                // Start the Server threads
                R1Server.StartServer();
                R2Server.StartServer();
                R3Server.StartServer();
                R4Server.StartServer();
                buttonManualTrigger.Enabled = false;
            }
            else
            {
                // Issue commands to stop robots
                this.StopSystem(false);

                // Stop the Server threads
                R1Server.StopServer();
                R2Server.StopServer();
                R3Server.StopServer();
                R4Server.StopServer();
                buttonManualTrigger.Enabled = true;

            }
        }
        private void buttonAdjustParameters_Click(object sender, EventArgs e)
        {

            buttonSelectRecipe.Enabled = false;
            buttonShowComIO.Enabled = false;

            using (ParmLauncher pl = new ParmLauncher(this, runtimeParameters, mcjmAcq, machineParameters))
            {
                pl.Location = new Point(1000, 200);
                pl.TopMost = true;
                pl.ShowDialog();
            }

            buttonSelectRecipe.Enabled = true;
            buttonShowComIO.Enabled = true;

            this.Invalidate(); // Force redraw

        }

        private void buttonShowComIO_Click(object sender, EventArgs e)
        {
            //try
            //{
            comWindow.BringToFront();
            comWindow.Show();
            //}
            //catch { }

        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            bool windowsIsShuttingDown = false;

            // Check if Windows is shutting down.
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                windowsIsShuttingDown = true;
            }

            if (!windowsIsShuttingDown && !exitApp)
            {

                if (DialogResult.Yes != Utilities.AreYouSure())
                {
                    e.Cancel = true;
                    return;
                }

                // Take the vision offline
                SetVisionOffline(true);

                // Remove events
                R1Server.ServerStatusMessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R1_ServerStatusMessageUpdate);
                R1Server.ServerTCP_MessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R1_ServerTCP_MessageUpdate);
                R1Server.ServerConnected -= new EventHandler<AbbCom.ServerCustomEventArgs>(R1_ServerConnected);

                R2Server.ServerStatusMessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R2_ServerStatusMessageUpdate);
                R2Server.ServerTCP_MessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R2_ServerTCP_MessageUpdate);
                R2Server.ServerConnected -= new EventHandler<AbbCom.ServerCustomEventArgs>(R2_ServerConnected);

                R3Server.ServerStatusMessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R3_ServerStatusMessageUpdate);
                R3Server.ServerTCP_MessageUpdate -= new EventHandler<AbbCom.ServerCustomEventArgs>(R3_ServerTCP_MessageUpdate);
                R3Server.ServerConnected -= new EventHandler<AbbCom.ServerCustomEventArgs>(R3_ServerConnected);


                // Enable Thermal camera NUC before exiting
                FlirNUC_operation((int)MainForm.FLIR_Operation.EnableNUC);

                // Be sure to shudown the CogJobManager!!
                System.Threading.Thread.Sleep(1000);
                ShutdownCogJob(true);

                // Shutdown the data logger
                Process[] myProcesses = Process.GetProcessesByName("PopsicleDataLogger");
                if (Maf.Tools.Utilities.IsProcessRunning("PopsicleDataLogger"))
                {
                    foreach (Process myProcess in myProcesses)
                    {
                        myProcess.CloseMainWindow();
                    }

                }
            }
        }


        private void ShutdownCogJob(bool dispose)
        {
            if (!formDebug)
            {
                LogClient.SendMessage(false, "In ShutdownCogJob");
                try
                {
                    if (mcjmAcq != null)
                    {
                        // Patch to prevent the Pure Virtual Function call error (known issue)
                        CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
                        foreach (ICogFrameGrabber fg in frameGrabbers)
                        {
                            LogClient.SendMessage(false, "In ShutdownCogJob, start disconnecting frame grabber " + fg.Name);
                            fg.Disconnect(false);
                            LogClient.SendMessage(false, "In ShutdownCogJob, finish disconnecting frame grabber " + fg.Name);
                        }

                        mcjmAcq.UserResultAvailable -= new CogJobManager.CogUserResultAvailableEventHandler(mcjmAcq_UserResultAvailable);
                        if (dispose)
                        {
                            LogClient.SendMessage(false, "In ShutdownCogJob, start disposing cogRecGreyRaw");
                            cogRecFlirLane1Raw.Dispose();
                            LogClient.SendMessage(false, "In ShutdownCogJob, finish disposing cogRecGreyRaw");

                            LogClient.SendMessage(false, "In ShutdownCogJob, start disposing cogRecThermRaw");
                            cogRecFlirLane2Raw.Dispose();
                            LogClient.SendMessage(false, "In ShutdownCogJob, finish disposing cogRecThermRaw");
                        }

                        LogClient.SendMessage(false, "In ShutdownCogJob, start mcjmAcq.Stop()");
                        mcjmAcq.Stop();
                        LogClient.SendMessage(false, "In ShutdownCogJob, finish mcjmAcq.Stop()");

                        LogClient.SendMessage(false, "In ShutdownCogJob, start mcjmAcq.Shutdown()");
                        mcjmAcq.Shutdown();
                        LogClient.SendMessage(false, "In ShutdownCogJob, finish mcjmAcq.Shutdown()");
                    }
                }
                catch (Exception ex)
                {
                    LogClient.SendMessage(false, "In ShutdownCogJob, Exception: " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Method used to start the system. (when the green start button is pressed on the ParmLauncher window)
        /// </summary>
        public void StartSystem()
        {

            if (!R1Server.Running)
            {
                MessageBox.Show("Vision is offline. Unable to start system.", "Warning!");
                return;
            }

            R1Server.Write(false, "start system" + R1Server.Terminator);
            R3Server.Write(false, "start system" + R3Server.Terminator);

        }

        /// <summary>
        /// Method used to stop the system. (when the red stop button is pressed on the ParmLauncher window)
        /// </summary>
        public void StopSystem(bool warn)
        {

            // Ask to stop robots
            if (warn)
            {
                if (DialogResult.No == Utilities.AreYouSure()) { return; }
            }

            R1Server.Write(false, "stop system" + R1Server.Terminator);
            R3Server.Write(false, "stop system" + R3Server.Terminator);

            // Give time to send message
            System.Threading.Thread.Sleep(2000);

        }
        private void buttonSelectRecipe_Click(object sender, EventArgs e)
        {

            SelectRecipe();

        }

        private void SelectRecipe()
        {
            using (AbbCom.Forms.SelectRecipe sr = new AbbCom.Forms.SelectRecipe(this, machineParameters))
            {
                sr.ShowDialog();


                bool success = false;

                if ((sr.SelectedRecipe != string.Empty) && (sr.SelectedRecipe != "None"))
                {
                    success = SelectRecipeRuntime(false, true, sr.SelectedRecipe, true);
                    if (!success) { return; }
                }
            }
        }

        /// <summary>
        /// Select a recipe. A recipe consists of 2 files, the recipe name with a .xml extention which contains all
        /// robot and vision adjustable parameters and the recipe name with a .vpp extention which is the Quickbuild
        /// job name. The files are located in the jobs directory 'this.JobsPath'.
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        private bool SelectRecipeRuntime(bool autoSelect, bool showErr, string recipeName, bool openPort)
        {
            bool success;

            try
            {
                FirstPass = true;

                // Disable the SaveAs pulldown
                saveAsToolStripMenuItem.Enabled = false;

                // Verify the recipe files exists
                if (!System.IO.File.Exists(string.Concat(this.JobsPath, recipeName, ".xml")))
                {
                    if (showErr)
                        if (autoSelect)
                        {
                            MessageBox.Show(string.Concat("Recipe File '", recipeName, ".xml' USED FOR AUTO SELECTING does not exist @path ", JobsPath), "Warning!");
                        }
                        else
                        {
                            MessageBox.Show(string.Concat("Recipe File '", recipeName, ".xml' does not exist @path ", JobsPath), "Warning!");
                        }
                    return false;
                }
                if (!System.IO.File.Exists(string.Concat(this.JobsPath, recipeName, ".vpp")))
                {
                    if (showErr)
                    {
                        MessageBox.Show(string.Concat("QuickBuild File '", recipeName, ".vpp' does not exist @path ", JobsPath), "Warning!");
                    }
                    return false;
                }

                // Get the runtime parameters
                runtimeParameters = DeserializeRuntimeParamters(string.Concat(recipeName, ".xml"), out success);
                if (!success) { return false; }

                // Update Main form title
                this.RecipeName = recipeName;
                this.Text = string.Concat(CurrentLoggedOnUser, " - ", formTitle, this.RecipeName);

                labelRecipeNotSelected.Visible = false;

                // Initialize the Cog Job Manager
                if (!formDebug)
                {
                    InitializeJobManager(this.JobsPath, this.RecipeName);
                    UpdateQuickBuild();
                }

                // Serialize to update any new object members (developer change);
                SerializeRuntimeParameters(runtimeParameters);

                // Reset stats
                ResetStats();

                // Save the current recipe as the default recipe
                machineParameters.DefaultRecipe = recipeName;
                this.SaveMachineParameters(machineParameters);

                // Enable controls now that recipe is selected
                EnableControls(CurrentLoggedOnUser);

                // Force the form to redraw (Paint event will fire)
                this.Invalidate();

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In SelectRecipeRuntime, Exception: " + ex.Message);
                comWindow.UpdateComWindow("Recipe Change", "Exception in 'SelectRecipeRuntime':" + ex.Message);
                return false;
            }

            return true;
        }

        public void UpdateQuickBuild()
        {


            try
            {

                CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(this.CogJobName).VisionTool;
                CogBlobTool myTool;
                CogPMAlignTool myPMAlignTool;


                // Set the property to indicate the job is Pattern Match compatible
                PatternMatchCompatible = QuickBuildFindTool(lctg, "FindWrapperPatterns");


                // Send min/max pick angles
                lctg.SetScriptTerminalData("MinPickAngle", runtimeParameters.VisMinAngle);
                lctg.SetScriptTerminalData("MaxPickAngle", runtimeParameters.VisMaxAngle);


                // Send Blob related information
                if (!runtimeParameters.VisUsePatternMatch)
                {
                    // Send Basler threshold
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.SegmentationParams.HardFixedThreshold = (int)runtimeParameters.VisGreyThreshold;

                    // Send Basler min area
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeLow = (double)runtimeParameters.VisGreyAreaMin;

                    // Send Basler max area
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeHigh = (double)runtimeParameters.VisGreyAreaMax;
                }

                // Send Flir threshold
                myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                myTool.RunParams.SegmentationParams.HardFixedThreshold = (int)runtimeParameters.VisThermThreshold;

                // Send Flir min area
                myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                myTool.RunParams.RunTimeMeasures[0].FilterRangeLow = (double)runtimeParameters.VisThermAreaMin;

                // Send Flir max area
                myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                myTool.RunParams.RunTimeMeasures[0].FilterRangeHigh = (double)runtimeParameters.VisThermAreaMax;

                // Send Pattern Match parameters
                if (runtimeParameters.VisUsePatternMatch && PatternMatchCompatible)
                {
                    // Send Acceptance Threshold
                    myPMAlignTool = (CogPMAlignTool)lctg.Tools["FindWrapperPatterns"];
                    myPMAlignTool.RunParams.AcceptThreshold = (double)runtimeParameters.VisPatMatchAcceptThreshold;

                    // Send scaling
                    myPMAlignTool.RunParams.ZoneScale.Low = runtimeParameters.VisPatRunParamsZoneScaleLow;
                    myPMAlignTool.RunParams.ZoneScale.High = runtimeParameters.VisPatRunParamsZoneScaleHigh;

                    // Send Min/Max Zone angles
                    myPMAlignTool.RunParams.ZoneAngle.Low = (double)runtimeParameters.VisPatRunParamsZoneAngleLow;
                    myPMAlignTool.RunParams.ZoneAngle.High = (double)runtimeParameters.VisPatRunParamsZoneAngleHigh;
                    myPMAlignTool.RunParams.ZoneAngle.Overlap = (double)runtimeParameters.VisPatRunParamsZoneAngleOverlap;

                    // Send Contrast threshold
                    myPMAlignTool.RunParams.ContrastThreshold = runtimeParameters.VisPatRunParamsContrastThreshold;

                    // Send Coarse Acceptance params
                    myPMAlignTool.RunParams.CoarseAcceptThresholdEnabled = runtimeParameters.VisPatRunParamsCoarseAcceptThresholdChecked;
                    myPMAlignTool.RunParams.CoarseAcceptThreshold = runtimeParameters.VisPatRunParamsCoarseAcceptThreshold;

                    // Send Elasticity
                    myPMAlignTool.Pattern.Elasticity = runtimeParameters.VisPatRunParamsElasticity;

                    // Send the pattern clipping window params
                    lctg.SetScriptTerminalData("VisXwindowMin", runtimeParameters.VisXwindowMin);
                    lctg.SetScriptTerminalData("VisXwindowMax", runtimeParameters.VisXwindowMax);


                    // Send the Flir Region adjustment parameters
                    lctg.SetScriptTerminalData("VisFlirRegionXadj", runtimeParameters.VisFlirRegionXadj);
                    lctg.SetScriptTerminalData("VisFlirRegionYadj", runtimeParameters.VisFlirRegionYadj);

                }

                // Send Therm disable flag
                lctg.SetScriptTerminalData("DisableHistogramInspection", runtimeParameters.VisDisableTherm);

                // Send Minimum Hist Count
                lctg.SetScriptTerminalData("MinPopsicleHistCount", runtimeParameters.VisThermMinHistCount);

                // Send the show area flag
                lctg.SetScriptTerminalData("ShowArea", runtimeParameters.VisShowArea);

                // Send the show hist count flag
                lctg.SetScriptTerminalData("ShowHistCount", runtimeParameters.VisShowHistCount);

                // Send the Flir Region X,Y size
                lctg.SetScriptTerminalData("VisSideXLength", runtimeParameters.VisSideXLength);
                lctg.SetScriptTerminalData("VisSideYLength", runtimeParameters.VisSideYLength);

            }
            catch (Exception ex)
            {
                LogClient.SendMessage(false, "In UpdateQuickBuild, Exception: " + ex.Message);
                MessageBox.Show(ex.Message, "Runtime Exception @UpdateQuickBuild");
            }

        }

        /// <summary>
        /// Check for tool existance
        /// </summary>
        /// <param name="lctg"></param>
        /// <param name="toolName"></param>
        /// <returns></returns>
        private static bool QuickBuildFindTool(CogToolGroup cogTool, string toolName)
        {
            bool found = false;
            for (int i = 0; i < cogTool.Tools.Count; i++)
            {
                if (cogTool.Tools[i].Name.Contains(toolName))
                {
                    found = true;
                }
            }
            return found;
        }

        /// <summary>
        /// Check for Function Key press (emulate Insight behavior).
        /// F4 = Select recipe.
        /// F5 = Manual Trigger.
        /// F6 = Popup parameter adjustment page.
        /// F7 = Show Com I/O page.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            bool handled = false;        // we haven't handled this key

            if ((keyData == Keys.F4) && (this.RecipeName != string.Empty) && (!this.VisionOnline))
            {
                buttonSelectRecipe_Click(null, null);
                handled = true;       // we handled the key press
            }
            else if ((keyData == Keys.F5) && (this.RecipeName != string.Empty) && (!this.VisionOnline))
            {
                ManualTrigger_Click(null, null);
                handled = true;       // we handled the key press
            }
            else if ((keyData == Keys.F6) && (this.RecipeName != string.Empty))
            {
                buttonAdjustParameters_Click(null, null);
                handled = true;       // we handled the key press
            }
            else if (keyData == Keys.F7)
            {
                buttonShowComIO_Click(null, null);
                handled = true;       // we handled the key press
            }

            // ## Special key combination for developers
            if (keyData == (Keys.Alt | Keys.T))
            {
                ManualTrigger_Click(null, null);
                handled = true;       // we handled the key press
            }

            return (handled || base.ProcessCmdKey(ref msg, keyData));
        }

        private void defaultRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Setup su = new Setup(this, runtimeParameters, machineParameters))
            {
                su.ShowDialog();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox ab = new AboutBox(GetInstalledVersionFromRegistry("PopsicleImaging")))
            {
                ab.ShowDialog();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = this.JobsPath;
                sfd.Filter = "QuickBuild files (*.vpp)|*.vpp";
                sfd.Title = "Save Recipe As.";
                sfd.ShowDialog();


                try
                {
                    if (sfd.FileName != string.Empty)
                    {
                        // Save the file pairs
                        string selectedName = Path.GetFileNameWithoutExtension(sfd.FileName);
                        string selectedPath = Path.GetDirectoryName(sfd.FileName);
                        File.Copy(string.Concat(this.JobsPath, this.RecipeName, ".vpp"), string.Concat(selectedPath, "\\", selectedName, ".vpp"), true);
                        File.Copy(string.Concat(this.JobsPath, this.RecipeName, ".xml"), string.Concat(selectedPath, "\\", selectedName, ".xml"), true);
                    }
                }
                catch (Exception ex)
                {
                    LogClient.SendMessage(false, "In toolStripMenuItemSaveAs_Click, Exception: " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void deleteRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = this.JobsPath;
                ofd.Filter = "QuickBuild files (*.vpp)|*.vpp";
                ofd.Title = "Select a Quickbuild file to Delete.";
                ofd.ShowDialog();

                try
                {
                    if (ofd.FileName != string.Empty)
                    {
                        if (DialogResult.Yes == Utilities.AreYouSure())
                        {
                            // Check if the recipe to delete is the current selected recipe and clear out data
                            if (this.RecipeName == Path.GetFileNameWithoutExtension(ofd.FileName))
                            {
                                labelRecipeNotSelected.Visible = true;
                                machineParameters.DefaultRecipe = "";
                                this.Text = string.Concat(formTitle, "");

                                // Serialize to update machine parameters;
                                this.SaveMachineParameters(machineParameters);
                            }


                            // Delete the file pairs
                            string selectedName = Path.GetFileNameWithoutExtension(ofd.FileName);
                            string selectedPath = Path.GetDirectoryName(ofd.FileName);
                            File.Delete(string.Concat(selectedPath, "\\", selectedName, ".vpp"));
                            File.Delete(string.Concat(selectedPath, "\\", selectedName, ".xml"));
                            ofd.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.SendMessage(false, "In toolStripMenuItemDeleteRecipe_Click, Exception: " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OpenCognexQuickBuildToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Method not used, kept for reference.

            // Be sure to shudown the CogJobManager!!
            //ShutdownCogJob(false);

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Cognex\VisionPro\bin\Cognex.VisionPro.QuickBuild.exe");
            //ProcessStartInfo startInfo = new ProcessStartInfo(this.JobsPath + this.RecipeName + ".vpp");

            if (!Utilities.IsProcessRunning("Cognex.VisionPro.QuickBuild"))
            {

                //startInfo.Arguments = string.Concat(this.JobsPath, this.RecipeName, ".vpp");

                Process.Start(startInfo);

                Popup.Message("Starting QuickBuild, wait...");
                System.Threading.Thread.Sleep(3000);
                Popup.Close();


            }
            else
            {
                // Find QuickBuild and maximize
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.ProcessName == "Cognex.VisionPro.QuickBuild")
                    {
                        ShowWindow(process.MainWindowHandle, 3); // 3 = Maximize (see pinvoke.net)
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }

        }

        private void saveVisionParametersToQuickBuildToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveVisionParametersToQuickBuild();
        }

        public bool SaveVisionParametersToQuickBuild()
        {
            bool saved = false;

            string str = "The following operation will save all vision parameters to the '";
            str = string.Concat(str, this.JobsPath, this.RecipeName, ".vpp' Vision Pro QuickBuild disk file.\n");

            // Save vision paramaters to QuickBuild
            if (DialogResult.OK == MessageBox.Show(str, "Warning!", MessageBoxButtons.OKCancel))
            {
                CogSerializer.SaveObjectToFile(mcjmAcq, this.JobsPath + this.RecipeName + ".vpp", typeof(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter));
                saved = true;
            }

            return saved;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // No operation is implemented. Parameters are saved each time the <OK> button is pressed in any window.
            // Save is visible for compatability purposes.

            Popup.Message("Please wait, saving parameters...");
            System.Threading.Thread.Sleep(5000);
            Popup.Close();

        }

        public void ResetStats()
        {
            this.VisionProcessTime = 0;
            this.VisionItemsLocated = 0;
            this.VisionAngleFailures = 0;
            this.VisionThermalFailures = 0;
        }

        private void selectRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectRecipe();
        }

        private void timerPictureTimer_Tick(object sender, EventArgs e)
        {

            if (runtimeParameters.VisPictureFrequency >= 250 && jobManagerInitialized)
            {
                RunQuickBuildJob();
            }

        }

        private void timerFlirAcqFifo_Tick(object sender, EventArgs e)
        {

            CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();

            ICogFrameGrabber myFg = null;

            foreach (ICogFrameGrabber fg in frameGrabbers)
            {
                if (fg.Name.Contains("FLIR"))
                {
                    myFg = fg;
                }
            }

            int focusPos = 0;
            if (myFg != null)
            {
                focusPos = Convert.ToInt32(myFg.OwnedGigEAccess.GetIntegerFeature("FocusPos"));
                comWindow.UpdateComWindow("Flir", "Focus Pos: " + focusPos.ToString());
            }


            //if (myFg != null)
            //myFg.OwnedGigEAccess.SetIntegerFeature("FocusSpeed", 0);


        }


        public void CloseAllForms()
        {
            //Create a Collection to Store all Opened Forms.
            List<Form> formsList = new List<Form>();

            //Collect all opened forms into a Collection.
            foreach (Form frm in Application.OpenForms)
            {
                //Execulde the Current Form.
                if (frm.Name == "MainForm")     // Exclude the main form
                {
                    continue;
                }
                else if (frm.Name == "ComIO")   // Hide the com I/O form
                {
                    frm.Hide();
                }
                else
                {
                    formsList.Add(frm);
                }
            }
            //Now Close the forms
            foreach (Form frm in formsList)
            {
                frm.Close();
            }
        }

        private void timerRunRecipeChange_Tick(object sender, EventArgs e)
        {
            bool allClosed = true;

            //Create a Collection to Store all Opened Forms.
            List<Form> formsList = new List<Form>();
            try
            {
                //All all opened forms into a Collection.
                foreach (Form frm in Application.OpenForms)
                {
                    //Execulde the Current Form.
                    if (frm.Name == "MainForm")     // Exclude the main form
                    {
                        continue;
                    }
                    else if (frm.Name == "ComIO")   // Exclude the com I/O form
                    {
                        continue;
                    }
                    else
                    {
                        formsList.Add(frm);
                    }
                }
                foreach (Form frm in formsList)
                {
                    if (!frm.IsDisposed)
                    {
                        allClosed = false;
                    }
                }
                if (allClosed)
                {
                    //Console.WriteLine("All forms are closed...");

                    // Stop this timer
                    timerRunRecipeChange.Stop();

                    // Go online
                    SetVisionOnline();

                    // Reset Recipe Changeover property
                    this.RecipeChangeover = false;

                }
            }
            catch (Exception ex)
            {

                // Reset Recipe Changeover property
                this.RecipeChangeover = false;

                // Stop this timer
                timerRunRecipeChange.Stop();

                LogClient.SendMessage(false, "In timerRunRecipeChange_Tick, Exception: " + ex.Message);

            }

        }

        private void engineeringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool success;
            machineParameters = machineParameters.DeserializeMachineConfigParameters(out success);
            if (!success) { Environment.Exit(0); }

            using (Engineering engineeringForm = new Engineering(LogClient, machineParameters, runtimeParameters, this))
            {
                engineeringForm.HeartbeatEvent += new Engineering.PropertyChangedHeartbeatControlEventHandler(engineeringForm_HeartbeatEvent);
                engineeringForm.ShowDialog();
            }

            // Serialize to update machine parameters;
            this.SaveMachineParameters(machineParameters);

        }

        /// <summary>
        /// Event handler to start or stop the heartbeat timer.
        /// </summary>
        /// <param name="cmd"></param>
        public void engineeringForm_HeartbeatEvent(string cmd)
        {
            switch (cmd)
            {
                case "StartHeartbeatTimer":
                    timerDataLoggerHeartbeat.Start();
                    LogClient.SendMessage(false, "Heartbeat");
                    break;
                case "StopHeartbeatTimer":
                    timerDataLoggerHeartbeat.Stop();
                    break;
                default:
                    break;
            }
        }

        private void timerDataLoggerHeartbeat_Tick(object sender, EventArgs e)
        {
            LogClient.SendMessage(false, "Heartbeat");
        }

        private void saveDataModulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!R1Server.Connected || !R3Server.Connected)
            {
                MessageBox.Show("Unable to save robot data. Check that the vision is Online and ensure robots are connected.", "Warning!");
                return;
            }

            R1Server.Write(false, "SaveDataModules" + R1Server.Terminator);
            R3Server.Write(false, "SaveDataModules" + R3Server.Terminator);

            Popup.Message("Saving robot data... See FlexPendant for acknowledgment.");
            System.Threading.Thread.Sleep(4000);
            Popup.Close();
        }

        private void openRecipeFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Get list of processes and Restore window if the process is running
            if (Utilities.IsProcessRunning("explorer"))
            {
                Process[] processes = Process.GetProcessesByName("explorer");

                foreach (Process process in processes)
                {
                    // Check to see if the window is all ready available (prevent cascading windows)
                    if (process.MainWindowTitle == "PopsicleImagingJobs")
                    {
                        ShowWindow(process.MainWindowHandle, 9); // 9 = SW_RESTORE
                        SetWindowPos(process.MainWindowHandle.ToInt32(), -1, 100, 50, 950, 700, 0x0040);
                        return;
                    }
                }

                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\explorer.exe", @"/E," + JobsPath);
                Process proc = Process.Start(startInfo);

            }

        }

        /// <summary>
        /// Get the Installed version from the Registry.
        /// </summary>
        /// <param name="p_name"></param>
        /// <returns>Version</returns>
        public static string GetInstalledVersionFromRegistry(string p_name)
        {
            string displayName;
            string displayVersion = null;
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return displayVersion;
                    }
                }
            }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return displayVersion;
                    }
                }
            }

            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return displayVersion;
                    }
                }
            }

            // NOT FOUND
            return null;
        }

        /// <summary>
        /// Remove the top right close button of a form.
        /// </summary>
        /// <param name="frm"></param>
        static void RemoveCloseButton(Form frm)
        {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(frm.Handle, false);
            if (hMenu != IntPtr.Zero)
            {
                n = GetMenuItemCount(hMenu);
                if (n > 0)
                {
                    RemoveMenu(hMenu, (uint)(n - 1), MF_BYPOSITION | MF_REMOVE);
                    RemoveMenu(hMenu, (uint)(n - 2), MF_BYPOSITION | MF_REMOVE);
                    DrawMenuBar(frm.Handle);
                }
            }
        }

        /// <summary>
        /// Verify the CogJob exists in Quickbuild. Returns default CogJob name if initial search fails.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="cogJobName"></param>
        /// <param name="defaultJobName"></param>
        /// <returns></returns>
        public bool VerifyCogJobExists(string path, string file, string cogJobName, out string defaultJobName)
        {
            // Search for CogJob name
            bool found = false;
            CogJob myJob;
            defaultJobName = string.Empty;
            for (int i = 0; i < mcjmAcq.JobCount; i++)
            {
                myJob = mcjmAcq.Job(i);
                found = found || (string.Compare(myJob.Name, cogJobName) == 0);
                if (!found)
                {
                    defaultJobName = myJob.Name;
                }
            }

            return found;
        }

        public string GetUserAccountName(int idx)
        {
            if (machineParameters.UserAccountName[idx] == null)
            {
                return string.Empty;
            }
            else
            {
                return machineParameters.UserAccountName[idx];
            }
        }
        public void SetUserAccountName(int idx, string name)
        {
            machineParameters.UserAccountName[idx] = name;
        }
        public string GetUserAccountPassword(int idx)
        {
            return machineParameters.UserAccountPassword[idx];
        }
        public string GetUserAccountPassword(string userName)
        {
            string password = string.Empty;
            for (int i = 0; i < machineParameters.UserAccountName.Length; i++)
            {
                if (machineParameters.UserAccountName[i].Equals(userName))
                {
                    password = machineParameters.UserAccountPassword[i];
                    break;
                }
            }

            return password;
        }
        public void SetUserAccountPassword(int idx, string password)
        {
            machineParameters.UserAccountPassword[idx] = password;
        }
        public int GetUserAccountIndex(string userName)
        {
            int index = 0;
            for (int i = 0; i < machineParameters.UserAccountName.Length; i++)
            {
                if (machineParameters.UserAccountName[i].Equals(userName))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        public int GetUserAccountLevel(int idx)
        {
            return machineParameters.UserAccountLevel[idx];
        }
        public int GetUserAccountLevel(string userName)
        {
            int level = 1;
            for (int i = 0; i < machineParameters.UserAccountName.Length; i++)
            {
                if (machineParameters.UserAccountName[i].Equals(userName))
                {
                    level = this.GetUserAccountLevel(i);
                    break;
                }
            }

            return level;
        }
        public string GetUserAccountLevelName(int level)
        {

            if (level == 1)
            {
                return "Locked";
            }
            else if (level == 2)
            {
                return "Protected";
            }
            else if (level == 3)
            {
                return "Full";
            }
            else
            {
                return "Locked";
            }



        }

        public void SetUserAccountLevel(int idx, int level)
        {
            machineParameters.UserAccountLevel[idx] = level;
        }
        public int GetNextUserAccountIndex()
        {
            int nextIndex = 99;

            for (int i = 2; i < machineParameters.UserAccountName.Length; i++)
            {
                if (machineParameters.UserAccountName[i] == null)
                {
                    nextIndex = i;
                    break;
                }
                if (machineParameters.UserAccountName[i].Equals(string.Empty))
                {
                    nextIndex = i;
                    break;
                }

            }

            if (nextIndex == 99)
            {
                throw new ArgumentOutOfRangeException("User Account Index, Account database full!");
            }

            return nextIndex;
        }

        public void SaveMachineParameters(MachineConfig machineParameters)
        {
            bool success = false;

            // Serialize to update any new object members (developer change);
            machineParameters.SerializeMachineConfigParameters(machineParameters, out success);
            if (!success) { Environment.Exit(0); }

            // Load in the Machine Config parameters
            machineParameters = machineParameters.DeserializeMachineConfigParameters(out success);
            if (!success) { Environment.Exit(0); }
        }

        /// <summary>
        /// Enable controls based on logged on user
        /// </summary>
        /// <param name="user"></param>
        private void EnableControls(string user)
        {

            int level;

            level = this.GetUserAccountLevel(user);

            // Enable controls based on user access level
            switch (level)
            {

                case 1: //Monitor

                    // File menu
                    selectRecipeToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    deleteRecipeToolStripMenuItem.Enabled = false;
                    exitToolStripMenuItem.Enabled = false;

                    // Configuration menu
                    defaultRecipeToolStripMenuItem.Enabled = false;

                    // Advanced menu
                    saveVisionParametersToQuickBuildToolStripMenuItem.Enabled = false;
                    saveRobotDataModulesToolStripMenuItem.Enabled = false;
                    engineeringToolStripMenuItem.Enabled = false;
                    openRecipeFolderToolStripMenuItem.Enabled = false;
                    userAccessSettingsToolStripMenuItem.Enabled = false;

                    // System menu
                    logonToolStripMenuItem.Enabled = true;
                    optionsToolStripMenuItem.Enabled = false;
                    toolStripMenuItemBackup.Enabled = false;


                    // Other controls
                    buttonAdjustParameters.Enabled = false;
                    buttonSelectRecipe.Enabled = false;
                    buttonManualTrigger.Enabled = false;
                    buttonShowComIO.Enabled = false;
                    checkBoxOnLine.Enabled = false;
                    //backupToolStripMenuItem.Enabled = false;

                    break;

                case 2:  //Maint

                    // File menu
                    selectRecipeToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    deleteRecipeToolStripMenuItem.Enabled = true;
                    exitToolStripMenuItem.Enabled = false;

                    // Configuration menu
                    defaultRecipeToolStripMenuItem.Enabled = true;

                    // Advanced menu
                    saveVisionParametersToQuickBuildToolStripMenuItem.Enabled = true;
                    saveRobotDataModulesToolStripMenuItem.Enabled = true;
                    engineeringToolStripMenuItem.Enabled = true;
                    openRecipeFolderToolStripMenuItem.Enabled = true;
                    userAccessSettingsToolStripMenuItem.Enabled = true;

                    // System menu
                    logonToolStripMenuItem.Enabled = true;
                    optionsToolStripMenuItem.Enabled = false;
                    toolStripMenuItemBackup.Enabled = true;


                    // Other controls
                    buttonAdjustParameters.Enabled = false;
                    if (this.RecipeName != "None" && this.RecipeName != string.Empty)
                    {
                        buttonAdjustParameters.Enabled = true;
                    }
                    buttonSelectRecipe.Enabled = true;
                    buttonShowComIO.Enabled = true;
                    if (this.VisionOnline)
                    {
                        buttonManualTrigger.Enabled = false;
                    }
                    else
                    {
                        buttonManualTrigger.Enabled = true;
                    }
                    checkBoxOnLine.Enabled = true;
                    //backupToolStripMenuItem.Enabled = true;

                    break;

                case 3: //Bpr, Admin

                    // File menu
                    selectRecipeToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    deleteRecipeToolStripMenuItem.Enabled = true;
                    exitToolStripMenuItem.Enabled = true;

                    // Configuration menu
                    defaultRecipeToolStripMenuItem.Enabled = true;

                    // Advanced menu
                    saveVisionParametersToQuickBuildToolStripMenuItem.Enabled = true;
                    saveRobotDataModulesToolStripMenuItem.Enabled = true;
                    engineeringToolStripMenuItem.Enabled = true;
                    openRecipeFolderToolStripMenuItem.Enabled = true;
                    userAccessSettingsToolStripMenuItem.Enabled = true;

                    // System menu
                    logonToolStripMenuItem.Enabled = true;
                    optionsToolStripMenuItem.Enabled = true;
                    toolStripMenuItemBackup.Enabled = true;

                    // Other controls
                    buttonAdjustParameters.Enabled = false;
                    if (this.RecipeName != "None" && this.RecipeName != string.Empty)
                    {
                        buttonAdjustParameters.Enabled = true;
                    }
                    buttonSelectRecipe.Enabled = true;
                    buttonShowComIO.Enabled = true;
                    if (this.VisionOnline)
                    {
                        buttonManualTrigger.Enabled = false;
                    }
                    else
                    {
                        buttonManualTrigger.Enabled = true;
                    }
                    checkBoxOnLine.Enabled = true;
                    //backupToolStripMenuItem.Enabled = true;

                    break;

                default:  //Monitor

                    // File menu
                    selectRecipeToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    deleteRecipeToolStripMenuItem.Enabled = false;
                    exitToolStripMenuItem.Enabled = false;

                    // Configuration menu
                    defaultRecipeToolStripMenuItem.Enabled = false;

                    // Advanced menu
                    saveVisionParametersToQuickBuildToolStripMenuItem.Enabled = false;
                    saveRobotDataModulesToolStripMenuItem.Enabled = false;
                    engineeringToolStripMenuItem.Enabled = false;
                    openRecipeFolderToolStripMenuItem.Enabled = false;
                    userAccessSettingsToolStripMenuItem.Enabled = false;

                    // System menu
                    logonToolStripMenuItem.Enabled = true;
                    optionsToolStripMenuItem.Enabled = false;
                    toolStripMenuItemBackup.Enabled = false;


                    // Other controls
                    buttonAdjustParameters.Enabled = false;
                    buttonSelectRecipe.Enabled = false;
                    buttonManualTrigger.Enabled = false;
                    buttonShowComIO.Enabled = false;
                    checkBoxOnLine.Enabled = false;

                    break;
            }

        }


        private void userAccessSettingstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (UserAccessSettings pe = new UserAccessSettings(this, machineParameters))
            {
                pe.ShowDialog();
                this.SaveMachineParameters(machineParameters);
            }
        }

        private void logonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (UserLogOnOff logOn = new UserLogOnOff(this, CurrentLoggedOnUser, machineParameters))
            {
                logOn.ShowDialog();


                if (logOn.LogOnSuccess)
                {
                    CurrentLoggedOnUser = logOn.NewUser;
                    this.Invalidate();
                    timerUserLogon.Start();
                }
            }
        }

        private void timerUserLogon_Tick(object sender, EventArgs e)
        {
            CurrentLoggedOnUser = machineParameters.StartupUserName;
            timerUserLogon.Stop();
            this.Invalidate();
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SystemOptions so = new SystemOptions(this, machineParameters))
            {
                so.ShowDialog();
            }
        }

        private string ComputeBackupPath(string machineName, string selectedPath)
        {

            string path = string.Empty;
            string[] folders = Directory.GetDirectories(selectedPath);
            int val = 0;
            int highestVal = 0;
            int maxVal = 999;
            int notUsed;
            string defaultMachineName = System.Net.Dns.GetHostName();

            foreach (string item in folders)
            {
                if (item.Length >= 4) // Must be correct length i.e. '.000'
                {
                    if (int.TryParse(item.Substring(item.Length - 3, 3), out notUsed)) //Last 3 must be numerical
                    {
                        if (item.Substring(item.Length - 4, 1) == ".") //Must have a dot before last 3 digits
                        {
                            if (item.Contains(machineName)) // Folder must contain machine name
                            {
                                // Calculate the next contiguous value
                                val = Convert.ToInt32(item.Substring(item.Length - 3, 3)) + 1;
                                if (val > highestVal)
                                {
                                    highestVal = val;
                                }
                            }
                        }
                    }
                }
            }

            // Create the folder
            if (highestVal <= maxVal)
            {
                if (machineParameters.MachineName != string.Empty)
                {
                    defaultMachineName = machineParameters.MachineName;
                }
                path = string.Concat(selectedPath, "\\", defaultMachineName, ".", highestVal.ToString("000"));
                Directory.CreateDirectory(path);
                //MessageBox.Show(highestVal.ToString("000"));
            }
            else
            {
                MessageBox.Show("Maximum number of backups allowed.", "Warning");
                return null;
            }

            return path;
        }

        private void toolStripMenuItemBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                string VppFile = string.Empty;
                string XmlFile = string.Empty;
                string computedPath = string.Empty;
                fbd.SelectedPath = string.Empty;
                //fbd.ShowNewFolderButton = false;
                fbd.Description = "Select backup folder";
                fbd.SelectedPath = machineParameters.DefaultBackupPath;

                DialogResult dr = fbd.ShowDialog();

                try
                {
                    if (dr == DialogResult.OK)
                    {
                        // Update the default backup path
                        machineParameters.DefaultBackupPath = fbd.SelectedPath;

                        string[] VppFiles = Directory.GetFiles(this.JobsPath, "*.vpp");
                        string[] XmlFiles = Directory.GetFiles(this.JobsPath, "*.xml");

                        // Compute next contiguous path in the format 'MachineName.000' to 'MachineName.999'
                        computedPath = ComputeBackupPath(machineParameters.MachineName, fbd.SelectedPath);

                        if (computedPath != null)
                        {
                            Popup.Message("Copying files, wait...");

                            // Copy all files from the jobs path to the 
                            foreach (string item in VppFiles)
                            {
                                VppFile = Path.GetFileName(item.ToString());
                                File.Copy(string.Concat(this.JobsPath, "\\", VppFile), string.Concat(computedPath, "\\", VppFile), true);
                            }
                            foreach (string item in XmlFiles)
                            {
                                XmlFile = Path.GetFileName(item.ToString());
                                File.Copy(string.Concat(this.JobsPath, "\\", XmlFile), string.Concat(computedPath, "\\", XmlFile), true);
                            }

                            Popup.Close();

                            Popup.Message("Backup Complete.");
                            System.Threading.Thread.Sleep(3000);
                            Popup.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.SendMessage(false, "In toolStripMenuItemBackup_Click, Exception: " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Popup.Close();
                }
            }
        }

    }

}
