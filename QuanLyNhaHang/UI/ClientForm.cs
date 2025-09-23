using QuanLyNhaHang.Models;  
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class ClientForm : Form
    {
        private NguoiDung currentUser;  // ✅ user hiện tại

        // Constructor nhận user
        public ClientForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user; // gán user truyền vào biến thành viên

        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Ví dụ: hiển thị tên client ở tiêu đề
            this.Text = $"Client Panel - Xin chào {currentUser.HoTen}";


        }
    }
}
