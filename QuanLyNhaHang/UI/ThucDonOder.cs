using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
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
    public partial class ThucDonOder : Form
    {
        private readonly GioHangBLL gioHang = new GioHangBLL();
        private readonly NguoiDung currentUser;
        private BindingList<GioHangItem> cartBinding;

        public ThucDonOder(NguoiDung User)
        {
            InitializeComponent();
            currentUser = User;
        }

       

        #region Load Thực đơn
        private void LoadThucDon()
        {
            flowThucDon.Controls.Clear();
            var menu = ThucDonBLL.GetMenu();

            foreach (var mon in menu)
            {
                var uc = new UC_MonAn(mon);
                uc.OnAddToCart += (s, m) =>
                {
                    gioHang.Add(m, 1);
                    RefreshCartUI();
                    MessageBox.Show($"Đã thêm {m.TenMon} vào giỏ hàng", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
                flowThucDon.Controls.Add(uc);
            }
        }

       
        #endregion

        #region Giỏ hàng (DataGridView)
        private void InitGioHangGrid()
        {
            dgvGioHang.AutoGenerateColumns = false;
            dgvGioHang.Columns.Clear();

            dgvGioHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenMon",
                HeaderText = "Tên món",
                DataPropertyName = "TenMon",
                ReadOnly = true
            });
            dgvGioHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonGia",
                HeaderText = "Đơn giá",
                DataPropertyName = "DonGia",
                ReadOnly = true
            });
            dgvGioHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong",
                HeaderText = "Số lượng",
                DataPropertyName = "SoLuong"
            });
            dgvGioHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien",
                ReadOnly = true
            });

            dgvGioHang.CellValueChanged += DgvGioHang_CellValueChanged;
            dgvGioHang.CurrentCellDirtyStateChanged += DgvGioHang_CurrentCellDirtyStateChanged;
        }

        private void DgvGioHang_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvGioHang.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                if (dgvGioHang.Rows[e.RowIndex].DataBoundItem is GioHangItem item)
                {
                    if (!int.TryParse(dgvGioHang.Rows[e.RowIndex].Cells["SoLuong"].Value?.ToString(), out int sl))
                        sl = item.SoLuong;

                    gioHang.UpdateQuantity(item.MonID, sl);
                    RefreshCartUI();
                }
            }
        }

        private void DgvGioHang_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvGioHang.IsCurrentCellDirty)
                dgvGioHang.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void RefreshCartUI()
        {
            var items = gioHang.GetItems() ?? new List<GioHangItem>();
            cartBinding = new BindingList<GioHangItem>(items);

            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = cartBinding;

            decimal tongTien = items.Sum(i => i.DonGia * i.SoLuong);

            if (txtTongTien != null)
            {
                txtTongTien.Text = tongTien.ToString("N0") + " đ";
                txtTongTien.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Nút chức năng
        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow?.DataBoundItem is GioHangItem item)
            {
                gioHang.Remove(item.MonID);
                RefreshCartUI();
            }
        }

        private void btnDatMon_Click(object sender, EventArgs e)
        {
            if (gioHang.IsEmpty())
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Cảnh báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Lưu hóa đơn qua HoaDonBLL
                // HoaDonBLL.TaoHoaDon(currentUser.UserId, gioHang.GetItems());

                MessageBox.Show("Đặt món thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                gioHang.Clear();
                RefreshCartUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đặt món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion

        private void ThucDonOder_Load(object sender, EventArgs e)
        {
            LoadThucDon();
            InitGioHangGrid();
            RefreshCartUI();
        }
    }
}
       