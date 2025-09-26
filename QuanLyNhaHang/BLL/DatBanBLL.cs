using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class DatBanBLL
    {
        public static List<BanAn> GetBanTrong()
        {
            using (var db = new Model1())
            {
                return db.BanAn.Where(b => b.TrangThai == "Trống").ToList();
            }
        }

        public static void DatBanMoi(int banId, int userId, DateTime thoiGian, string yeuCau)
        {
            using (var db = new Model1())
            {
                var datBan = new DatBan
                {
                    BanID = banId,
                    UserID = userId,
                    NgayDat = thoiGian,
                    TrangThai = "Chờ duyệt"
                };
                db.DatBan.Add(datBan);

                // Cập nhật trạng thái bàn
                var ban = db.BanAn.Find(banId);
                if (ban != null) ban.TrangThai = "Chờ duyệt";

                db.SaveChanges();
            }
        }

        public static List<DatBan> GetDatBanByUser(int userId)
        {
            using (var db = new Model1())
            {
                return db.DatBan
                         .Where(d => d.UserID == userId)
                         .OrderByDescending(d => d.NgayDat)
                         .ToList();
            }
        }
    }
}