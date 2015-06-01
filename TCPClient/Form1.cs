using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region MemberVariables
        const int readBufferLength = 20;
        TcpClient client = new TcpClient();
        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog Dlg = new OpenFileDialog())
            {
                Dlg.Filter = "All Files (*.*)|*.*";
                Dlg.CheckFileExists = true;
                Dlg.Title = "Choose a File";
                Dlg.InitialDirectory = @"C:\";
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = Dlg.FileName;
                }
                Dlg.Dispose();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] SendingBuffer = null;
                
                client.Connect(IPAddress.Parse("127.0.0.1"), 5004);
                NetworkStream stream = client.GetStream();
                using (BinaryReader b = new BinaryReader(File.Open(txtFile.Text, FileMode.Open)))
                {
                    // 2.
                    // Position and length variables.
                    int pos = 0;
                    // 2A.
                    // Use BaseStream.
                    int length = (int)b.BaseStream.Length;
                    while (pos < length)
                    {
                        // 3.
                        // Read bytes.
                        if ((length - pos) < readBufferLength)
                        {
                            int remainingBytes = length - pos;
                            SendingBuffer = b.ReadBytes(remainingBytes);

                            // richTextBox1.Text += v.ToString();
                            stream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
                            break;

                        }
                        else
                        {
                            SendingBuffer = b.ReadBytes(readBufferLength);
                            stream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);

                        }
                        // 4.
                        // Advance our position variable.
                        pos += sizeof(long);
                        Thread.Sleep(100);
                    }
                }
                //
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            if (txtFile.Text != "")
            {
                btnSend.Enabled = true;
            }
            else
            {
                btnSend.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //client.Close();

        }


    }
}
