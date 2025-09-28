using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class DatBanBLL
    {
        // ✅ Lấy bàn còn trống cho khách chọn
        public static List<BanAn> GetBanTrong()
        {
            using (var db = new Model1())
            {
                return db.BanAn.Where(b => b.TrangThai == "Trống").ToList();
            }
        }

        // ✅ Khách đặt bàn (tạo yêu cầu "Chờ duyệt")
        public static void DatBanMoi(int banId, int userId, DateTime thoiGian)
        {
            using (var db = new Model1())
            {
                var ban = db.BanAn.Find(banId);
                if (ban == null)
                    throw new InvalidOperationException("Bàn không tồn tại.");

                if (ban.TrangThai != "Trống")
                    throw new InvalidOperationException("Bàn hiện không trống.");

                var datBan = new DatBan
                {
                    BanID = banId,
                    UserID = userId,
                    NgayDat = thoiGian,
                    TrangThai = "Chờ duyệt"
                };

                db.DatBan.Add(datBan);
                db.SaveChanges();
            }
        }

        // ✅ Admin duyệt yêu cầu đặt bàn
        public static bool DuyetDatBan(int datBanId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan.Find(datBanId);
                if (datBan == null || datBan.TrangThai != "Chờ duyệt")
                    return false;

                datBan.TrangThai = "Đã duyệt";
                var ban = db.BanAn.Find(datBan.BanID);
                if (ban != null) ban.TrangThai = "Đặt trước"; // mapping đúng với CHECK constraint

                db.SaveChanges();
                return true;
            }
        }

        // ✅ Khi khách tới => xác nhận dùng bàn
        public static bool XacNhanDatBan(int datBanId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan.Find(datBanId);
                if (datBan == null || datBan.TrangThai != "Đã duyệt")
                    return false;

                // Giữ trạng thái DatBan = "Đã duyệt" để làm lịch sử
                var ban = db.BanAn.Find(datBan.BanID);
                if (ban != null) ban.TrangThai = "Đang dùng";

                db.SaveChanges();
                return true;
            }
        }

        // ✅ Hủy đặt bàn
        public static bool HuyDatBan(int datBanId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan.Find(datBanId);
                if (datBan == null) return false;

                datBan.TrangThai = "Đã hủy";
                var ban = db.BanAn.Find(datBan.BanID);
                if (ban != null) ban.TrangThai = "Trống";

                db.SaveChanges();
                return true;
            }
        }

        // ✅ Lấy danh sách đặt bàn cho Admin
        public static List<object> GetDanhSachDatBan()
        {
            using (var db = new Model1())
            {
                return db.DatBan
                         .Select(d => new
                         {
                             d.DatBanID,
                             TenBan = d.BanAn.TenBan,
                             NguoiDat = d.NguoiDung.HoTen,
                             d.NgayDat,
                             d.TrangThai
                         })
                         .OrderByDescending(d => d.NgayDat)
                         .ToList<object>();
            }
        }

        // ✅ Lấy danh sách đặt bàn của một user
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
