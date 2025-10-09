using QuanLyNhaHang.BLL;
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class DuyetBanAnForm : Form
    {
        private int datBanId;

        public DuyetBanAnForm()
        {
            InitializeComponent();
            this.Load += DuyetBanAnForm_Load;
        }

        private void DuyetBanAnForm_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupTimer();
        }

        // Load danh sách đặt bàn
        private void LoadData()
        {
            dataGridView1.DataSource = DatBanBLL.GetDanhSachDatBan();

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["DatBanID"].HeaderText = "Mã đặt bàn";
                dataGridView1.Columns["TenBan"].HeaderText = "Tên bàn";
                dataGridView1.Columns["NguoiDat"].HeaderText = "Người đặt";
                dataGridView1.Columns["NgayDat"].HeaderText = "Thời gian đặt";
                dataGridView1.Columns["TrangThaiYeuCau"].HeaderText = "Trạng thái yêu cầu";
                dataGridView1.Columns["TrangThaiBan"].HeaderText = "Trạng thái bàn";
            }

            datBanId = 0;
        }


        // Chọn dòng trong datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                datBanId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["DatBanID"].Value);
            }
        }

        // Nút Load lại
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // ✅ Xác nhận khách đã đến (chuyển sang Đang dùng)
        private void btnDuyet_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                string result = DatBanBLL.DuyetDatBan(datBanId);
                if (result.Contains("thành công"))
                {
                    MessageBox.Show("Đã chuyển bàn sang trạng thái 'Đang dùng'!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }
        
        // ✅ Nút Hủy (Admin hủy yêu cầu)
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                string result = DatBanBLL.HuyDatBan(datBanId);
                if (result.Contains("thành công"))
                {
                    MessageBox.Show("Hủy thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }

        // ✅ Xóa lịch sử đặt bàn (chỉ với yêu cầu đã hủy)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                var confirm = MessageBox.Show("Bạn có chắc muốn xóa yêu cầu đã hủy này?", "Xóa lịch sử", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    string result = DatBanBLL.XoaDatBan(datBanId);
                    MessageBox.Show(result);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }
        private void SetupTimer()
        {
            // timer1 là tên mặc định của control Timer bạn kéo vào
            timerRefresh.Interval = 10000; // 30000 mili-giây = 30 giây
            timerRefresh.Tick += new EventHandler(timerRefresh_Tick);
            timerRefresh.Start();
        }
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Gọi lại các hàm tải dữ liệu của bạn ở đây
            LoadData();

        }
        private void LichSuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng timer khi form đóng lại
            timerRefresh.Stop();
        }

    }
}
