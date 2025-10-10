using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Utils;
using System;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class TKadminForm : Form
    {
        private NguoiDungBLL bll = new NguoiDungBLL();
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
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = bll.GetAll();

                // ✨ THÊM ĐOẠN CODE NÀY ĐỂ ĐẶT LẠI TÊN CỘT ✨
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["UserID"].HeaderText = "Mã TK";
                    dataGridView1.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                    dataGridView1.Columns["MatKhau"].HeaderText = "Mật khẩu";
                    dataGridView1.Columns["HoTen"].HeaderText = "Họ và tên";
                    dataGridView1.Columns["VaiTro"].HeaderText = "Vai trò";

                    // Thay đổi độ rộng cho cột để dễ nhìn hơn (tùy chọn)
                    dataGridView1.Columns["TenDangNhap"].Width = 150;
                    dataGridView1.Columns["HoTen"].Width = 200;
                }

                // Ẩn các cột không cần thiết
                if (dataGridView1.Columns.Contains("DatBan"))
                    dataGridView1.Columns["DatBan"].Visible = false;

                if (dataGridView1.Columns.Contains("HoaDon"))
                    dataGridView1.Columns["HoaDon"].Visible = false;

                ClearInputs();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải danh sách tài khoản");
            }
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedIndex = 1; 
            selectedID = 0;
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserID"].Value);
                txtUsername.Text = dataGridView1.Rows[e.RowIndex].Cells["TenDangNhap"].Value?.ToString();
                txtPassword.Text = dataGridView1.Rows[e.RowIndex].Cells["MatKhau"].Value?.ToString();
                txtFullName.Text = dataGridView1.Rows[e.RowIndex].Cells["HoTen"].Value?.ToString();
                cboRole.Text = dataGridView1.Rows[e.RowIndex].Cells["VaiTro"].Value?.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string result = bll.Add(txtUsername.Text, txtPassword.Text, txtFullName.Text, cboRole.Text);
            if (result.Contains("thành công"))
            {
                ExceptionHelper.ShowSuccessMessage(result);
                LoadData();
            }
            else
            {
                ExceptionHelper.ShowWarningMessage(result);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string result = bll.Update(selectedID, txtUsername.Text, txtPassword.Text, txtFullName.Text, cboRole.Text);
            if (result.Contains("thành công"))
            {
                ExceptionHelper.ShowSuccessMessage(result);
                LoadData();
            }
            else
            {
                ExceptionHelper.ShowWarningMessage(result);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                ExceptionHelper.ShowWarningMessage("Vui lòng chọn tài khoản cần xóa!");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                string result = bll.Delete(selectedID);
                if (result.Contains("thành công"))
                {
                    ExceptionHelper.ShowSuccessMessage(result);
                    LoadData();
                }
                else
                {
                    ExceptionHelper.ShowWarningMessage(result);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}