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
using System.Text.Json;
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
        private List<OrderItem> allOrders = new List<OrderItem>();

        public class OrderItem
        {
            public int SoBan { get; set; }
            public string TenMon { get; set; }
            public int SoLuong { get; set; }
            public double ThanhTien { get; set; }
        }
        private void Log(string msg)
        {
            if (InvokeRequired) { Invoke(new Action(() => Log(msg))); return; }
            historyTextBox.AppendText($"[{DateTime.Now:T}] {msg}\r\n");
            historyTextBox.SelectionStart = historyTextBox.Text.Length;
            historyTextBox.ScrollToCaret();
        }

        public string ReadMenuFromFile()
        {
            string filePath = "menu.txt";
            if (!File.Exists(filePath))
            {
                Log("File menu.txt không tồn tại.");
                return string.Empty;
            }
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Log("Lỗi đọc file menu.txt: " + ex.Message);
                return string.Empty;
            }
        }
        private void Server_Load(object sender, EventArgs e)
        {
            string menu = ReadMenuFromFile();
            Log("Menu load xong:\n" + menu);
        }
        private async Task HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            string menu = ReadMenuFromFile();
            await writer.WriteLineAsync(JsonSerializer.Serialize(new { Type = "Menu", Data = menu }));

            try
            {
                while (client.Connected)
                {
                    string line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line)) break;

                    Log("Nhận lệnh: " + line);
                    var request = JsonSerializer.Deserialize<Dictionary<string, object>>(line.ToString());
                    string action = request["action"].ToString().ToUpper();

                    if (action == "ORDER")
                    {
                 
                        var data = JsonSerializer.Deserialize<OrderItem>(request["data"].ToString());
                        allOrders.Add(data);
                        Log($"Bàn {data.SoBan} đặt {data.TenMon} x{data.SoLuong}");
                        await writer.WriteLineAsync(JsonSerializer.Serialize(new { Status = "OK" }));
                    }
                    else if (action == "PAY")
                    {
                        
                        int soBan = int.Parse(request["data"].ToString());
                        var billDetails = allOrders.Where(o => o.SoBan == soBan).ToList();
                        double total = billDetails.Sum(o => o.ThanhTien);

                        var response = new { Status = "Paid", Total = total, Details = billDetails };
                        await writer.WriteLineAsync(JsonSerializer.Serialize(response));

                        allOrders.RemoveAll(o => o.SoBan == soBan);
//=======
//                    Log("Client: " + line);
//                    if (line.StartsWith("Order"))
//                    {
//                        Log("Nhận Order: " + line);
//                        string detail = await reader.ReadLineAsync();
//                        if (detail != null)
//                        {
//                            Log("Chi tiết: " + detail);
//                            var parts = detail.Split(' ');
//                            if (parts.Length == 2)
//                            {
//                                string id = parts[0];
//                                string qty = parts[1];
//                                Log($"Món {id}, SL {qty}");
//                            }
//                        }
//>>>>>>> 361338c498fc7ed24027f088fa54fbb8006370d1
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Lỗi client: " + ex.Message);
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (stopServer)
            {
                stopServer = false;
                listenButton.Text = "Stop";
                listenThread = new Thread(async () =>
                {
                    tcpListener = new TcpListener(IPAddress.Any, _serverPort);
                    tcpListener.Start();
                    Log($"Server started on port {_serverPort}");
                    while (!stopServer)
                    {
                        try
                        {
                            var client = await tcpListener.AcceptTcpClientAsync();
                            Log("Client connected: " + client.Client.RemoteEndPoint.ToString());
                            _ = HandleClient(client);
                        }
                        catch (Exception ex)
                        {
                            Log("Lỗi server: " + ex.Message);
                        }
                    }
                    tcpListener.Stop();
                    Log("Server stopped.");
                });
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            else
            {
                stopServer = true;
                listenButton.Text = "Listen";
            }
        }
    }
}
