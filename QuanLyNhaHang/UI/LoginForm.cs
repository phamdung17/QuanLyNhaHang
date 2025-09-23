using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class LoginForm : Form
    {
        private NguoiDungBLL bll = new NguoiDungBLL();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đủ tên đăng nhập và mật khẩu!");
                return;
            }

            NguoiDung user = bll.Login(username, password);

            if (user != null)
            {
                MessageBox.Show($"Đăng nhập thành công! Xin chào {user.HoTen} ({user.VaiTro})");

                if (user.VaiTro == "Admin")   // ✅ check 
                {
                    AdminForm admin = new AdminForm(user);
                    admin.Show();
                }
                else if (user.VaiTro == "Client")   // ✅ check client
                {
                    ClientForm client = new ClientForm(user);
                    client.Show();
                }

                this.Hide(); // ẩn form login
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Registerlog_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
