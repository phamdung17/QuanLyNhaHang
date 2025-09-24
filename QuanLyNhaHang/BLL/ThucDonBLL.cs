using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class ThucDonBLL
    {
        public static List<ThucDonViewModel> GetThucDon()
        {
            using (var db = new QuanLyNhaHangDbContext())
            {
                var list = db.ThucDons.ToList();

                // Mapping sang ViewModel
                var result = list.Select(m => new ThucDonViewModel
                {
                    MonID = m.MonID,
                    TenMon = m.TenMon,
                    DonGia = m.DonGia,
                    DonViTinh = m.DonViTinh,
                    TrangThai = m.TrangThai,

                    // Setup ngoài
                    HinhAnh = GetImagePath(m.MonID),
                    LoaiMon = GetCategory(m.MonID)
                }).ToList();

                return result;
            }
        }

        private static string GetImagePath(int monId)
        {
            // Gắn ảnh theo ID hoặc theo logic khác
            switch (monId)
            {
                case 1: return "Images/pho.jpg";
                case 2: return "Images/coca.jpg";
                default: return "Images/default.png";
            }
        }

        private static string GetCategory(int monId)
        {
            // Tạm hardcode loại món
            if (monId == 1) return "Món chính";
            if (monId == 2) return "Đồ uống";
            return "Khác";
        }
    }
}
