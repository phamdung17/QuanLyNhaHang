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
        public event EventHandler<UC_BanAn> OnBanChecked;


        public CheckBox chkChonPublic => chkChon;

        public UC_BanAn(BanAn b)
        {
            InitializeComponent();
            ban = b;
            LoadData();
        }
        // Public read-only property để form ngoài lấy ID bàn
        public int BanID => ban?.BanID ?? 0;

        // Public property để kiểm tra/bật chọn (thay vì truy cập chkChon trực tiếp)
        public bool IsSelected
        {
            get => chkChon != null && chkChon.Checked;
            set
            {
                if (chkChon != null) chkChon.Checked = value;
            }
        }
        private void LoadData()
        {
            lblTenBan.Text = ban.TenBan;
            lblTrangThai.Text = ban.TrangThai;

            // Tô màu theo trạng thái
            switch (ban.TrangThai)
            {
                case "Trống":
                    this.BackColor = Color.LightGreen;
                    break;
                case "Đang dùng":
                    this.BackColor = Color.Orange;
                    break;
                case "Chờ duyệt":
                    this.BackColor = Color.Yellow;
                    break;
                default:
                    this.BackColor = Color.LightGray;
                    break;
            }

            // Nếu bàn không trống thì disable checkbox chọn
            chkChon.Enabled = (ban.TrangThai == "Trống");
        }
       
        private void UC_BanAn_Click(object sender, EventArgs e)
        {
            OnChonBan?.Invoke(this, ban);
        }

        private void chkChon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChon.Checked && ban.TrangThai != "Trống")
            {
                MessageBox.Show("Bàn này không khả dụng!", "Thông báo");
                chkChon.Checked = false;
            }
            else if (chkChon.Checked) // chỉ bắn sự kiện khi chọn bàn hợp lệ
            {
                OnBanChecked?.Invoke(this, this);
            }
        }

        // Hàm hỗ trợ bỏ check từ bên ngoài
        public void Uncheck()
        {
            chkChon.Checked = false;
        }

    }
}