using System;
using System.Collections.Generic;
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
        public Server() { InitializeComponent(); }

        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopServer = true;
        private readonly int _serverPort = 8080;
        private readonly Dictionary<string, (string Name, string PriceText, double PriceValue)> _menu =
    new Dictionary<string, (string, string, double)>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, Dictionary<string, int>> _orders =
            new Dictionary<int, Dictionary<string, int>>();

        private readonly object _lock = new object();

        private void Log(string msg)
        {
            if (InvokeRequired) { Invoke(new Action(() => Log(msg))); return; }
            historyTextBox.AppendText($"[{DateTime.Now:T}] {msg}\r\n");
            historyTextBox.SelectionStart = historyTextBox.Text.Length;
            historyTextBox.ScrollToCaret();
        }

        private void LoadMenu()
        {
            _menu.Clear();
            string filePath = "menu.txt";

            if (!File.Exists(filePath))
            {
                Log("File menu.txt không tồn tại.");
                return;
            }
            foreach (var raw in File.ReadAllLines(filePath))
            {
                var line = raw.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                if (parts.Length < 3) continue;

                var id = parts[0].Trim();
                var name = parts[1].Trim();
                var priceText = parts[2].Trim();
                var digits = new string(priceText.Where(char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(digits)) continue;

                if (!double.TryParse(digits, out double priceValue)) continue;

                _menu[id] = (name, priceText, priceValue);
            }

            Log($"Menu load xong: {_menu.Count} món.");
        }


        private void Server_Load(object sender, EventArgs e)
        {
            LoadMenu();
        }

        private async Task HandleClient(TcpClient client)
        {
            var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
            Log($"Client connected: {endpoint}");

            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            try
            {
                while (client.Connected)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null) break;

                    line = line.Trim();
                    if (line.Length == 0) continue;

                    Log($"[{endpoint}] -> {line}");

                    if (line.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                    {
                        await writer.WriteLineAsync("OK");
                        break;
                    }

                    if (line.Equals("MENU", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var kv in _menu.OrderBy(k => k.Key))
                        {
                            await writer.WriteLineAsync($"{kv.Key};{kv.Value.Name};{kv.Value.PriceText}");
                        }
                        await writer.WriteLineAsync("END");
                        continue;
                    }

                    if (line.Equals("GET_ORDERS", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> rows = new List<string>();
                        lock (_lock)
                        {
                            foreach (var banKv in _orders.OrderBy(x => x.Key))
                            {
                                int soBan = banKv.Key;
                                foreach (var itemKv in banKv.Value)
                                {
                                    string id = itemKv.Key;
                                    int qty = itemKv.Value;

                                    if (!_menu.TryGetValue(id, out var m)) continue;
                                    double thanhTien = m.PriceValue * qty;



                                    rows.Add($"{soBan};{id};{m.Name};{qty};{thanhTien:0}");
                                }
                            }
                        }

                        foreach (var r in rows) await writer.WriteLineAsync(r);
                        await writer.WriteLineAsync("END");
                        continue;
                    }

                    if (line.StartsWith("ORDER ", StringComparison.OrdinalIgnoreCase))
                    {
                        var tail = line.Substring(6).Trim();
                        if (!int.TryParse(tail, out int soBan) || soBan <= 0)
                        {
                            await writer.WriteLineAsync("FAIL InvalidTable");
                            continue;
                        }
                        Dictionary<string, int> add = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                        while (true)
                        {
                            string d = await reader.ReadLineAsync();
                            if (d == null) { await writer.WriteLineAsync("FAIL Disconnected"); return; }
                            d = d.Trim();
                            if (d.Equals("END", StringComparison.OrdinalIgnoreCase)) break;
                            if (d.Length == 0) continue;

                            var parts = d.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length != 2) continue;

                            string id = parts[0].Trim();
                            if (!int.TryParse(parts[1], out int qty) || qty <= 0) continue;
                            if (!_menu.ContainsKey(id)) continue;

                            add[id] = add.TryGetValue(id, out int old) ? old + qty : qty;
                        }

                        if (add.Count == 0)
                        {
                            await writer.WriteLineAsync("FAIL EmptyOrder");
                            continue;
                        }

                        lock (_lock)
                        {
                            if (!_orders.TryGetValue(soBan, out var cur))
                            {
                                cur = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                                _orders[soBan] = cur;
                            }

                            foreach (var kv in add)
                                cur[kv.Key] = cur.TryGetValue(kv.Key, out int old) ? old + kv.Value : kv.Value;
                        }

                        await writer.WriteLineAsync("OK");
                        continue;
                    }

                    if (line.StartsWith("PAY ", StringComparison.OrdinalIgnoreCase))
                    {
                        var tail = line.Substring(4).Trim();
                        if (!int.TryParse(tail, out int soBan) || soBan <= 0)
                        {
                            await writer.WriteLineAsync("FAIL InvalidTable");
                            continue;
                        }

                        Dictionary<string, int> bill;
                        lock (_lock)
                        {
                            if (!_orders.TryGetValue(soBan, out bill))
                            {
                                bill = null;
                            }
                            else
                            {
                                bill = new Dictionary<string, int>(bill, StringComparer.OrdinalIgnoreCase);
                                _orders.Remove(soBan);
                            }
                        }

                        if (bill == null || bill.Count == 0)
                        {
                            await writer.WriteLineAsync("FAIL NoOrder");
                            continue;
                        }

                        double total = 0;
                        await writer.WriteLineAsync($"TOTAL {total:0}");

                        List<string> detailLines = new List<string>();
                        foreach (var kv in bill)
                        {
                            if (!_menu.TryGetValue(kv.Key, out var m)) continue;
                            int qty = kv.Value;
                            double thanhTien = m.PriceValue * qty;

                            total += thanhTien;
                            detailLines.Add($"{soBan};{m.Name};{qty};{thanhTien:0}");
                        }
                        await writer.WriteLineAsync($"TOTAL2 {total:0}");
                        foreach (var dl in detailLines) await writer.WriteLineAsync(dl);
                        await writer.WriteLineAsync("END");
                        continue;
                    }

                    await writer.WriteLineAsync("FAIL UnknownCommand");
                }
            }
            catch (Exception ex)
            {
                Log($"Lỗi client {endpoint}: {ex.Message}");
            }
            finally
            {
                try { client.Close(); } catch { }
                Log($"Client disconnected: {endpoint}");
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (stopServer)
            {
                LoadMenu();
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
