using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public class MenuItem
        {
            public string ID { get; set; }
            public string Names { get; set; }
            public string Price { get; set; }
            public int Quantity { get; set; }
        }

        private async Task ConnectServer(string host, int port)
        {
            try
            {
                tcp = new System.Net.Sockets.TcpClient();
                await tcp.ConnectAsync(host, port);
                reader = new StreamReader(tcp.GetStream(), Encoding.UTF8);
                writer = new StreamWriter(tcp.GetStream(), Encoding.UTF8) { AutoFlush = true };
                clientCts = new CancellationTokenSource();
                await LoadMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private async Task LoadMenu()
        {
            if (writer == null) return;

            await writer.WriteLineAsync("MENU");
            var lines = new List<string>();
            while (true)
            {
                string line = await reader.ReadLineAsync();
                if (line == null) break;
                if (line.Trim().Equals("END", StringComparison.OrdinalIgnoreCase)) break;
                if (string.IsNullOrWhiteSpace(line)) continue;
                lines.Add(line);
            }

            var menuItems = lines.Select(item =>
            {
                var parts = item.Split(';');
                return new MenuItem
                {
                    ID = parts.Length > 0 ? parts[0] : "",
                    Names = parts.Length > 1 ? parts[1] : "",
                    Price = parts.Length > 2 ? parts[2] : "",
                    Quantity = 0
                };
            }).ToList();

            dgv_Thucdon.DataSource = menuItems;

            dgv_Thucdon.Columns["Quantity"].ReadOnly = false;
            dgv_Thucdon.Columns["ID"].ReadOnly = true;
            dgv_Thucdon.Columns["Names"].ReadOnly = true;
            dgv_Thucdon.Columns["Price"].ReadOnly = true;
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
                await writer.WriteLineAsync($"ORDER {soBan}");
                foreach (var item in orderItems)
                    await writer.WriteLineAsync($"{item.ID} {item.Quantity}");
                await writer.WriteLineAsync("END");
                string resp = await reader.ReadLineAsync();
                if (resp != null && resp.StartsWith("OK", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Đặt món thành công!");
                    foreach (var item in orderItems) item.Quantity = 0;
                    dgv_Thucdon.Refresh();
                }
                else
                {
                    MessageBox.Show("Đặt món thất bại: " + (resp ?? "No response"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi Order: " + ex.Message);
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            _ = ConnectServer("127.0.0.1", 8080);
        }
    }
}
