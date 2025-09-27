using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Drawing;
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

        // Sự kiện Load form: Focus vào textbox Username
        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            txtUsername.SelectAll();  // Chọn hết nếu có text cũ
        }

        // Sự kiện Đăng Nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            try
            {
                NguoiDung user = bll.Login(username, password);

                if (user != null)
                {
                    MessageBox.Show($"Đăng nhập thành công! Xin chào {user.HoTen} ({user.VaiTro})",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form tương ứng dựa trên vai trò
                    if (user.VaiTro == "Admin")
                    {
                        AdminForm admin = new AdminForm(user);
                        admin.Show();
                    }
                    else if (user.VaiTro == "Client")
                    {
                        ClientForm client = new ClientForm(user);
                        client.Show();
                    }
                    else
                    {
                        // Vai trò khác (nếu có, ví dụ: Staff - mở rộng sau)
                        MessageBox.Show("Vai trò không được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //this.Hide();  // Ẩn form Login (có thể quay lại nếu cần)
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi Đăng Nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();  // Xóa mật khẩu sai
                    txtPassword.Focus();  // Focus vào mật khẩu để nhập lại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\nVui lòng kiểm tra kết nối database.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Có thể log lỗi: System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        // Sự kiện Thoát
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Sự kiện Đăng Ký (mở RegisterForm)
        private void Registerlog_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.ShowDialog();  // Mở modal, chờ đóng
            // Sau đăng ký, focus lại Username (nếu RegisterForm lưu user mới, có thể reload)
            txtUsername.Focus();
        }

        // Hover effect cho button (tùy chọn, thêm tương tác đẹp)
        private void Registerlog_MouseEnter(object sender, EventArgs e)
        {
            Registerlog.BackColor = Color.FromArgb(0, 150, 255);  // Sáng hơn khi hover
        }

        private void Registerlog_MouseLeave(object sender, EventArgs e)
        {
            Registerlog.BackColor = Color.FromArgb(0, 122, 204);  // Màu gốc
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(76, 175, 80);  // Xanh lá sáng hơn
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(46, 125, 50);  // Màu gốc
        }

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.FromArgb(255, 99, 71);  // Đỏ sáng hơn
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.FromArgb(244, 67, 54);  // Màu gốc
        }
    }
}
