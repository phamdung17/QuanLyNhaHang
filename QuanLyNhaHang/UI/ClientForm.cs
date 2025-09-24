using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class ClientForm : Form
    {
        private List<ThucDon> gioHang = new List<ThucDon>();

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            var thucDonList = ThucDonBLL.GetThucDon();

            foreach (var item in thucDonList)
            {
                var uc = new UcMonAn(item); // UserControl custom
                flowThucDon.Controls.Add(uc);
            }
        }

        private void LoadThucDon()
        {
            flowThucDon.Controls.Clear();
            var listMon = ThucDonBLL.GetAll();

            foreach (var mon in listMon)
            {
                var uc = new UC_MonAn(mon);
                uc.OnAddToCart += Uc_OnAddToCart;
                flowThucDon.Controls.Add(uc);
            }
        }

        private void Uc_OnAddToCart(object sender, ThucDon mon)
        {
            gioHang.Add(mon);
            MessageBox.Show($"{mon.TenMon} đã thêm vào giỏ!");
        }
    }
}
