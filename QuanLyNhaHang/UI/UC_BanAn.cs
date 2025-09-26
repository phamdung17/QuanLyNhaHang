using QuanLyNhaHang.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class UC_BanAn : UserControl
    {
        private BanAn ban;
        public event EventHandler<BanAn> OnChonBan;
      

        public UC_BanAn(BanAn b)
        {
            InitializeComponent();
            ban = b;
            LoadData();
        }

        private void LoadData()
        {
            lblTenBan.Text = ban.TenBan;
            lblTrangThai.Text = ban.TrangThai;

            if (ban.TrangThai == "Trống")
                this.BackColor = Color.LightGreen;
            else if (ban.TrangThai == "Đang dùng")
                this.BackColor = Color.Orange;
            else if (ban.TrangThai == "Chờ duyệt")
                this.BackColor = Color.Yellow;
            else
                this.BackColor = Color.LightGray;
        }
        private void btnChonBan_Click(object sender, EventArgs e)
        {
            if (ban.TrangThai != "Trống")
            {
                MessageBox.Show("Bàn này không khả dụng!", "Thông báo");
                return;
            }

            OnChonBan?.Invoke(this, ban);
        }


        private void UC_BanAn_Click(object sender, EventArgs e)
        {
            OnChonBan?.Invoke(this, ban);
        }
    }
}