using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class BanAnBLL
    {
        public static List<BanAn> GetAll()
        {
            using (var db = new Model1())
            {
                return db.BanAn.ToList();
            }
        }

        // Thêm bàn
        public static void ThemBan(string tenBan)
        {
            using (var db = new Model1())
            {
                var ban = new BanAn
                {
                    TenBan = tenBan,
                    TrangThai = "Trống"
                };
                db.BanAn.Add(ban);
                db.SaveChanges();
            }
        }

        // Sửa bàn
        public static void SuaBan(int banId, string tenBan, string trangThai)
        {
            using (var db = new Model1())
            {
                var ban = db.BanAn.Find(banId);
                if (ban == null)
                    throw new Exception("Không tìm thấy bàn.");

                ban.TenBan = tenBan;
                ban.TrangThai = trangThai;
                db.SaveChanges();
            }
        }

        // Xóa bàn
        public static void XoaBan(int banId)
        {
            using (var db = new Model1())
            {
                var ban = db.BanAn.Find(banId);
                if (ban == null)
                    throw new Exception("Không tìm thấy bàn cần xóa.");
                db.BanAn.Remove(ban);
                db.SaveChanges();
            }
        }
        public static void CapNhatTrangThai(int banId, string trangThai)
        {
            using (var db = new Model1())
            {
                var ban = db.BanAn.Find(banId);
                if (ban == null)
                    throw new Exception("Không tìm thấy bàn.");

                ban.TrangThai = trangThai;
                db.SaveChanges();
            }
        }
        
    }
}