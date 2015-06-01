using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TCPApplicationDemo
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        #region MemberVariables
        const int bufferLength = 20;
        #endregion

        #region Events
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            Thread tcpServerRunThread = new Thread(new ThreadStart(TcpSeverRun));
            tcpServerRunThread.Start();
            btnStartServer.Enabled = false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Starts Listening to incoming messages
        /// </summary>
        private void TcpSeverRun()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 5004);
            tcpListener.Start();
            UpdateUI("Listening");
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpHandlerThread.Start(client);
            }
        }
        /// <summary>
        /// Receives the incoming message and update it to UI simultaneously
        /// </summary>
        /// <param name="client"></param>
        private void tcpHandler(object client)
        {
            try
            { 
            using (TcpClient lclient = (TcpClient)client)
            {
                NetworkStream stream = lclient.GetStream();
                byte[] message = new byte[bufferLength];
                while (true)
                {
                    if(message!= null)
                    { 
                        stream.Read(message, 0, message.Length);
                        UpdateUI(Encoding.ASCII.GetString(message));
                    }
                }
            }
            
            }
            catch(Exception ex)
            {

            }

        }

        private void UpdateUI(string s)
        {
            Func<int> del = delegate()
            {
                rtxtBox.AppendText(s + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);

        }
        #endregion
    }
}
