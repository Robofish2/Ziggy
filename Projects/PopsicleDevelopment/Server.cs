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
    /// Class used to send and receive messages from a client.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// The application should subcribe to the custom event handlers to receive information from this object.
        ///  </summary>

        /// <summary>
        /// Custom Event Handler to send TCP/IP status message update to subscribers.
        /// </summary>
        public event EventHandler<ServerCustomEventArgs> ServerStatusMessageUpdate;
        /// <summary>
        /// Custom Event Handler to send TCP/IP message to subscribers.
        /// </summary>
        public event EventHandler<ServerCustomEventArgs> ServerTCP_MessageUpdate;
        /// <summary>
        /// Custom Event Handler to inform subscribers when the Connected propery changes.
        /// </summary>
        public event EventHandler<ServerCustomEventArgs> ServerConnected;

        /// <summary>
        ///  Server Connected Property.
        /// </summary>
        public bool Connected { get { return _sc; } set { _sc = value; } }
        private bool _sc;

        /// <summary>
        ///  Server Running Property.
        /// </summary>
        public bool Running { get { return _sr; } set { _sr = value; } }
        private bool _sr;

        /// <summary>
        ///  Server Response Property.
        /// </summary>
        public string Response { get { return _resp; } set { _resp = value; } }
        private string _resp = "\x06";

        /// <summary>
        ///  Server Port Property.
        /// </summary>
        public int ListenPort { get { return _lp; } set { _lp = value; } }
        private int _lp = 0;

        /// <summary>
        ///  Client IP Address Property for informational purposes only.
        /// </summary>
        public string ClientIPaddress { get { return _cip; } set { _cip = value; } }
        private string _cip = string.Empty;

        /// <summary>
        ///  Server Stopped Property.
        /// </summary>
        public bool ServerStopped { get { return _sstopped; } set { _sstopped = value; } }
        private bool _sstopped = false;

        /// <summary>
        ///  Message receive timeout Property.
        /// </summary>
        public int ReceiveTimeout { get { return _rt; } set { _rt = value; } }
        private int _rt = 5000;  //5 Secs.

        /// <summary>
        ///  Server Start of Text Property.
        /// </summary>
        public string STX { get { return _stx; } }
        private string _stx = "\x02";

        /// <summary>
        ///  Server End of Text Property.
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
        ///  Server message terminator Property.
        /// </summary>
        public string Terminator { get { return _term; } }
        private string _term = "\r\n";

        /// <summary>
        ///  Server Name Property.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        private string _name = "Server";

        /// <summary>
        ///  Indicates that a Heartbeat message will be sent.
        /// </summary>
        public bool SendHeartbeat { get { return _hb; } set { _hb = value; } }
        private bool _hb = false;

        /// <summary>
        ///  Heartbeat frequency
        /// </summary>
        public int HeartbeatFrequency { get { return _hbt; } set { _hbt = value; } }
        private int _hbt = 30;  //30 Secs.

        private Queue<string> serverQ = new Queue<string>();
        private Object qLocker = new Object();

        /// <summary>
        /// Method to flush the Server queue.
        /// </summary>
        public void FlushQueue()
        {
            serverQ.Clear();
        }

        /// <summary>
        /// Method to stop the Server.
        /// </summary>
        public void StopServer()
        {
            ServerStopped = true;
            FlushQueue();
            
        }

        /// <summary>
        /// Method to fire event to subscribers to update a status message.
        /// </summary>
        /// <param name="s">The text message.</param>
        void OnServerStatusMessageUpdate(string s)
        {

            // Copy to a temporary variable to be thread-safe.
            EventHandler<ServerCustomEventArgs> copy = ServerStatusMessageUpdate;

            // Add newline character to wrap text to next line when displaying in textbox
            //s = string.Concat(DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss "), s, "\n");
            s = string.Concat(s, "\n");

            // If event is wired, fire it!
            if (copy != null)
            {

                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ServerCustomEventArgs(s) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ServerCustomEventArgs(s));
                }

            }

        }

        /// <summary>
        /// Method to fire event to subscribers so the TCP/IP message can be processed.
        /// </summary>
        /// <param name="s"></param>
        void OnServerTCP_MessageUpdate(string s)
        {
            if (this.ServerStopped) { return; }

            // Copy to a temporary variable to be thread-safe.
            EventHandler<ServerCustomEventArgs> copy = ServerTCP_MessageUpdate;

            // If event is wired, fire it!
            if (copy != null)
            {
                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ServerCustomEventArgs(s) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ServerCustomEventArgs(s));
                }
            }
        }

        /// <summary>
        /// Method to fire event to subscribers. Subscriber can read the Server.Connected property
        /// to update a control.
        /// </summary>
        /// <param name="b"></param>
        void OnServerConnected(bool b)
        {
            // Make a copy for thread safety
            EventHandler<ServerCustomEventArgs> copy = ServerConnected;

            // If event is wired, fire it!
            if (copy != null)
            {
                // Get the target and safe cast it.
                Control target = copy.Target as Control;

                // If the target is a control and invoke is required,
                // invoke it; otherwise just fire it normally.
                if (target != null && target.InvokeRequired)
                {
                    object[] args = new object[] { null, new ServerCustomEventArgs(b) };
                    target.Invoke(copy, args);
                }
                else
                {
                    copy(null, new ServerCustomEventArgs(b));
                }

            }
        }

        /// <summary>
        ///  Method run as a thread to receive Client messages.
        /// </summary>
        public void ServerHandler()
        {

            // Initialize variables
            string msg = string.Empty;
            string messageToSend = string.Empty;
            string msgFiltered = string.Empty;
            byte[] buffer = new byte[2048];

            Socket serverSocket = null;
            Stopwatch swHeartBeat = new Stopwatch();

            Connected = false;
            OnServerConnected(Connected);

            ASCIIEncoding asen = new ASCIIEncoding();

            // Initializes the Listener
            TcpListener listener = new TcpListener(IPAddress.Any, Convert.ToInt32(ListenPort));

            this.Running = true;
            string sendString = string.Empty;

            while (!this.ServerStopped)
            {
                try
                {

                    // Start listening for incomming messages.
                    listener.Start();

                    string str = Convert.ToString(ClientIPaddress) + ":" + ListenPort;
                    OnServerStatusMessageUpdate(this.Name + ": Server waiting for connection from: " + str);
                    Connected = false;
                    OnServerConnected(Connected);

                    // Wait for connection
                    while (!listener.Pending() && !ServerStopped)
                    {
                        Thread.Sleep(10);
                    }

                    if (ServerStopped)
                        continue;

                    serverSocket = listener.AcceptSocket();

                    // Debug code block to catch possible conflict
                    IPEndPoint ep = (IPEndPoint)serverSocket.LocalEndPoint;
                    if (ep.Port != this.ListenPort)
                        OnServerTCP_MessageUpdate("Local End Point:" + ep.Port.ToString() + "  Listen Port:" + ListenPort.ToString());

                    OnServerStatusMessageUpdate(this.Name + ": Server connected");
                    Connected = true;
                    OnServerConnected(Connected);

                    swHeartBeat.Start();

                    while (serverSocket.Connected && !ServerStopped)
                    {

                        // Init buffer count (number of tokens)
                        int bufferCount = 0;

                        // Poll the socket to check for client message
                        serverSocket.Poll(10000, SelectMode.SelectRead);
                        if (serverSocket.Available > 0)
                        {
                            // Read the TCP/IP line
                            bufferCount = serverSocket.Receive(buffer);

                            for (int i = 0; i < bufferCount; i++)
                            {
                                if (buffer[i] != 10)
                                {
                                    sendString = sendString + (char)buffer[i];
                                }
                                else
                                {

                                    // Filter out the non readable characters.
                                    msgFiltered = MessageFilter(sendString);
                                    OnServerStatusMessageUpdate("<" + this.Name + ": Server recieved:" + msgFiltered);

                                    // Fire event and send the message to the subscriber. It is expected for the
                                    // subscriber to update the Server.Response property.
                                    OnServerTCP_MessageUpdate(sendString);
                                    sendString = string.Empty;
                                }
                            }

                        }
                        else
                        {
                            // Process messages in the queue
                            if (serverQ.Count > 0)
                            {
                                // Send message to Client
                                messageToSend = serverQ.Dequeue();
                                serverSocket.Send(asen.GetBytes(messageToSend));
                                msgFiltered = MessageFilter(messageToSend);
                                OnServerStatusMessageUpdate(">" + this.Name + ": Server sent: " + msgFiltered);
                                swHeartBeat.Reset();
                                swHeartBeat.Start();
                            }

                            if (this.SendHeartbeat)
                            {
                                if (swHeartBeat.ElapsedMilliseconds > this.HeartbeatFrequency * 1000)
                                {
                                    messageToSend = "Heartbeat" + this.Terminator;
                                    serverSocket.Send(asen.GetBytes(messageToSend));
                                    //OnServerStatusMessageUpdate(">" + this.Name + ": Server sent: " + messageToSend);
                                    swHeartBeat.Reset();
                                    swHeartBeat.Start();
                                }
                            }

                        }

                        if (ServerStopped)
                            continue;

                    }

                }
                catch (Exception ex)
                {

                    OnServerStatusMessageUpdate(this.Name + ": Server Exception>" + ex.Message);
                    Thread.Sleep(500);
                }

                finally
                {
                    /* clean up */
                    if (serverSocket != null)
                    {
                        if (!serverSocket.Connected)
                        {
                            Connected = false;
                            OnServerConnected(Connected);
                            OnServerStatusMessageUpdate(this.Name + ": Server not connected");
                        }
                    }
                }
            }

            // Clean up when server exits
            OnServerStatusMessageUpdate(this.Name + ": Server stopped");
            Connected = false;
            OnServerConnected(Connected);

            this.Running = false;
            listener.Stop();
            if (serverSocket != null)
            {
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
            }

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
        /// Method used to start the Server Handler thread.
        /// </summary>
        public void StartServer()
        {

            if (this.Running) { return; }

            // Start the Server thread
            Thread myServerThread = new Thread(new ThreadStart(this.ServerHandler));

            ServerStopped = false;
            myServerThread.IsBackground = true;
            myServerThread.Start();

            // Loop until the server thread activates.
            while (!myServerThread.IsAlive) ;

        }

        /// <summary>
        /// Composes a message and place in the queue for processing.
        /// </summary>
        /// <param name="showError"></param>
        /// <param name="msg"></param>
        /// <param name="parms"></param>
        /// <remarks></remarks>
        /// <returns>Operator response code.</returns>
        public Utilities.OperatorResponse Write(bool showError, string msg)
        {
            string errMsg;

            if (msg == null || msg == "")
            {
                return Utilities.OperatorResponse.Success;
            }

            if (!Connected)
            {
                if (showError)
                {
                    errMsg = this.Name + ": Server not connected to Client";
                    Utilities.RuntimeError(MessageBoxButtons.OK, 0, errMsg);
                    return Utilities.OperatorResponse.Success;
                }
                else
                {
                    // Ignore the request and exit
                    return Utilities.OperatorResponse.Success;
                }

            }

            // Initialize the return value
            Utilities.OperatorResponse error = Utilities.OperatorResponse.Success;

            // Popup an error if the Server Handler is busy
            while ((serverQ.Count > 0) && showError)
            {
                if (!ServerStopped)
                {
                    errMsg = "***" + this.Name + ": Server Task is busy***";
                    error = Utilities.RuntimeError(MessageBoxButtons.RetryCancel, 0, errMsg);
                    if (error != Utilities.OperatorResponse.Retry)
                        return error;
                }
            }

            lock (qLocker)
            {
                serverQ.Enqueue(msg);
            }
            return error;
        }

    }

    /// <summary>
    /// EventArgs derived class to hold Custom Event Arg data
    /// </summary>
    public class ServerCustomEventArgs : EventArgs
    {
        /// <summary>
        ///  Server Message Property.
        /// </summary>
        public string Message { get { return _msg; } }
        private string _msg;

        /// <summary>
        ///  Server Connected Property.
        /// </summary>
        public bool Connected { get { return _con; } }
        private bool _con;

        /// <summary>
        /// Constructors.
        /// </summary>
        public ServerCustomEventArgs()
        {
        }
        /// <summary>
        /// Server Event args for type string
        /// </summary>
        /// <param name="s"></param>
        public ServerCustomEventArgs(string s)
        {
            _msg = s;
        }
        /// <summary>
        /// Server Event args for type bool
        /// </summary>
        /// <param name="b"></param>
        public ServerCustomEventArgs(bool b)
        {
            _con = b;
        }
    }
}
