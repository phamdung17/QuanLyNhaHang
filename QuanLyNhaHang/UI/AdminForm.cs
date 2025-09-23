using QuanLyNhaHang.Models;  // cần để dùng class NguoiDung
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class AdminForm : Form
    {
        private NguoiDung currentUser;  // ✅ biến lưu user đang đăng nhập

        // Constructor nhận user
        public AdminForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user; // gán user truyền vào biến thành viên
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // Ví dụ: hiển thị tên Admin ở tiêu đề
            this.Text = $"Admin Panel - Xin chào {currentUser.HoTen}";
        }
    }
}
