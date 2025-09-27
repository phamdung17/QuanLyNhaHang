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
    public partial class QLBanAnForm : Form
    {
        private int selectedBanId = -1;

        public QLBanAnForm()
        {
            InitializeComponent();
            
        }

        private void LoadData()
        {
            dataGridView1.DataSource = BanAnBLL.GetAll()
                .Select(b => new
                {
                    b.BanID,
                    b.TenBan,
                    b.TrangThai
                }).ToList();

            dataGridView1.Columns["BanID"].HeaderText = "Mã bàn";
            dataGridView1.Columns["TenBan"].HeaderText = "Tên bàn";
            dataGridView1.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        // Khi chọn 1 dòng trong datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // đảm bảo click vào dòng hợp lệ
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedBanId = Convert.ToInt32(row.Cells["BanID"].Value);
                txtTenBan.Text = row.Cells["TenBan"].Value.ToString();
                cboTrangThai.SelectedItem = row.Cells["TrangThai"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenBan = txtTenBan.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenBan))
            {
                MessageBox.Show("Vui lòng nhập tên bàn.");
                return;
            }

            // Kiểm tra trùng tên bàn
            var dsBan = BanAnBLL.GetAll();
            if (dsBan.Any(b => b.TenBan.Equals(tenBan, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Tên bàn đã tồn tại, vui lòng nhập tên khác.");
                return;
            }

            // Nếu không trùng thì thêm
            BanAnBLL.ThemBan(tenBan);
            LoadData();

            // Xóa input để nhập mới
            txtTenBan.Clear();
            cboTrangThai.SelectedIndex = 0;
        }
        // chọn bạn để sửa hoặc xóa trong datagridview
        

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedBanId == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn để sửa.");
                return;
            }

            BanAnBLL.SuaBan(selectedBanId, txtTenBan.Text, cboTrangThai.Text);
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedBanId == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BanAnBLL.XoaBan(selectedBanId);
                LoadData();
                selectedBanId = -1;
                txtTenBan.Clear();
                cboTrangThai.SelectedIndex = -1;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = -1;
            selectedBanId = -1;
            LoadData();
        }

        private void QLBanAnForm_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
            LoadData();
        }
    }

}

