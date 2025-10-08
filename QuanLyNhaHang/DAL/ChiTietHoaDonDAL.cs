using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace QuanLyNhaHang.DAL
{
    public class ChiTietHoaDonDAL
    {
        private Model1 context = new Model1();

        public List<ChiTietHoaDon> GetAll()
        {
            return context.ChiTietHoaDon
                .Include("HoaDon")
                .Include("ThucDon")
                .ToList();
        }

        public List<ChiTietHoaDon> GetByHoaDonId(int hoaDonId)
        {
            return context.ChiTietHoaDon
                .Include("ThucDon")
                .Where(c => c.HoaDonID == hoaDonId)
                .ToList();
        }

        public ChiTietHoaDon GetById(int id)
        {
            return context.ChiTietHoaDon
                .Include("HoaDon")
                .Include("ThucDon")
                .FirstOrDefault(c => c.CTHD_ID == id);
        }

        public ChiTietHoaDon GetByHoaDonAndMon(int hoaDonId, int monId)
        {
            return context.ChiTietHoaDon
                .FirstOrDefault(c => c.HoaDonID == hoaDonId && c.MonID == monId);
        }

        public string Add(int hoaDonId, int monId, int soLuong, decimal donGia)
        {
            // Kiểm tra hóa đơn có tồn tại không
            var hoaDon = context.HoaDon.Find(hoaDonId);
            if (hoaDon == null)
                return "Không tìm thấy hóa đơn!";

            // Kiểm tra món ăn có tồn tại không
            var mon = context.ThucDon.Find(monId);
            if (mon == null)
                return "Không tìm thấy món ăn!";

            // Kiểm tra món có đang bán không
            if (mon.TrangThai != true)
                return "Món ăn hiện không bán!";

            // Kiểm tra đã có món này trong hóa đơn chưa
            var existing = GetByHoaDonAndMon(hoaDonId, monId);
            if (existing != null)
            {
                // Cập nhật số lượng
                existing.SoLuong += soLuong;
                context.SaveChanges();
                return "Cập nhật số lượng món thành công!";
            }
            else
            {
                // Thêm mới
                var chiTiet = new ChiTietHoaDon
                {
                    HoaDonID = hoaDonId,
                    MonID = monId,
                    SoLuong = soLuong,
                    DonGia = donGia
                };

                context.ChiTietHoaDon.Add(chiTiet);
                context.SaveChanges();
                return "Thêm món vào hóa đơn thành công!";
            }
        }

        public string UpdateSoLuong(int id, int soLuong)
        {
            var chiTiet = context.ChiTietHoaDon.Find(id);
            if (chiTiet == null)
                return "Không tìm thấy chi tiết hóa đơn!";

            if (soLuong <= 0)
                return "Số lượng phải lớn hơn 0!";

            chiTiet.SoLuong = soLuong;
            context.SaveChanges();
            return "Cập nhật số lượng thành công!";
        }

        public string UpdateDonGia(int id, decimal donGia)
        {
            var chiTiet = context.ChiTietHoaDon.Find(id);
            if (chiTiet == null)
                return "Không tìm thấy chi tiết hóa đơn!";

            if (donGia <= 0)
                return "Đơn giá phải lớn hơn 0!";

            chiTiet.DonGia = donGia;
            context.SaveChanges();
            return "Cập nhật đơn giá thành công!";
        }

        public string Delete(int id)
        {
            var chiTiet = context.ChiTietHoaDon.Find(id);
            if (chiTiet == null)
                return "Không tìm thấy chi tiết hóa đơn!";

            context.ChiTietHoaDon.Remove(chiTiet);
            context.SaveChanges();
            return "Xóa món khỏi hóa đơn thành công!";
        }

        public string DeleteByHoaDonAndMon(int hoaDonId, int monId)
        {
            var chiTiet = GetByHoaDonAndMon(hoaDonId, monId);
            if (chiTiet == null)
                return "Không tìm thấy món trong hóa đơn!";

            return Delete(chiTiet.CTHD_ID);
        }

        public decimal GetTongTienHoaDon(int hoaDonId)
        {
            return context.ChiTietHoaDon
                .Where(c => c.HoaDonID == hoaDonId)
                .Sum(c => c.SoLuong * c.DonGia);
        }

        public int GetTongSoLuongHoaDon(int hoaDonId)
        {
            return context.ChiTietHoaDon
                .Where(c => c.HoaDonID == hoaDonId)
                .Sum(c => c.SoLuong);
        }

        public List<object> GetChiTietHoaDonForDisplay(int hoaDonId)
        {
            return context.ChiTietHoaDon
                .Where(c => c.HoaDonID == hoaDonId)
                .Select(c => new
                {
                    TenMon = c.ThucDon.TenMon,
                    c.SoLuong,
                    c.DonGia,
                    ThanhTien = c.SoLuong * c.DonGia
                })
                .ToList<object>();
        }

        public List<object> GetTopMonAnBanChay(int top = 10, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = context.ChiTietHoaDon
                .Include("HoaDon")
                .Include("ThucDon")
                .Where(c => c.HoaDon.TrangThai == "Đã thanh toán");

            if (tuNgay.HasValue)
                query = query.Where(c => c.HoaDon.NgayLap >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(c => c.HoaDon.NgayLap <= denNgay.Value);

            return query
                .GroupBy(c => new { c.MonID, c.ThucDon.TenMon })
                .Select(g => new
                {
                    MonID = g.Key.MonID,
                    TenMon = g.Key.TenMon,
                    TongSoLuong = g.Sum(x => x.SoLuong),
                    TongTien = g.Sum(x => x.SoLuong * x.DonGia)
                })
                .OrderByDescending(x => x.TongSoLuong)
                .Take(top)
                .ToList<object>();
        }

        public bool HasChiTietHoaDon(int hoaDonId)
        {
            return context.ChiTietHoaDon.Any(c => c.HoaDonID == hoaDonId);
        }

        public int GetSoMonTrongHoaDon(int hoaDonId)
        {
            return context.ChiTietHoaDon.Count(c => c.HoaDonID == hoaDonId);
        }
    }
}
