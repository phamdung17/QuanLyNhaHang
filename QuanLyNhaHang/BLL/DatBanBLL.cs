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

        public static void DatBanMoi(int banId, int userId, DateTime thoiGian)
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
                db.DatBan.Add(datBan);   // ✅ thêm yêu cầu vào DB

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
        // Lấy danh sách tất cả đặt bàn cho Admin duyệt
        public static List<object> GetDanhSachDatBan()
        {
            using (var db = new Model1())
            {
                var list = db.DatBan
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

                return list;
            }
        }
        public static void CapNhatTrangThaiDatBan()
        {
            using (var db = new Model1())
            {
                var now = DateTime.Now;

                var list = db.DatBan
                             .Where(d => d.TrangThai == "Đặt trước" && d.NgayDat <= now)
                             .ToList();

                foreach (var datBan in list)
                {
                    datBan.TrangThai = "Đang dùng";

                    var ban = db.BanAn.Find(datBan.BanID);
                    if (ban != null)
                        ban.TrangThai = "Đang dùng";
                }

                db.SaveChanges();
            }
        }

    }
}