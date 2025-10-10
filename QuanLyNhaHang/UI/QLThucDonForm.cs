using QuanLyNhaHang.BLL;
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
    public partial class QLThucDonForm : Form
    {
        private ThucDonBLL bll = new ThucDonBLL();
        private int selectedID = 0;

        public QLThucDonForm()
        {
            InitializeComponent();
        }
        private void QLThucDonForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Giả sử bll là instance của ThucDonBLL
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = bll.GetAll();

                // ✨ THÊM ĐOẠN CODE NÀY ĐỂ ĐẶT LẠI TÊN CỘT ✨
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["MonID"].HeaderText = "Mã Món";
                    dataGridView1.Columns["TenMon"].HeaderText = "Tên Món Ăn";
                    dataGridView1.Columns["DonGia"].HeaderText = "Đơn Giá";
                    dataGridView1.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";
                    dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";

                    // Định dạng cột tiền tệ cho dễ đọc (tùy chọn)
                    dataGridView1.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                }

                // Ẩn các cột không cần thiết (navigation property)
                if (dataGridView1.Columns.Contains("ChiTietHoaDon"))
                {
                    dataGridView1.Columns["ChiTietHoaDon"].Visible = false;
                }

                // Tự động điều chỉnh độ rộng cột cho vừa với nội dung
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Reset lại ID đang được chọn
                selectedID = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thực đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MonID"].Value);
                txtTenMon.Text = dataGridView1.Rows[e.RowIndex].Cells["TenMon"].Value.ToString();
                txtDonGia.Text = dataGridView1.Rows[e.RowIndex].Cells["DonGia"].Value.ToString();
                cboDonViTinh.Text = dataGridView1.Rows[e.RowIndex].Cells["DonViTinh"].Value.ToString();
                chkTrangThai.Checked = Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["TrangThai"].Value);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(bll.Add(txtTenMon.Text, decimal.Parse(txtDonGia.Text), cboDonViTinh.Text, chkTrangThai.Checked));
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) { MessageBox.Show("Chưa chọn món!"); return; }
            MessageBox.Show(bll.Update(selectedID, txtTenMon.Text, decimal.Parse(txtDonGia.Text), cboDonViTinh.Text, chkTrangThai.Checked));
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) { MessageBox.Show("Chưa chọn món!"); return; }
            if (MessageBox.Show("Xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(bll.Delete(selectedID));
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTenMon.Clear();
            txtDonGia.Clear();
            cboDonViTinh.SelectedIndex = -1;
            chkTrangThai.Checked = false;
            selectedID = 0;
            LoadData();
        }

        private void QLThucDonForm_Load_1(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
