using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.UI
{
    public partial class LichSuForm : Form
    {
        private readonly NguoiDung currentUser;

        public LichSuForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void LichSuForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = DatBanBLL.GetDatBanByUser(currentUser.UserID);
                // giả sử có DataGridView tên dgvLichSu
                var grid = this.Controls.Find("dgvLichSu", true);
                if (grid.Length > 0 && grid[0] is DataGridView dgv)
                {
                    dgv.DataSource = list.Select(d => new
                    {
                        d.DatBanID,
                        Ban = d.BanAn?.TenBan,
                        d.NgayDat,
                        d.TrangThai
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
