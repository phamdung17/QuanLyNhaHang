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
                var listBan = db.BanAn.ToList();
                foreach (var b in listBan)
                {
                    var uc = new UC_BanAn(b);
                    uc.OnChonBan += (s, ban) =>
                    {
                        // Xử lý đặt bàn trực tiếp
                        DateTime thoiGian = DateTime.Now.AddMinutes(30);
                        string yeuCau = "Đặt bàn nhanh";

                        DatBanBLL.DatBanMoi(ban.BanID, currentUser.UserID, thoiGian, yeuCau);
                        MessageBox.Show($"Đã đặt {ban.TenBan} thành công!", "Thông báo");

                        // Reload lại danh sách bàn
                        LoadBanAn();
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

    }
}
