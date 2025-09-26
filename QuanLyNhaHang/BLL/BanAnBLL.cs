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
    }
}