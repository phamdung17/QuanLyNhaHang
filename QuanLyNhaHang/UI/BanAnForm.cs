using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Utils;
using System;
using System.Drawing;
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

        private void BanAnForm_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị tối thiểu cho DateTimePicker là thời điểm hiện tại
            dtpThoiGian.MinDate = DateTime.Now;
            dtpThoiGian.Value = DateTime.Now;
            LoadBanAn();
        }

        /// <summary>
        /// Tải và hiển thị danh sách các bàn còn trống
        /// </summary>
        private void LoadBanAn()
        {
            try
            {
                flowBanAn.Controls.Clear();

                var listBanTrong = BanAnBLL.GetBanTrong();

                foreach (var ban in listBanTrong)
                {
                    var uc = new UC_BanAn(ban);

                    // Đăng ký sự kiện để đảm bảo chỉ chọn được 1 bàn
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách bàn ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDatBan_Click(object sender, EventArgs e)
        {
            // 1. Lấy thông tin người dùng và bàn được chọn
            int userId = currentUser?.UserID ?? 0;
            if (userId == 0)
            {
                MessageBox.Show("Thông tin người dùng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime thoiGian = dtpThoiGian.Value;
            UC_BanAn selectedUC = null;

            // 2. Tìm User Control của bàn đã được chọn
            foreach (Control control in flowBanAn.Controls)
            {
                if (control is UC_BanAn uc && uc.IsSelected)
                {
                    selectedUC = uc;
                    break;
                }
            }

            // 3. Nếu đã chọn được bàn, tiến hành gọi BLL để đặt
            if (selectedUC != null)
            {
                int banId = selectedUC.BanID;

                // Gọi BLL để thực hiện đặt bàn
                string result = DatBanBLL.DatBanMoi(banId, userId, thoiGian);

                // 4. Xử lý kết quả trả về
                if (result.Contains("thành công"))
                {
                    MessageBox.Show(result, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    
                }
                else // ✨ THÊM LOGIC XỬ LÝ KHI THẤT BẠI
                {
                    // Hiển thị thông báo lỗi từ BLL (ví dụ: "Bạn đã đặt bàn rồi!...")
                    MessageBox.Show(result, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Làm mới danh sách bàn để người dùng thấy trạng thái mới nhất
                    LoadBanAn();
                }
            }
            else // Nếu chưa chọn bàn nào
            {
                MessageBox.Show("Vui lòng chọn một bàn để đặt!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadBanAn();
        }
    }
    }