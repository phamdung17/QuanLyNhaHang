using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang.BLL
{
    public class ThucDonBLL
    {
        // Kết nối DAL
        private DAL.ThucDonDAL dal = new DAL.ThucDonDAL();
        public List<ThucDon> GetAll()
        {
            return dal.GetAll();
        }
        // Thêm món ăn
        public string Add(string tenMon, decimal donGia, string donViTinh, bool trangThai)
        {
            return dal.Add(tenMon, donGia, donViTinh, trangThai);
        }
        // Sửa món ăn
        public string Update(int id, string tenMon, decimal donGia, string donViTinh, bool trangThai)
        {
            return dal.Update(id, tenMon, donGia, donViTinh, trangThai);
        }
        // Xóa món ăn
        public string Delete(int id)
        {
            return dal.Delete(id);
        }
        // Lấy danh sách món ăn để hiển thị menu (có ảnh & loại món)
        public static List<ThucDonViewModel> GetMenu()
        {
            using (var db = new Model1()) // tên DbContext của bạn
            {
                var list = db.ThucDon
                    .Where(m => m.TrangThai == true)   // chỉ món còn bán
                    .ToList();

                var vmList = list.Select(m => new ThucDonViewModel
                {
                    MonID = m.MonID,
                    TenMon = m.TenMon,
                    DonGia = m.DonGia,
                    DonViTinh = m.DonViTinh,
                    TrangThai = m.TrangThai ?? false,
                    HinhAnh = GetImageForMon(m.MonID),   // gán tên file ảnh (không phải đường dẫn tuyệt đối)
                    LoaiMon = GetLoaiForMon(m.MonID)
                }).ToList();

                return vmList;
            }
        }

        // --- Tùy bạn chọn 1 trong 2 cách: gán theo MonID (ở đây) hoặc map theo tên/DB ---
        private static string GetImageForMon(int monId)
        {
            // trả về tên file trong thư mục Images (ví dụ: "anh1.jpg")
            switch (monId)
            {
                case 1: return "anh1.jpg";
                case 2: return "anh2.jpg";
                case 3: return "anh3.jpg";
                case 4: return "anh4.jpg";
                case 5: return "anh5.jmg";
                default: return "no_image.jpg";
            }
        }

        private static string GetLoaiForMon(int monId)
        {
            switch (monId)
            {
                case 1: return "Món chính";
                case 2: return "Khai vị";
                default: return "Khác";
            }
        }
    }
}
