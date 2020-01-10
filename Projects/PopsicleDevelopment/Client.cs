using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Maf.Tools;

namespace AbbCom
{
    /// <summary>
    /// Class used to send messages to a Server.
    /// <para>
    /// Step1: Subscribe to Client events in target class constructor, i.e:
    /// </para>
    /// <para>
    /// myClient.ClientStatusMessageUpdate +=...
    /// </para>
    /// <para>
    /// myClient.ClientTCP_MessageUpdate +=...
    /// </para>
    /// <para>
    /// myClient.ClientConnected +=...
    /// </para>
    /// <para>
    /// Step2: Set the Client object properties i.e: (Code in step 2 and 3 can be placed in a form load method)
    /// </para>
    ///  <para>
    ///  myClient.ServerIPaddress = XmlData.ServerIPAddr;
    ///  </para>
    ///  <para>
    ///  myClient.ServerPort = XmlData.ServerPort;
    /// </para>
    /// <para>
    /// Step3: Start the Client by:
    /// </para>
    /// <para>
    /// myClient.StartClient()
    /// </para>
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Custom Event Handler to send text message to subscribers.
        /// Step1:
        ///  Create a Client object i.e. myClient = new Client();
        /// Step2: 
        ///  Subscribe to the event by placing the following statement in the target class constructor:
        ///  myClient.ClientMessageUpdate +=new EventHandler|ClientCustomEventArgs|(Client_ClientMessageUpdate);
        /// Step3:
        ///  Add the following method to the target class:
        ///  void Client_ClientMessageEvent(object sender, ClientCustomEventArgs args)
        ///  {
        ///    textBox1.Text = args.Message; // Or whatever control to update.
        ///  }
        //  </summary>
        public event EventHandler<ClientCustomEventArgs> ClientStatusMessageUpdate;
        /// <summary>
        /// Custom Event Handler to send TCP/IP message to subscribers.
        /// </summary>
        public event EventHandler<ClientCustomEventArgs> ClientTCP_MessageUpdate;

        /// <summary>
        /// Custom Event Handler to inform subscribers when the Connected propery changes.
        /// </summary>
        public event EventHandler<ClientCustomEventArgs> ClientConnected;

        /// <summary>
        ///  Client Connected Property.
        /// </summary>
        public bool Connected { get { return _cc; } set { _cc = value; } }
        private bool _cc;

        /// <summary>
        ///  Server Port Property.
        /// </summary>
        public int ServerPort { get { return _sp; } set { _sp = value; } }
        private int _sp = 0;

        /// <summary>
        ///  Server IP Address Property.
        /// </summary>
        public string ServerIPaddress { get { return _sip; } set { _sip = value; } }
        private string _sip = string.Empty;

        /// <summary>
        ///  Client Response Property.
        /// </summary>
        public string Response { get { return _resp; } set { _resp = value; } }
        private string _resp = "\x06";

        /// <summary>
        ///  Client Default Response Property.
        /// </summary>
        public string DefaultResponse { get { return _dresp; } set { _dresp = value; } }
        private string _dresp = "\x06";

        /// <summary>
        ///  Client Start of Text Property.
        /// </summary>
        public string STX { get { return _stx; } }
        private string _stx = "\x02";

        /// <summary>
        ///  Client End of Text Property.
        /// </summary>
        public string ETX { get { return _etx; } }
        private string _etx = "\x03";

        /// <summary>
        ///  Indicates that STX and ETX should be used on message transactions.
        /// </summary>
        public bool Affix { get { return _af; } set { _af = value; } }
        private bool _af = false;

        /// <summary>
        ///  Message delimiter.
        /// </summary>
        public string Delim { get { return _delim; } set { _delim = value; } }
        private string _delim = ",";

        /// <summary>
        ///  Client Terminator Property.
        /// </summary>
        public string Terminator { get { return _term; } }
        private string _term = "\r\n";

        /// <summary>
        ///  Client Stopped Property.
        /// </summary>
        public bool ClientStopped { get { return _cstopped; } set { _cstopped = value; } }
        private bool _cstopped = false;

        /// <summary>
        ///  Message receive timeout Property.
        /// </summary>
        public int ReceiveTimeout { get { return _rt; } set { _rt = value; } }
        private int _rt = 5000;  //5 Secs.

        private Queue<string> clientQ = new Queue<string>();
        private Object qLocker = new Object();

        /// <summary>
        ///  Cliennt Running Property.
        /// </summary>
        public bool Running { get { return _cr; } set { _cr = value; } }
        private bool _cr;

        /// <summary>
        ///  Property used to wait for ack or return value from a Server
        /// </summary>
        public bool WaitForServerAck { get { return _wfsa; } set { _wfsa = value; } }
        private bool _wfsa = false;

        /// <summary>
        /// Method to fire event to subscribers to update a status message.
        /// </summary>
        /// <param name="s">Text message.</param>
        void OnClientStatusMessageUpdate(string s)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ClientCustomEventArgs> copy = ClientStatusMessageUpdate;

            // If event is wired, fire it!
            if (copy != null)
            {

                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ClientCustomEventArgs(s) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ClientCustomEventArgs(s));
                }

            }

        }

        /// <summary>
        /// Method to fire event to subscribers so the TCP/IP message can be processed.
        /// </summary>
        /// <param name="s"></param>
        void OnClientTCP_MessageUpdate(string s)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ClientCustomEventArgs> copy = ClientTCP_MessageUpdate;

            // If event is wired, fire it!
            if (copy != null)
            {
                // Initialize response property.
                Response = string.Empty;

                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ClientCustomEventArgs(s) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ClientCustomEventArgs(s));
                }

                // Default Response property to <ACK>
                if (Response == string.Empty)
                    Response = DefaultResponse;

            }
        }

        /// <summary>
        /// Method to fire event to subscribers. Subscriber can read the Client.Connected property
        /// to update a control.
        /// </summary>
        /// <param name="b"></param>
        void OnClientConnected(bool b)
        {
            // Make a copy for thread safety
            EventHandler<ClientCustomEventArgs> copy = ClientConnected;

            // If event is wired, fire it!
            if (copy != null)
            {
                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ClientCustomEventArgs(b) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ClientCustomEventArgs(b));
                }

            }
        }

        /// <summary>
        /// Method to stop the Client
        /// </summary>
        public void StopClient()
        {
            ClientStopped = true;
        }

        // 
        /// <summary>
        /// Composes a message and place in the queue for processing.
        /// </summary>
        /// <param name="showError"></param>
        /// <param name="msg"></param>
        /// <param name="parms"></param>
        /// <remarks></remarks>
        /// <returns>Operator response code.</returns>
        public Utilities.OperatorResponse SendMessage(bool showError, string msg, string[] parms)
        {
            string msgParms = "";
            string errMsg;

            if (msg == null || msg == "")
            {
                return Utilities.OperatorResponse.Success;
            }

            if (!Connected)
            {
                errMsg = "Client not connected to Server";
                Utilities.RuntimeError(MessageBoxButtons.OK, 0, errMsg);
                return Utilities.OperatorResponse.Success;

            }

            // Initialize the return value
            Utilities.OperatorResponse error = Utilities.OperatorResponse.Success;

            // Popup an error if the Client Handler is busy
            while ((clientQ.Count > 0) && showError)
            {
                if (!ClientStopped)
                {
                    errMsg = "***Client Task is busy***";
                    error = Utilities.RuntimeError(MessageBoxButtons.RetryCancel, 0, errMsg);
                    if (error != Utilities.OperatorResponse.Retry)
                    return error;
                }
            }

            lock (qLocker)
            {
                // Compose the message
                for (int i = 0; i < parms.Length; i++)
                {
                    if(parms[i] != "")
                        msgParms = msgParms + Delim + parms[i];
                }
                if (this.Affix)
                {
                    clientQ.Enqueue(this.STX + msg + msgParms + this.ETX);
                }
                else
                {
                    clientQ.Enqueue(msg + msgParms + this.Terminator);
                }
            }
            return error;
        }

        /// <summary>
        /// Send a message to a Server
        /// </summary>
        /// <param name="showError"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Utilities.OperatorResponse SendMessage(bool showError, string msg)
        {
            string errMsg;

            if (msg == null || msg == "" || !this.Running)
            {
                return Utilities.OperatorResponse.Success;
            }

            if (!this.Connected && showError)
            {
                errMsg = "Client not connected to Server";
                Utilities.RuntimeError(MessageBoxButtons.OK, 0, errMsg);
                return Utilities.OperatorResponse.Success;

            }

            // Initialize the return value
            Utilities.OperatorResponse error = Utilities.OperatorResponse.Success;

            // Popup an error if the Client Handler is busy
            while ((clientQ.Count > 0) && showError)
            {
                if (!ClientStopped)
                {
                    errMsg = "***Client Task is busy***";
                    error = Utilities.RuntimeError(MessageBoxButtons.RetryCancel, 0, errMsg);
                    if (error != Utilities.OperatorResponse.Retry)
                        return error;
                }
            }

            lock (qLocker)
            {
                if (this.Affix)
                {
                    clientQ.Enqueue(this.STX + msg + this.ETX);
                }
                else
                {
                    clientQ.Enqueue(msg + this.Terminator);
                }
            }
            return error;
        }

        /// <summary>
        /// Method run as a thread to send Client messages
        /// </summary>
        public void ClientHandler()
        {
            // Initialize variables
            string msg = string.Empty;
            string msgFiltered = string.Empty;
            byte[] buffer = new byte[1024];

            Socket clientSocket = null;
            this.Running = true;

            while (!ClientStopped)
            {

                OnClientStatusMessageUpdate("Client waiting for Server connection to:"
                    + Convert.ToString(this.ServerIPaddress)
                    + ":" + this.ServerPort);

                // Create the socket connection
                IPEndPoint ipep =
                new IPEndPoint(IPAddress.Parse(this.ServerIPaddress), this.ServerPort);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    bool done = false;
                    Connected = false;
                    OnClientConnected(Connected);

                    // Try to connect, keep looping if exception is thrown (server may not be connected)
                    done = false;
                    while (!done && !ClientStopped)
                    {
                        Thread.Sleep(10);
                        try
                        {
                            clientSocket.Connect(ipep);
                            done = true;
                        }
                        catch { }
                    }

                    if (ClientStopped)
                        continue;

                    Connected = true;
                    OnClientConnected(Connected);
                    OnClientStatusMessageUpdate("Client connected");

                    while (clientSocket.Connected && !ClientStopped)
                    {
                        // We need this to prevent CPU usage problem
                        Thread.Sleep(10);

                        if (clientQ.Count > 0)
                        {

                            // Send message to Server
                            ASCIIEncoding asen = new ASCIIEncoding();
                            string messageToSend = clientQ.Dequeue();
                            clientSocket.Send(asen.GetBytes(messageToSend));
                            msgFiltered = MessageFilter(messageToSend);
                            OnClientStatusMessageUpdate(">Client sent: " + msgFiltered);

                            // Optionally read the TCP/IP line for Ack/return parameters
                            if (this.WaitForServerAck)
                            {

                            // Timeout value before Receive will throw an
                            // exception if no data received.
                            clientSocket.ReceiveTimeout = ReceiveTimeout;
                                int bufferCount = clientSocket.Receive(buffer);

                                if (bufferCount > 0)
                                {
                                    // Encode the string from the byte buffer
                                    msg = Encoding.UTF8.GetString(buffer, 0, bufferCount);

                                    // Filter out the non readable characters.
                                    msgFiltered = MessageFilter(msg);
                                    OnClientStatusMessageUpdate("<Client received:" + msgFiltered);

                                    // Fire event and send the message to the subscriber. It is expected for the
                                    // subscriber to update the Client.Response property.
                                    OnClientTCP_MessageUpdate(msg);
                                }
                            }
                        }
                    } // while

                    /* clean up */
                    if (clientSocket != null)
                        clientSocket.Close();
                    Connected = false;
                    OnClientConnected(Connected);

                }

                catch (Exception genError)
                {
                    if (!ClientStopped)
                    {
                        OnClientStatusMessageUpdate("Execption Error: " + genError.Message);

                        //MessageBox.Show(
                        //    String.Format("Unhandled Execption: {0} ", genError.Message), "Error!",
                        //    MessageBoxButtons.OK,
                        //    MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    /* clean up */
                    if (clientSocket != null)
                    {
                        //clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                    }
                    Connected = false;
                    OnClientConnected(Connected);
                    OnClientStatusMessageUpdate("Client connection closed");

                }

            }

            this.Running = false;
            OnClientStatusMessageUpdate("Client stopped");
            Connected = false;
            OnClientConnected(Connected);

        }

        /// <summary>
        /// Replace the non printable characters with human readable text.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string MessageFilter(string msg)
        {

            if ((msg.Contains(Convert.ToString('\x06'))))
            {
                msg = msg.Replace(Convert.ToString('\x06'), "<ACK>");
            }
            else if ((msg.Contains(Convert.ToString('\x02'))))
            {
                msg = msg.Replace(Convert.ToString('\x02'), "<STX>");
            }
            else if ((msg.Contains(Convert.ToString('\x03'))))
            {
                msg = msg.Replace(Convert.ToString('\x03'), "<ETX>");
            }
            else if ((msg.Contains(Convert.ToString("\r\n"))))
            {
                msg = msg.Replace(Convert.ToString("\r\n"), "<CRLF>");
            }
            else if ((msg.Contains(Convert.ToString('\r'))))
            {
                msg = msg.Replace(Convert.ToString('\r'), "<CR>");
            }
            else if ((msg.Contains(Convert.ToString('\n'))))
            {
                msg = msg.Replace(Convert.ToString('\n'), "<LF>");
            }

            return msg;
        }

        /// <summary>
        /// Method used to start the Client Handler thread.
        /// </summary>
        public void StartClient()
        {

            // Start the Client thread
            Thread myClientThread = new Thread(new ThreadStart(this.ClientHandler));

            ClientStopped = false;
            myClientThread.IsBackground = true;
            myClientThread.Start();

            // Loop until the server thread activates.
            while (!myClientThread.IsAlive) ;

        }
    }


    /// <summary>
    /// EventArgs derived class to hold Custom Event Arg data
    /// </summary>
    public class ClientCustomEventArgs : EventArgs
    {
        /// <summary>
        ///  Client Message Property.
        /// </summary>
        public string Message { get { return _msg; } }
        private string _msg;

        /// <summary>
        ///  Client Connected Property.
        /// </summary>
        public bool Connected { get { return _con; } }
        private bool _con;

        /// <summary>
        /// Constructors.
        /// </summary>
        public ClientCustomEventArgs()
        {
        }
        /// <summary>
        /// Client Custom Event args for type string
        /// </summary>
        /// <param name="s"></param>
        public ClientCustomEventArgs(string s)
        {
            _msg = s;
        }
        /// <summary>
        /// Client Custom Event args for type bool
        /// </summary>
        /// <param name="b"></param>
        public ClientCustomEventArgs(bool b)
        {
            _con = b;
        }
    }
}
