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
                    // Logic mới: Kiểm tra đã đặt bàn chưa trước khi order
                    string result = HoaDonBLL.ThemMonVaoHoaDon(currentUser.UserID, m.MonID, 1);
                    
                    if (result.Contains("thành công") || result.Contains("Đã thêm"))
                    {
                        RefreshCartUI();
                        MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (result.Contains("chưa đặt bàn") || result.Contains("đặt bàn"))
                    {
                        // Nếu chưa đặt bàn, hỏi có muốn đặt bàn không
                        var dialogResult = MessageBox.Show(result + "\n\nBạn có muốn đặt bàn ngay không?", 
                            "Yêu cầu đặt bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        
                        if (dialogResult == DialogResult.Yes)
                        {
                            using (var banAnForm = new BanAnForm(currentUser))
                            {
                                banAnForm.ShowDialog();
                                RefreshCartUI(); // Refresh sau khi đặt bàn
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
            // Logic mới: Lấy dữ liệu từ hóa đơn hiện tại thay vì từ GioHangBLL
            var chiTietHoaDon = HoaDonBLL.GetChiTietHoaDonHienTai(currentUser.UserID);
            
            // Convert sang GioHangItem để tương thích với UI
            var items = new List<GioHangItem>();
            
            foreach (dynamic item in chiTietHoaDon)
            {
                items.Add(new GioHangItem
                {
                    MonID = item.MonID, // Sử dụng MonID trực tiếp từ database
                    TenMon = item.TenMon,
                    DonGia = item.DonGia,
                    SoLuong = item.SoLuong
                });
            }

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
                // Logic mới: Xóa món khỏi hóa đơn hiện tại
                var hoaDon = HoaDonBLL.GetHoaDonHienTai(currentUser.UserID);
                if (hoaDon != null && item.MonID > 0)
                {
                    string result = HoaDonBLL.XoaMonKhoiHoaDon(hoaDon.HoaDonID, item.MonID);
                    if (result.Contains("thành công"))
                    {
                        RefreshCartUI();
                        MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể xóa món này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnDatMon_Click(object sender, EventArgs e)
        {
            try
            {
                // Logic mới: Kiểm tra có thể order không
                string kiemTra = HoaDonBLL.KiemTraCoTheOrder(currentUser.UserID);
                if (kiemTra != "OK")
                {
                    var r = MessageBox.Show(
                        kiemTra + "\n\nBạn có muốn đặt bàn ngay không?",
                        "Yêu cầu đặt bàn",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        using (var f = new BanAnForm(currentUser))
                        {
                            f.ShowDialog();
                            RefreshCartUI(); // Refresh sau khi đặt bàn
                        }
                    }
                    return;
                }

                // Kiểm tra có món trong hóa đơn không
                var chiTietHoaDon = HoaDonBLL.GetChiTietHoaDonHienTai(currentUser.UserID);
                if (chiTietHoaDon == null || chiTietHoaDon.Count == 0)
                {
                    MessageBox.Show("Bạn chưa có món nào trong hóa đơn!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show("Đặt món thành công! Hóa đơn đã được cập nhật.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                RefreshCartUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion

        private void ThucDonOder_Load(object sender, EventArgs e)
        {
            LoadThucDon();
            InitGioHangGrid();
            RefreshCartUI();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadThucDon();
        }
    }
}
       