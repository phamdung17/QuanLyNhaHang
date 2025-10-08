using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Utils;
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
            
            // Logic mới: Hiển thị tất cả bàn trống (không chỉ bàn đã được đặt bởi user hiện tại)
            var listBan = BanAnBLL.GetBanTrong();

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


        private void flowBanAn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BanAnForm_Load(object sender, EventArgs e)
        {
            LoadBanAn();
            dtpThoiGian.Value = DateTime.Now;
        }
        private void btnDatBan_Click(object sender, EventArgs e)
        {
            // Validation cơ bản
            int userId = currentUser?.UserID ?? 0;
            if (userId == 0)
            {
                ExceptionHelper.ShowErrorMessage(new ArgumentException("Thông tin người dùng không hợp lệ!"), "Lỗi đặt bàn");
                return;
            }

            DateTime thoiGian = dtpThoiGian.Value;
            bool daChonBan = false;

            // Tìm bàn được chọn
            foreach (Control control in flowBanAn.Controls)
            {
                if (control is UC_BanAn uc && uc.IsSelected)
                {
                    daChonBan = true;
                    int banId = uc.BanID;

                    // Logic mới: Đặt bàn và tạo hóa đơn luôn
                    string result = DatBanBLL.DatBanMoi(banId, userId, thoiGian);
                    
                    if (result.Contains("thành công"))
                    {
                        ExceptionHelper.ShowSuccessMessage(result);
                        LoadBanAn(); // load lại danh sách bàn
                        this.Close(); // Đóng form sau khi đặt bàn thành công
                    }
                    // Nếu có lỗi, ExceptionHelper đã hiển thị thông báo
                    break;
                }
            }

            if (!daChonBan)
            {
                ExceptionHelper.ShowWarningMessage("Vui lòng chọn ít nhất một bàn để đặt!");
            }
        }



        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép nhập số và dấu phân cách thời gian
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ':' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
