using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace QuanLyNhaHang.DAL
{
    public class HoaDonDAL
    {
        private Model1 context = new Model1();

        public List<HoaDon> GetAll()
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .Include("ChiTietHoaDon")
                .OrderByDescending(h => h.NgayLap)
                .ToList();
        }

        public HoaDon GetById(int id)
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .Include("ChiTietHoaDon")
                .FirstOrDefault(h => h.HoaDonID == id);
        }

        public List<HoaDon> GetByUserId(int userId)
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .Where(h => h.UserID == userId)
                .OrderByDescending(h => h.NgayLap)
                .ToList();
        }

        public List<HoaDon> GetByTrangThai(string trangThai)
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .Where(h => h.TrangThai == trangThai)
                .OrderByDescending(h => h.NgayLap)
                .ToList();
        }

        public List<HoaDon> GetChuaThanhToan()
        {
            return GetByTrangThai("Chưa thanh toán");
        }

        public List<HoaDon> GetDaThanhToan()
        {
            return GetByTrangThai("Đã thanh toán");
        }

        public HoaDon GetHoaDonChuaThanhToanByBan(int banId)
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .FirstOrDefault(h => h.BanID == banId && h.TrangThai == "Chưa thanh toán");
        }

        public HoaDon GetHoaDonChuaThanhToanByUser(int userId)
        {
            return context.HoaDon
                .Include("BanAn")
                .Include("NguoiDung")
                .FirstOrDefault(h => h.UserID == userId && h.TrangThai == "Chưa thanh toán");
        }

        public string Add(int banId, int userId, DateTime ngayLap, decimal tongTien = 0, string trangThai = "Chưa thanh toán")
        {
            // Kiểm tra bàn có hóa đơn chưa thanh toán chưa
            if (context.HoaDon.Any(h => h.BanID == banId && h.TrangThai == "Chưa thanh toán"))
                return "Bàn này đã có hóa đơn chưa thanh toán!";

            var hoaDon = new HoaDon
            {
                BanID = banId,
                UserID = userId,
                NgayLap = ngayLap,
                TongTien = tongTien,
                TrangThai = trangThai
            };

            context.HoaDon.Add(hoaDon);
            context.SaveChanges();
            return "Tạo hóa đơn thành công!";
        }

        public string UpdateTrangThai(int id, string trangThai)
        {
            var hoaDon = context.HoaDon.Find(id);
            if (hoaDon == null)
                return "Không tìm thấy hóa đơn!";

            string oldTrangThai = hoaDon.TrangThai;
            hoaDon.TrangThai = trangThai;

            // Nếu thanh toán thì cập nhật trạng thái bàn
            if (trangThai == "Đã thanh toán")
            {
                var ban = context.BanAn.Find(hoaDon.BanID);
                if (ban != null)
                    ban.TrangThai = "Trống";
            }

            context.SaveChanges();
            return $"Cập nhật trạng thái hóa đơn thành công! ({oldTrangThai} → {trangThai})";
        }

        public string UpdateTongTien(int id, decimal tongTien)
        {
            var hoaDon = context.HoaDon.Find(id);
            if (hoaDon == null)
                return "Không tìm thấy hóa đơn!";

            hoaDon.TongTien = tongTien;
            context.SaveChanges();
            return "Cập nhật tổng tiền thành công!";
        }

        public string ThanhToan(int id)
        {
            return UpdateTrangThai(id, "Đã thanh toán");
        }

        public string Delete(int id)
        {
            var hoaDon = context.HoaDon.Find(id);
            if (hoaDon == null)
                return "Không tìm thấy hóa đơn!";

            // Chỉ cho phép xóa hóa đơn chưa thanh toán
            if (hoaDon.TrangThai == "Đã thanh toán")
                return "Không thể xóa hóa đơn đã thanh toán!";

            // Xóa chi tiết hóa đơn trước
            var chiTiet = context.ChiTietHoaDon.Where(c => c.HoaDonID == id).ToList();
            context.ChiTietHoaDon.RemoveRange(chiTiet);

            // Xóa hóa đơn
            context.HoaDon.Remove(hoaDon);
            context.SaveChanges();
            return "Xóa hóa đơn thành công!";
        }

        public List<object> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return context.HoaDon
                .Where(h => h.TrangThai == "Đã thanh toán" && h.NgayLap.HasValue &&
                           h.NgayLap.Value >= tuNgay && 
                           h.NgayLap.Value <= denNgay)
                .GroupBy(h => h.NgayLap.Value.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoHoaDon = g.Count(),
                    TongTien = g.Sum(x => (decimal?)x.TongTien) ?? 0
                })
                .OrderBy(x => x.Ngay)
                .ToList<object>();
        }

        public List<object> GetDoanhThuTheoThang(int nam)
        {
            return context.HoaDon
                .Where(h => h.TrangThai == "Đã thanh toán" && h.NgayLap.HasValue &&
                           h.NgayLap.Value.Year == nam)
                .GroupBy(h => h.NgayLap.Value.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    SoHoaDon = g.Count(),
                    TongTien = g.Sum(x => (decimal?)x.TongTien) ?? 0
                })
                .OrderBy(x => x.Thang)
                .ToList<object>();
        }

        public List<object> GetDoanhThuTheoQuy(int nam)
        {
            return context.HoaDon
                .Where(h => h.TrangThai == "Đã thanh toán" && h.NgayLap.HasValue && h.NgayLap.Value.Year == nam)
                .GroupBy(h => ((h.NgayLap.Value.Month - 1) / 3) + 1)
                .Select(g => new
                {
                    Quy = g.Key,
                    SoHoaDon = g.Count(),
                    TongTien = g.Sum(x => (decimal?)x.TongTien) ?? 0
                })
                .OrderBy(x => x.Quy)
                .ToList<object>();
        }

        public decimal GetTongDoanhThu(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = context.HoaDon.Where(h => h.TrangThai == "Đã thanh toán");

            if (tuNgay.HasValue)
                query = query.Where(h => h.NgayLap >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(h => h.NgayLap <= denNgay.Value);

            return query.Sum(h => (decimal?)h.TongTien) ?? 0;
        }

        public int GetSoHoaDon(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = context.HoaDon.Where(h => h.TrangThai == "Đã thanh toán");

            if (tuNgay.HasValue)
                query = query.Where(h => h.NgayLap >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(h => h.NgayLap <= denNgay.Value);

            return query.Count();
        }

        public List<object> GetAllHoaDonForDisplay()
        {
            return context.HoaDon
                .Select(h => new
                {
                    h.HoaDonID,
                    TenBan = h.BanAn.TenBan,
                    TenKhach = h.NguoiDung.HoTen,
                    h.NgayLap,
                    h.TongTien,
                    h.TrangThai
                })
                .OrderByDescending(h => h.NgayLap)
                .ToList<object>();
        }
    }
}
