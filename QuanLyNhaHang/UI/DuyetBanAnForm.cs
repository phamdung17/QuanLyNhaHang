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
                dataGridView1.Columns["TrangThai"].HeaderText = "Trạng thái";
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

        // ✅ Nút Duyệt (Admin duyệt yêu cầu)
        private void btnDuyet_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                if (DatBanBLL.DuyetDatBan(datBanId))
                {
                    MessageBox.Show("Duyệt thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không thể duyệt yêu cầu này!");
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
                if (DatBanBLL.HuyDatBan(datBanId))
                {
                    MessageBox.Show("Hủy thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không thể hủy yêu cầu này!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }

        // ✅ Nút Xác nhận (Khách đến, đổi bàn sang Đang dùng)
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                if (DatBanBLL.XacNhanDatBan(datBanId))
                {
                    MessageBox.Show("Xác nhận thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không thể xác nhận!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }
    }
}
