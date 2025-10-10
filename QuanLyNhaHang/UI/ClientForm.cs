using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyNhaHang
{
    public partial class ClientForm : Form
    {
       
        private readonly NguoiDung currentUser;
      
        public ClientForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;

        }

        private void ClientForm_Load(object sender, EventArgs e)
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
        private void btnThucDon_Click(object sender, EventArgs e)
        {
           OpenChildForm(new ThucDonOder(currentUser));
        }

        private void btnDatBan_Click(object sender, EventArgs e)
            => OpenChildForm(new BanAnForm(currentUser));

        private void btnLichSu_Click(object sender, EventArgs e)
            => OpenChildForm(new LichSuForm(currentUser));

        private void btnTkClient_Click(object sender, EventArgs e)
            => OpenChildForm(new TKclientForm(currentUser));

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.Closed += (s, args) => this.Close(); // Đóng ClientForm khi LoginForm đóng
            loginForm.Show();
        }
            
    }
}