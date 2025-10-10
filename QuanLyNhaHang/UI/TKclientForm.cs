using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class TKclientForm : Form
    {
        private readonly NguoiDung currentUser;
        public TKclientForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void TKClientForm_Load(object sender, EventArgs e)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Lỗi: Không có thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Tải thông tin lên Tab 1
            txtTenDangNhap.Text = currentUser.TenDangNhap;
            txtHoTen.Text = currentUser.HoTen;
            txtTenDangNhap.ReadOnly = true; // ✨ Không cho sửa tên đăng nhập

            // Tải thông tin lên Tab 2 (giả sử tên textbox là txtTenDangNhapTab2)
            txtTenDangNhapTab2.Text = currentUser.TenDangNhap;
            txtTenDangNhapTab2.ReadOnly = true; // ✨ Không cho sửa tên đăng nhập
        }

        // --- SỰ KIỆN CHO TAB 1: THÔNG TIN TÀI KHOẢN ---
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            string newHoTen = txtHoTen.Text.Trim();

            string result = NguoiDungBLL.CapNhatThongTin(currentUser.UserID, newHoTen);

            if (result.Contains("thành công"))
            {
                currentUser.HoTen = newHoTen; // Cập nhật lại họ tên trên đối tượng hiện tại
                MessageBox.Show(result, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- SỰ KIỆN CHO TAB 2: ĐỔI MẬT KHẨU ---
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string oldPass = txtMatKhauCu.Text;
            string newPass = txtMatKhauMoi.Text;
            string confirmPass = txtXacNhanMatKhau.Text;

            string result = NguoiDungBLL.DoiMatKhau(currentUser.UserID, oldPass, newPass, confirmPass);

            if (result.Contains("thành công"))
            {
                MessageBox.Show(result, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMatKhauCu.Clear();
                txtMatKhauMoi.Clear();
                txtXacNhanMatKhau.Clear();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}