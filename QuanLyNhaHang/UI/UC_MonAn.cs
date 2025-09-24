using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class UC_MonAn : UserControl
    {
        public string TenMon { get; private set; }
        public decimal Gia { get; private set; }
        public string ImgPath { get; private set; }

        public UC_MonAn()
        {
            InitializeComponent();
        }

        public void SetData(string ten, decimal gia, string imgPath)
        {
            TenMon = ten;
            Gia = gia;
            ImgPath = imgPath;

            lblTenMon.Text = ten;
            lblGia.Text = gia.ToString("N0") + " đ";

            if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                pictureBox1.Image = Image.FromFile(imgPath);
            else
                pictureBox1.Image = Properties.Resources.no_image; // ảnh mặc định
        }

        private void btnThemGio_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Đã thêm {TenMon} vào giỏ hàng!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
