using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Models
{
    public class ThucDonViewModel
    {
        public int MonID { get; set; }
        public string TenMon { get; set; }
        public decimal DonGia { get; set; }
        public string DonViTinh { get; set; }
        public bool? TrangThai { get; set; }

        // Thuộc tính bổ sung ngoài DB
        public string HinhAnh { get; set; }
        public string LoaiMon { get; set; }
    }
}