using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5_WInForm
{
    public partial class Form1 : Form
    {
        private Socket client;
        private byte[] buffer = new byte[1024];

        public Form1()
        {
            InitializeComponent();
            
        }

        private async void btnConnect_Click(object sender, EventArgs e)

        {
            
            
            try
            {
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var port = 8005;
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                await client.ConnectAsync(new IPEndPoint(ipAddress, port));
                AddText($"Client connected to {ipAddress}:{port}");
                _ = Task.Run(ReceiveLoop);
            }
            catch (Exception ex)
            {
                txtOutput.AppendText($"Error connecting to server: {ex.Message}");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var message = txtMessage.Text;
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                txtOutput.AppendText($"Client sent message: \"{message}\"");
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                txtOutput.AppendText($"Error sending message: {ex.Message}");
            }
        }

        private async Task ReceiveLoop()
        {
            try
            {
                while (true)
                {
                    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);
                    if (response == "")
                    {
                        AddText($"Client received acknowledgment: \"{response}\"");
                        break;
                    }
                    else
                    {
                        AddText($"Client received message: \"{response}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                txtOutput.AppendText($"Error receiving message: {ex.Message}");
            }
        }

        private void AddText(string text)
        {
            txtOutput.Text += (text + "\n");
        
        }

       
    }
}
