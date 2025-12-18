using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace TcpClient
{
    public partial class frm_KhachHang : Form
    {
        public frm_KhachHang()
        {
            InitializeComponent();
            dgv_Thucdon.ReadOnly = false;
        }
        private System.Net.Sockets.TcpClient tcp;
        private StreamReader reader;
        private StreamWriter writer;
        private CancellationTokenSource clientCts;
        private async Task ConnectServer(string host, int port)
        {
            try
            {
                tcp = new System.Net.Sockets.TcpClient();
                await tcp.ConnectAsync(host, port);
                reader = new StreamReader(tcp.GetStream(), Encoding.UTF8);
                writer = new StreamWriter(tcp.GetStream(), Encoding.UTF8) { AutoFlush = true };
                clientCts = new CancellationTokenSource();
                _ = Task.Run(() => ListenFromServer(clientCts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }
        private async Task ListenFromServer(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        var message = JsonSerializer.Deserialize<Dictionary<string, string>>(line);
                        if (message != null && message.ContainsKey("Type") && message["Type"] == "Menu")
                        {
                            string menuData = message["Data"];
                            var menuItems = menuData.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(item =>
                                {
                                    var parts = item.Split(';');
                                    return new MenuItem
                                    {
                                        ID = parts[0],
                                        Names = parts[1],
                                        Price = parts[2],
                                        Quantity = 0
                                    };
                                }).ToList();
                            if (dgv_Thucdon.InvokeRequired)
                            {
                                dgv_Thucdon.Invoke(new Action(() =>
                                {
                                    dgv_Thucdon.DataSource = menuItems;
                                }));
                            }
                            else
                            {
                                dgv_Thucdon.DataSource = menuItems;
                            }
                            dgv_Thucdon.Columns["Quantity"].ReadOnly = false;
                            dgv_Thucdon.Columns["ID"].ReadOnly = true;
                            dgv_Thucdon.Columns["Names"].ReadOnly = true;
                            dgv_Thucdon.Columns["Price"].ReadOnly = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhận dữ liệu từ server: " + ex.Message);
            }
        }
        private async void btn_PlaceOrder_Click(object sender, EventArgs e)
        {
            dgv_Thucdon.EndEdit();
            dgv_Thucdon.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (writer == null)
            {
                MessageBox.Show("Chưa kết nối server!");
                return;
            }
            int soBan = (int)nud_BanSo.Value;
            var list = dgv_Thucdon.DataSource as List<MenuItem>;
            if (list == null) return;
            var orderItems = list.Where(x => x.Quantity > 0).ToList();
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn món nào!");
                return;
            }
            try
            {
                foreach (var item in orderItems)
                {
                    await writer.WriteLineAsync($"Order {soBan}");
                    await writer.WriteLineAsync($"{item.ID} {item.Quantity}");
                }
                MessageBox.Show("Đặt món thành công!");
                foreach (var item in orderItems)
                    item.Quantity = 0;
                dgv_Thucdon.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi Order: " + ex.Message);
            }
        }

        public class MenuItem
        {
            public string ID { get; set; }
            public string Names { get; set; }
            public string Price { get; set; }
            public int Quantity { get; set; }
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            _ = ConnectServer("127.0.0.1", 8080);
        }

    }
}

