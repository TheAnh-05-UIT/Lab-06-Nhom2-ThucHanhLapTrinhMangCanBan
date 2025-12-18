using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClient
{
    public partial class frm_ThuNgan : Form
    {
        private System.Net.Sockets.TcpClient tcp;
        private StreamReader reader;
        private StreamWriter writer;

        private List<OrderItem> currentBillDetails = new List<OrderItem>();

        public class OrderItem
        {
            public int SoBan { get; set; }
            public string TenMon { get; set; }
            public int SoLuong { get; set; }
            public double ThanhTien { get; set; }
        }

        public frm_ThuNgan()
        {
            InitializeComponent();
            frm_ThuNgan_Load(this, null);
        }

        private async void frm_ThuNgan_Load(object sender, EventArgs e)
        {
            await ConnectServer("127.0.0.1", 8080);
            await RefreshOrders();
        }

        private async Task ConnectServer(string host, int port)
        {
            try
            {
                tcp = new System.Net.Sockets.TcpClient();
                await tcp.ConnectAsync(host, port);
                var stream = tcp.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private async Task RefreshOrders()
        {
            if (writer == null) return;

            await writer.WriteLineAsync("GET_ORDERS");
            var all = new List<OrderItem>();
            while (true)
            {
                string line = await reader.ReadLineAsync();
                if (line == null) break;
                line = line.Trim();
                if (line.Equals("END", StringComparison.OrdinalIgnoreCase)) break;
                if (line.Length == 0) continue;

                var parts = line.Split(';');
                if (parts.Length < 5) continue;

                int soBan = int.TryParse(parts[0], out var b) ? b : 0;
                string ten = parts[2];
                int sl = int.TryParse(parts[3], out var q) ? q : 0;
                var moneyStr = parts[4].Replace(",", "").Replace(".", "");
                double tt = double.TryParse(moneyStr, out var m) ? m : 0;

                all.Add(new OrderItem { SoBan = soBan, TenMon = ten, SoLuong = sl, ThanhTien = tt });
            }

            dgvData.DataSource = all;
        }

        private async void btnCharge_Click(object sender, EventArgs e)
        {
            if (writer == null)
            {
                MessageBox.Show("Chưa kết nối server!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSoban.Text) || !int.TryParse(txtSoban.Text, out int soBan))
                return;

            try
            {
                await writer.WriteLineAsync($"PAY {soBan}");

                string first = await reader.ReadLineAsync();
                if (first == null)
                {
                    MessageBox.Show("Không nhận được phản hồi!");
                    return;
                }
                if (first.StartsWith("FAIL", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Tính tiền thất bại: " + first);
                    return;
                }
                double total = 0;
                if (first.StartsWith("TOTAL2 ", StringComparison.OrdinalIgnoreCase))
                {
                    double.TryParse(first.Substring(7).Trim(), out total);
                }
                else if (first.StartsWith("TOTAL ", StringComparison.OrdinalIgnoreCase))
                {
                    string t2 = await reader.ReadLineAsync();
                    if (t2 != null && t2.StartsWith("TOTAL2 ", StringComparison.OrdinalIgnoreCase))
                        double.TryParse(t2.Substring(7).Trim(), out total);
                }

                var details = new List<OrderItem>();
                while (true)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null) break;
                    line = line.Trim();
                    if (line.Equals("END", StringComparison.OrdinalIgnoreCase)) break;
                    if (line.Length == 0) continue;
                    var parts = line.Split(';');
                    if (parts.Length < 4) continue;

                    int sb = int.TryParse(parts[0], out var b) ? b : soBan;
                    string ten = parts[1];
                    int sl = int.TryParse(parts[2], out var q) ? q : 0;

                    var moneyStr = parts[3].Replace(",", "").Replace(".", "");
                    double tt = double.TryParse(moneyStr, out var m) ? m : 0;

                    details.Add(new OrderItem { SoBan = sb, TenMon = ten, SoLuong = sl, ThanhTien = tt });
                }

                lblTongTien.Text = total.ToString("N0") + " VNĐ";
                dgvData.DataSource = details;
                currentBillDetails = details;
                await RefreshOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tiền: " + ex.Message);
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (currentBillDetails == null || currentBillDetails.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất hóa đơn!");
                return;
            }

            string soBan = txtSoban.Text;
            string fileName = $"bill_Ban{soBan}.txt";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine("      HÓA ĐƠN THANH TOÁN      ");
                    sw.WriteLine($"Bàn số: {soBan}");
                    sw.WriteLine($"Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    sw.WriteLine("--------------------------------");
                    sw.WriteLine("{0,-20} {1,-5} {2,-10}", "Tên món", "SL", "T.Tiền");

                    foreach (var item in currentBillDetails)
                        sw.WriteLine("{0,-20} {1,-5} {2,-10:N0}", item.TenMon, item.SoLuong, item.ThanhTien);

                    sw.WriteLine("--------------------------------");
                    sw.WriteLine($"TỔNG CỘNG: {lblTongTien.Text}");
                    sw.WriteLine("Cảm ơn quý khách!");
                }
                MessageBox.Show($"Đã xuất hóa đơn tại: {path}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message);
            }
        }
    }
}
