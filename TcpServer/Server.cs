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


namespace TcpServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopServer = true;
        private readonly int _serverPort = 8080;
        private delegate void SafeCallDelegate(string text);
        public void UpdateHistorySafeCall(string text)
        {
            if (stopServer)
            {
                return;
            }
            if (historyTextBox.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateHistorySafeCall);
                historyTextBox.Invoke(d, new object[] { text });
            }
            else
            {
                historyTextBox.AppendText(text + Environment.NewLine);
            }
        }

        public string ReadMenuFromFile()
        {
            string menu = "";
            try
            {
                using (StreamReader sr = new StreamReader("menu.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        menu += line + Environment.NewLine;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi đọc file: " + e.Message);
            }
            return menu;
        }
        private void Server_Load(object sender, EventArgs e)
        {
            string menu = ReadMenuFromFile();
            UpdateHistorySafeCall("Menu đã load");
            UpdateHistorySafeCall(menu);
        }
        public async Task Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, _serverPort);
                tcpListener.Start();
                statusLabel.Text = @"Status: Server is listening on port " + _serverPort;
                Server_Load(this, null);
                while (!stopServer)
                {
                    //TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    //var ns = tcpClient.GetStream();
                    ////var sReader = new StreamReader(ns, Encoding.UTF8);
                    //// var sWriter = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };
                    //byte[] data = Encoding.UTF8.GetBytes();

                    //string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
                    //UpdateHistorySafeCall("Connected to client: " + clientEndPoint);
                    //string menu = ReadMenuFromFile();
                    //await sWriter.WriteAsync("" +menu);
                    //NetworkStream stream = new NetworkStream();
                    //byte[] buffer = new byte[1024];
                    //TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    //stream = tcpClient.GetStream();
                    //string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
                    //UpdateHistorySafeCall("Connected to client: " + clientEndPoint);
                    //int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    //string menu = "Customer" + ReadMenuFromFile();
                    //UpdateHistorySafeCall(menu);
                    //byte[] menuBytes = Encoding.UTF8.GetBytes(menu);
                    //await stream.WriteAsync(menuBytes, 0, menuBytes.Length);
                    //stream.Close();
                    //tcpClient.Close();
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    _ = HandlerGetClient(tcpClient);
                }
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show(sockEx.Message);
            }
        }
        private async Task HandlerGetClient(TcpClient tcpClient)
        {
            using (tcpClient)
            {
                NetworkStream stream = new NetworkStream(tcpClient.Client);
                string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
                UpdateHistorySafeCall("Connected to client: " + clientEndPoint);
                string menu = "Customer" + ReadMenuFromFile();
                UpdateHistorySafeCall(menu);
                byte[] menuBytes = Encoding.UTF8.GetBytes(menu);
                await stream.WriteAsync(menuBytes, 0, menuBytes.Length);
                stream.Close();
            }
        }
        private void listenButton_Click(object sender, EventArgs e)
        {
            if (stopServer)
            {
                stopServer = false;
                listenButton.Text = "Stop";
                listenThread = new Thread(async () => await Listen());
                listenThread.Start();
            }
            else
            {
                stopServer = true;
                listenButton.Text = "Listen";
                tcpListener.Stop();
                listenThread.Abort();
                statusLabel.Text = @"Status: Server is stopped";
            }
        }
    }
}
