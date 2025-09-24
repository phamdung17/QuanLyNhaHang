using QuanLyNhaHang.Models;  
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    public partial class ClientForm : Form
    {
        private NguoiDung currentUser;  // ✅ user hiện tại

        // Constructor nhận user
        public ClientForm(NguoiDung user)
        {
            InitializeComponent();
            currentUser = user; // gán user truyền vào biến thành viên

        }
        private void LoadThucDon()
        {
            flowThucDon.Controls.Clear();

            var listMon = new List<(string Ten, decimal Gia, string Img)>
            {
                ("Phở bò", 45000, "Images/pho_bo.jpg"),
                ("Cơm gà", 40000, "Images/com_ga.jpg"),
                ("Lẩu thái", 120000, "Images/lau_thai.jpg"),
                ("Trà chanh", 15000, "Images/tra_chanh.jpg")
            };

            foreach (var mon in listMon)
            {
                UC_MonAn uc = new UC_MonAn();
                uc.SetData(mon.Ten, mon.Gia, mon.Img);
                flowThucDon.Controls.Add(uc);
            }
        }
        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Ví dụ: hiển thị tên client ở tiêu đề
            this.Text = $"Client Panel - Xin chào {currentUser.HoTen}";

            LoadThucDon();
        }

    }
}
