using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class DuyetBanAnForm : Form
    {
        private int datBanId;

        public DuyetBanAnForm()
        {
            InitializeComponent();
            this.Load += DuyetBanAnForm_Load;  // Gắn sự kiện Load
        }

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
        }

        private void DuyetBanAnForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Duyệt đặt bàn => đổi trạng thái bàn, xóa record khỏi DatBan
        private void DuyetDatBan(int datBanId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan.Find(datBanId);
                if (datBan != null)
                {
                    var ban = db.BanAn.Find(datBan.BanID);
                    if (ban != null)
                        ban.TrangThai = "Đặt trước";

                    db.DatBan.Remove(datBan); // xóa yêu cầu khỏi bảng DatBan
                    db.SaveChanges();
                }
            }
        }

        // Hủy đặt bàn => bàn thành "Trống", xóa record khỏi DatBan
        private void HuyDatBan(int datBanId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan.Find(datBanId);
                if (datBan != null)
                {
                    var ban = db.BanAn.Find(datBan.BanID);
                    if (ban != null)
                        ban.TrangThai = "Trống";

                    db.DatBan.Remove(datBan); // xóa yêu cầu
                    db.SaveChanges();
                }
            }
        }

        // chọn dòng trong datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                datBanId = Convert.ToInt32(row.Cells["DatBanID"].Value);
            }
        }

        // nút Load lại
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // nút Duyệt
        private void button1_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                DuyetDatBan(datBanId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }

        // nút Hủy
        private void button2_Click(object sender, EventArgs e)
        {
            if (datBanId > 0)
            {
                HuyDatBan(datBanId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn yêu cầu đặt bàn!");
            }
        }
    }
}
