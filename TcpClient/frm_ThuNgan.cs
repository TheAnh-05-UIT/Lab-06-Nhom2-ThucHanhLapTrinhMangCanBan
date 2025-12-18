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

        public class PayResponse
        {
            public string Status { get; set; }
            public double Total { get; set; }
            public List<OrderItem> Details { get; set; }
        }
        public frm_ThuNgan()
        {
            InitializeComponent();
        }

        private async void frm_ThuNgan_Load(object sender, EventArgs e)
        {
            await ConnectServer("127.0.0.1", 8080);
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
        private async void btnCharge_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoban.Text)) return;

            try
            {
                var request = new { action = "pay", data = int.Parse(txtSoban.Text) };
                await writer.WriteLineAsync(JsonSerializer.Serialize(request));
                string response = await reader.ReadLineAsync();
                if (!string.IsNullOrEmpty(response))
                {
                    var result = JsonSerializer.Deserialize<PayResponse>(response);
                    lblTongTien.Text = result.Total.ToString("N0") + " VNĐ";
                   // dgvData.DataSource = result.Details;

                    currentBillDetails = result.Details;
                }
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
                    sw.WriteLine("{0,-15} {1,-5} {2,-10}", "Tên món", "SL", "T.Tiền");

                    foreach (var item in currentBillDetails)
                    {
                        sw.WriteLine("{0,-15} {1,-5} {2,-10:N0}", item.TenMon, item.SoLuong, item.ThanhTien);
                    }

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
