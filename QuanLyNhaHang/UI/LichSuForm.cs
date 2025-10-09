using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.UI
{
    public partial class LichSuForm : Form
    {
        private readonly NguoiDung currentUser;
        private int selectedDatBanId = 0;
        private string selectedTrangThai = "";

        public LichSuForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void LichSuForm_Load(object sender, EventArgs e)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Không có thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadLichSuDatBan();
            LoadLichSuDonHang();
            SetupTimer(); // Thiết lập timer để tự động làm mới dữ liệu
        }

        // ... (Các hàm LoadLichSuDatBan, LoadLichSuDonHang, dgvLichSu_CellClick, btnHuy_Click giữ nguyên như trước)

        #region Tải Dữ Liệu

        private void LoadLichSuDatBan()
        {
            try
            {
                dgvLichSu.DataSource = DatBanBLL.GetLichSuDatBanForDisplay(currentUser.UserID);
                if (dgvLichSu.Columns.Count > 0)
                {
                    dgvLichSu.Columns["DatBanID"].HeaderText = "Mã Đặt Bàn";
                    dgvLichSu.Columns["TenBan"].HeaderText = "Tên Bàn";
                    dgvLichSu.Columns["NgayDat"].HeaderText = "Ngày Đặt";
                    dgvLichSu.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
                selectedDatBanId = 0;
                selectedTrangThai = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLichSuDonHang()
        {
            try
            {
                dgvLichSuDonHang.DataSource = HoaDonBLL.GetLichSuHoaDonByUser(currentUser.UserID);
                if (dgvLichSuDonHang.Columns.Count > 0)
                {
                    dgvLichSuDonHang.Columns["HoaDonID"].HeaderText = "Mã Hóa Đơn";
                    dgvLichSuDonHang.Columns["TenBan"].HeaderText = "Tên Bàn";
                    dgvLichSuDonHang.Columns["NgayLap"].HeaderText = "Ngày Lập";
                    dgvLichSuDonHang.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dgvLichSuDonHang.Columns["TrangThai"].HeaderText = "Trạng Thái";
                    dgvLichSuDonHang.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Sự kiện Click

        private void dgvLichSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedDatBanId = Convert.ToInt32(dgvLichSu.Rows[e.RowIndex].Cells["DatBanID"].Value);
                selectedTrangThai = dgvLichSu.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            }
        }

        // ✨ SỰ KIỆN MỚI: Click vào một dòng trong Lịch sử Đơn hàng
        private void dgvLichSuDonHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Lấy ID của hóa đơn từ dòng được chọn
                    int selectedHoaDonId = Convert.ToInt32(dgvLichSuDonHang.Rows[e.RowIndex].Cells["HoaDonID"].Value);

                    // Gọi hàm hiển thị chi tiết
                    ShowChiTietHoaDon(selectedHoaDonId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (selectedDatBanId == 0)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu đặt bàn để hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✨ THAY ĐỔI LOGIC KIỂM TRA TẠI ĐÂY ✨
            // Kiểm tra trạng thái ngay trên giao diện
            if (selectedTrangThai != "Chờ duyệt")
            {
                MessageBox.Show($"Không thể hủy vì yêu cầu đã được admin xử lý (Trạng thái: {selectedTrangThai}).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn hủy yêu cầu đặt bàn này không?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string result = DatBanBLL.ClientHuyDatBan(selectedDatBanId, currentUser.UserID);
                MessageBox.Show(result, "Kết quả", MessageBoxButtons.OK, result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                LoadLichSuDatBan();
                LoadLichSuDonHang();
            }
        }
        private void btnXemHoaDon_Click(object sender, EventArgs e)
        {
            var hoaDon = HoaDonBLL.GetHoaDonHienTai(currentUser.UserID);
            if (hoaDon == null)
            {
                MessageBox.Show("Bạn hiện không có hóa đơn nào chưa thanh toán. Hãy bắt đầu gọi món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ShowChiTietHoaDon(hoaDon.HoaDonID);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadLichSuDatBan();
            LoadLichSuDonHang();
        }

        #endregion

        #region Hàm Hỗ Trợ

        // ✨ HÀM MỚI: Tái sử dụng code để hiển thị chi tiết hóa đơn
        private void ShowChiTietHoaDon(int hoaDonId)
        {
            try
            {
                var hoaDon = HoaDonBLL.GetById(hoaDonId);
                var chiTiet = HoaDonBLL.GetChiTietHoaDon(hoaDonId);

                if (hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("📄 CHI TIẾT HÓA ĐƠN");
                sb.AppendLine("--------------------------------------");
                sb.AppendLine($"Mã hóa đơn: {hoaDon.HoaDonID}");
                sb.AppendLine($"Bàn: {hoaDon.BanAn?.TenBan}");
                sb.AppendLine($"Khách hàng: {hoaDon.NguoiDung?.HoTen}"); // Thêm tên khách hàng
                sb.AppendLine($"Ngày lập: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}");
                sb.AppendLine("--------------------------------------");
                sb.AppendLine("Tên món\t\tSL \tĐơn giá \tThành tiền");

                decimal tongTien = 0;
                foreach (dynamic item in chiTiet)
                {
                    sb.AppendLine($"{item.TenMon}\t\t{item.SoLuong}\t{item.DonGia:N0} đ\t{item.ThanhTien:N0} đ");
                    tongTien += item.ThanhTien;
                }

                sb.AppendLine("--------------------------------------");
                sb.AppendLine($"TỔNG CỘNG: {tongTien:N0} đ");
                sb.AppendLine($"Trạng thái: {hoaDon.TrangThai}");

                MessageBox.Show(sb.ToString(), $"Chi tiết hóa đơn #{hoaDon.HoaDonID}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            LoadLichSuDatBan();
            LoadLichSuDonHang();
        }
        private void LichSuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng timer khi form đóng lại
            timerRefresh.Stop();
        }
        #endregion
    }
}