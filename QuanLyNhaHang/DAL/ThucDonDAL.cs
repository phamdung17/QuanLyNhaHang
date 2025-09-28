using System.Collections.Generic;
using System.Linq;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.DAL
{
    public class ThucDonDAL
    {
        private Model1 db = new Model1();

        public List<ThucDon> GetAll()
        {
            return db.ThucDon.ToList();
        }

        public string Add(string tenMon, decimal donGia, string donViTinh, bool trangThai)
        {
            if (db.ThucDon.Any(x => x.TenMon == tenMon))
                return "Món ăn đã tồn tại!";

            var mon = new ThucDon
            {
                TenMon = tenMon,
                DonGia = donGia,
                DonViTinh = donViTinh,
                TrangThai = trangThai
            };
            db.ThucDon.Add(mon);
            db.SaveChanges();
            return "Thêm món thành công!";
        }

        public string Update(int id, string tenMon, decimal donGia, string donViTinh, bool trangThai)
        {
            var mon = db.ThucDon.Find(id);
            if (mon == null) return "Không tìm thấy món!";

            mon.TenMon = tenMon;
            mon.DonGia = donGia;
            mon.DonViTinh = donViTinh;
            mon.TrangThai = trangThai;
            db.SaveChanges();
            return "Cập nhật thành công!";
        }

        public string Delete(int id)
        {
            var mon = db.ThucDon.Find(id);
            if (mon == null) return "Không tìm thấy món!";

            db.ThucDon.Remove(mon);
            db.SaveChanges();
            return "Xóa món thành công!";
        }
    }
}
