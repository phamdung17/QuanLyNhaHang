using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class ClientForm : Form
    {
        private List<ThucDonViewModel> gioHang = new List<ThucDonViewModel>();
        private NguoiDung currentUser;

        public ClientForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            LoadThucDon();
            // cập nhật UI nếu có label hiển thị user
            if (this.Controls.ContainsKey("lblWelcome"))
                (this.Controls["lblWelcome"] as Label).Text = $"Xin chào {currentUser?.HoTen}";
        }

        private void LoadThucDon()
        {
            flowThucDon.Controls.Clear();
            var menu = ThucDonBLL.GetMenu();
            foreach (var item in menu)
            {
                var uc = new UC_MonAn(item);
                uc.OnAddToCart += Uc_OnAddToCart;
                flowThucDon.Controls.Add(uc);
            }
        }

        private void Uc_OnAddToCart(object sender, ThucDonViewModel mon)
        {
            gioHang.Add(mon);
            MessageBox.Show($"Đã thêm {mon.TenMon} vào giỏ hàng", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // cập nhật label giỏ hàng nếu có
            if (this.Controls.ContainsKey("lblGioHang"))
                (this.Controls["lblGioHang"] as Label).Text = $"Giỏ: {gioHang.Count} món";
        }

        public List<ThucDonViewModel> GetGioHang() => gioHang;
    }
}
