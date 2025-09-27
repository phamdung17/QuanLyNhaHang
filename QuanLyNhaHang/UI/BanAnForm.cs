using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class BanAnForm : Form
    {
        private readonly NguoiDung currentUser;

        public BanAnForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }
        private void LoadBanAn()
        {
            flowBanAn.Controls.Clear();
            using (var db = new Model1())
            {
                var listBan = db.BanAn.Where(b => b.TrangThai == "Trống").ToList();

                foreach (var b in listBan)
                {
                    var uc = new UC_BanAn(b);

                    // Khi 1 bàn được check
                    uc.OnBanChecked += (s, selectedUC) =>
                    {
                        foreach (Control ctrl in flowBanAn.Controls)
                        {
                            if (ctrl is UC_BanAn otherUC && otherUC != selectedUC)
                            {
                                otherUC.Uncheck();
                            }
                        }
                    };

                    flowBanAn.Controls.Add(uc);
                }
            }
        }


        private void flowBanAn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BanAnForm_Load(object sender, EventArgs e)
        {
            LoadBanAn();
        }
        private void btnDatBan_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = currentUser.UserID;
                DateTime thoiGian = DateTime.Now;
                bool daChonBan = false;

                foreach (Control control in flowBanAn.Controls)
                {
                    if (control is UC_BanAn uc && uc.IsSelected)
                    {
                        daChonBan = true;
                        int banId = uc.BanID;
                        DatBanBLL.DatBanMoi(banId, userId, thoiGian);
                    }
                }

                if (!daChonBan)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một bàn để đặt!",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Đặt bàn thành công! Vui lòng chờ Admin duyệt.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadBanAn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đặt bàn: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
