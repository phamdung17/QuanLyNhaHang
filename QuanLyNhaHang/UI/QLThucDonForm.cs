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
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bll.GetAll();

            // Tùy chỉnh hiển thị cột
            dataGridView1.Columns["ChiTietHoaDon"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            selectedID = 0;
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
