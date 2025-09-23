using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class RegisterForm : Form
    {
        private NguoiDungBLL bll = new NguoiDungBLL();

        public RegisterForm()
        {
            InitializeComponent();
        }

        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirmPassword.Text.Trim();
            string fullname = txtFullName.Text.Trim();
            

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            string result = bll.Register(username, password, fullname, "Client");
            MessageBox.Show(result);

            if (result == "Đăng ký thành công!")
            {
                this.Close(); // quay lại form login
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
