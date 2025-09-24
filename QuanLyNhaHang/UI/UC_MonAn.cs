using QuanLyNhaHang.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class UC_MonAn : UserControl
    {
        private ThucDon mon;

        public event EventHandler<ThucDon> OnAddToCart;

        public UC_MonAn(ThucDon monAn)
        {
            InitializeComponent();
            mon = monAn;
            LoadData();
        }

        private void LoadData()
        {
            lblTen.Text = mon.TenMon;
            lblGia.Text = mon.DonGia.ToString("N0") + " đ";

            if (!string.IsNullOrEmpty(mon.HinhAnh) && File.Exists(mon.HinhAnh))
                picMon.Image = Image.FromFile(mon.HinhAnh);
            else
                picMon.Image = Properties.Resources.no_image; // ảnh mặc định
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            OnAddToCart?.Invoke(this, mon);
        }
    }
}
