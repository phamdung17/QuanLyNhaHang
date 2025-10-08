using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang.BLL
{
    public class ThucDonBLL
    {
     
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
            using (var db = new Model1()) 
            {
                var list = db.ThucDon
                    .Where(m => m.TrangThai == true && m.TrangThai != null)   
                    .ToList();

                var vmList = list.Select(m => new ThucDonViewModel
                {
                    MonID = m.MonID,
                    TenMon = m.TenMon,
                    DonGia = m.DonGia,
                    DonViTinh = m.DonViTinh,
                    TrangThai = m.TrangThai ?? false,
                    HinhAnh = GetImageForMon(m.MonID),  
                    LoaiMon = GetLoaiForMon(m.MonID)
                }).ToList();

                return vmList;
            }
        }
        private static string GetImageForMon(int monId)
        {
            return (monId >= 1 && monId <= 12) ? $"anh{monId}.jpg" : "no_image.png";
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
