using QuanLyNhaHang.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class UC_MonAn : UserControl
    {
        private ThucDonViewModel mon;

        public event EventHandler<ThucDonViewModel> OnAddToCart;

        public UC_MonAn(ThucDonViewModel monAn)
        {
            InitializeComponent();
            mon = monAn;
            LoadData();
        }

        private void LoadData()
        {
            lblTen.Text = mon.TenMon;
            lblGia.Text = mon.DonGia.ToString("N0") + " đ";
            lblLoai.Text = mon.LoaiMon ?? "";
            // đường dẫn tới folder Images trong output (bin/Debug/Images/)
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imgFileName = mon.HinhAnh ?? "no_image.jpg";
            string imgPath = Path.Combine(basePath, "Images", imgFileName);

            Image img = null;
            try
            {
                if (File.Exists(imgPath))
                {
                    // mở bằng FileStream rồi clone vừa tránh file lock vừa an toàn
                    using (var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                    {
                        img = Image.FromStream(fs);
                    }
                }
            }
            catch
            {
                img = null;
            }

            if (img != null)
                picMon.Image = new Bitmap(img);
            else
            {
                // fallback: nếu bạn đã thêm no_image.jpg trong Images thì sẽ load được
                string defaultImg = Path.Combine(basePath, "Images", "no_image.jpg");
                if (File.Exists(defaultImg))
                {
                    using (var fs = new FileStream(defaultImg, FileMode.Open, FileAccess.Read))
                    {
                        picMon.Image = Image.FromStream(fs);
                    }
                }
                else
                {
                    picMon.Image = SystemIcons.Application.ToBitmap();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            OnAddToCart?.Invoke(this, mon);
        }

        private void UC_MonAn_Load(object sender, EventArgs e)
        {

        }
    }
}
