using System;
using System.Windows.Forms;
using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.UI
{
    public partial class AdminForm : Form
    {
        private readonly NguoiDung currentUser;
        public AdminForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

            if (this.Controls.ContainsKey("lblWelcome"))
                (this.Controls["lblWelcome"] as Label).Text = $"Xin chào {currentUser?.HoTen}";
        }
        private void OpenChildForm(Form childForm)
        {
            // Xóa control cũ trong panel
            panelMain.Controls.Clear();

            // Cấu hình form con để hiển thị trong panel
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Thêm vào panel
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;

            childForm.Show();
        }


        private void btnQLBan_Click(object sender, EventArgs e)
    => OpenChildForm(new QLBanAnForm());

        private void btnQLThucDon_Click(object sender, EventArgs e)
            => OpenChildForm(new QLThucDonForm());

        private void btnDuyetBanAn_Click(object sender, EventArgs e)
            => OpenChildForm(new DuyetBanAnForm());

        private void btnHoaDon_Click(object sender, EventArgs e)
            => OpenChildForm(new QLHoaDon());

        private void btnBaoCaoDT_Click(object sender, EventArgs e)
            => OpenChildForm(new BaoCaoDTForm());

        private void btnTKadmin_Click(object sender, EventArgs e)
            => OpenChildForm(new TKadminForm());

        // Riêng nút đăng xuất thì mở form Login mới
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.Closed += (s, args) => this.Close(); // Đóng ClientForm khi LoginForm đóng
            loginForm.Show();
        }

    }
}