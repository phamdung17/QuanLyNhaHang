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
    public partial class TKadminForm : Form
    {
        private BLL.NguoiDungBLL bll = new BLL.NguoiDungBLL();
        private int selectedID = 0;
        public TKadminForm()
        {
            InitializeComponent();
        }

        private void TKadminForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bll.GetAll();
            // mặc định role 
            cboRole.SelectedIndex = 1;
            // ẩn các cột không cần thiết
            if (dataGridView1.Columns.Contains("DatBan"))
                dataGridView1.Columns["DatBan"].Visible = false;

            if (dataGridView1.Columns.Contains("HoaDon"))
                dataGridView1.Columns["HoaDon"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserID"].Value);
                txtUsername.Text = dataGridView1.Rows[e.RowIndex].Cells["TenDangNhap"].Value.ToString();
                txtPassword.Text = dataGridView1.Rows[e.RowIndex].Cells["MatKhau"].Value.ToString();
                txtFullName.Text = dataGridView1.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();
                cboRole.Text = dataGridView1.Rows[e.RowIndex].Cells["VaiTro"].Value.ToString();
            }
        }
        
        private void btnThem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(bll.Add(txtUsername.Text, txtPassword.Text, txtFullName.Text, cboRole.Text));
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!");
                return;
            }

            MessageBox.Show(bll.Update(selectedID, txtUsername.Text, txtPassword.Text, txtFullName.Text, cboRole.Text));
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(bll.Delete(selectedID));
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedIndex = 1;
            selectedID = 0;
        }
    }
}
