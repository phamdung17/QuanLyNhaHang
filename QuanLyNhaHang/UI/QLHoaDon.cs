using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class QLHoaDon : Form
    {
        private int selectedHoaDonId = 0;

        public QLHoaDon()
        {
            InitializeComponent();
        }

        private void QLHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            SetupTimer();
        }

        // Load danh sách hóa đơn
        private void LoadHoaDon()
        {
            try
            {
                dgvHoaDon.DataSource = null;
                dgvHoaDon.DataSource = HoaDonBLL.GetAllHoaDon();

                if (dgvHoaDon.Columns.Count > 0)
                {
                    dgvHoaDon.Columns["HoaDonID"].HeaderText = "Mã HĐ";
                    dgvHoaDon.Columns["TenBan"].HeaderText = "Bàn";
                    dgvHoaDon.Columns["TenKhach"].HeaderText = "Khách hàng";
                    dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
                    dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
                    dgvHoaDon.Columns["TrangThai"].HeaderText = "Trạng thái";
                }
                selectedHoaDonId = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hóa đơn: " + ex.Message);
            }
        }

        // Khi chọn hóa đơn
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];
                    selectedHoaDonId = Convert.ToInt32(row.Cells["HoaDonID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn hóa đơn: " + ex.Message);
                }
            }
        }

        // In và thanh toán
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (selectedHoaDonId == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            try
            {
                var hoaDon = HoaDonBLL.GetById(selectedHoaDonId);
                if (hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!");
                    return;
                }

                // Nếu hóa đơn chưa thanh toán thì cho phép thanh toán và in
                if (hoaDon.TrangThai == "Chưa thanh toán")
                {
                    var chiTiet = HoaDonBLL.GetChiTietHoaDon(selectedHoaDonId);

                    // Thanh toán
                    HoaDonBLL.ThanhToan(selectedHoaDonId);

                    MessageBox.Show("In và thanh toán thành công!");
                }
                else if (hoaDon.TrangThai == "Đã thanh toán")
                {
                    // Nếu đã thanh toán thì chỉ cho in lại
                    MessageBox.Show("Hóa đơn đã thanh toán trước đó! Đang in lại...");
                }

                // load lại danh sách hóa đơn
                LoadHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in hóa đơn: " + ex.Message);
            }
        }
        // Xem chi tiết hóa đơn bằng MessageBox
        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (selectedHoaDonId == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            try
            {
                var hoaDon = HoaDonBLL.GetById(selectedHoaDonId);
                var chiTiet = HoaDonBLL.GetChiTietHoaDon(selectedHoaDonId);

                if (hoaDon == null || chiTiet == null || chiTiet.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy chi tiết hóa đơn!");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("📄 HÓA ĐƠN THANH TOÁN");
                sb.AppendLine("--------------------------------------");
                sb.AppendLine($"Mã hóa đơn: {hoaDon.HoaDonID}");
                sb.AppendLine($"{hoaDon.BanAn?.TenBan}");
                sb.AppendLine($"Khách hàng: {hoaDon.NguoiDung?.HoTen}");
                sb.AppendLine($"Ngày lập: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}");
                sb.AppendLine("--------------------------------------");
                sb.AppendLine("Tên món\tSL \tĐơn giá       \tThành tiền");

                decimal tongTien = 0;
                foreach (dynamic item in chiTiet)
                {
                    sb.AppendLine($"{item.TenMon}\t{item.SoLuong}\t{item.DonGia:N0} đ\t{item.ThanhTien:N0} đ");
                    tongTien += item.ThanhTien;
                }

                sb.AppendLine("--------------------------------------");
                sb.AppendLine($"TỔNG CỘNG: {tongTien:N0} đ");
                sb.AppendLine($"Trạng thái: {hoaDon.TrangThai}");

                MessageBox.Show(sb.ToString(), "Chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem chi tiết: " + ex.Message);
            }
        }

        // Xóa hóa đơn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedHoaDonId == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    HoaDonBLL.Delete(selectedHoaDonId);
                    MessageBox.Show("Xóa thành công!");
                    LoadHoaDon();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message);
                }
            }
        }

        // Xuất PDF
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (selectedHoaDonId == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{selectedHoaDonId}.pdf"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string result = HoaDonBLL.ExportHoaDonToPdf(selectedHoaDonId, sfd.FileName);
                    MessageBox.Show(result);

                    if (result.Contains("thành công"))
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message);
            }
        }
        // cập nhật trạng thái đã thanh toán
        private void btnXacNhanThanhToan_Click(object sender, EventArgs e)
        {
            if (selectedHoaDonId == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            if (HoaDonBLL.XacNhanThanhToan(selectedHoaDonId))
            {
                MessageBox.Show("Xác nhận thanh toán thành công!");
                LoadHoaDon(); // refresh lại danh sách
            }
            else
            {
                MessageBox.Show("Không thể xác nhận. Hóa đơn đã thanh toán hoặc không tồn tại!");
            }
        }
        private void QLHoaDon_Load_1(object sender, EventArgs e)
        {
            // load danh sách hóa đơn
            LoadHoaDon();
        }
        private void SetupTimer()
        {
            // timer1 là tên mặc định của control Timer bạn kéo vào
            timerRefresh.Interval = 10000; // 30000 mili-giây = 30 giây
            timerRefresh.Tick += new EventHandler(timerRefresh_Tick);
            timerRefresh.Start();
        }
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Gọi lại các hàm tải dữ liệu của bạn ở đây
            LoadHoaDon();

        }
        private void LichSuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng timer khi form đóng lại
            timerRefresh.Stop();
        }
    }
}
