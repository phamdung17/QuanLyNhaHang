using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity; // for Include
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace QuanLyNhaHang.BLL
{
    public class HoaDonBLL
    {
        #region Client - Đặt món

        // Kiểm tra khách đã có bàn được duyệt chưa
        public static BanAn GetBanDaDuyet(int userId)
        {
            using (var db = new Model1())
            {
                var datBan = db.DatBan
                               .FirstOrDefault(d => d.UserID == userId && d.TrangThai == "Đã duyệt");
                return datBan != null ? db.BanAn.Find(datBan.BanID) : null;
            }
        }

        // Lấy hoặc tạo hóa đơn mới cho khách
        public static HoaDon GetOrCreateHoaDon(int banId, int userId)
        {
            using (var db = new Model1())
            {
                var hd = db.HoaDon.FirstOrDefault(h => h.BanID == banId && h.TrangThai == "Chưa thanh toán");
                if (hd == null)
                {
                    hd = new HoaDon
                    {
                        BanID = banId,
                        UserID = userId,
                        NgayLap = DateTime.Now,
                        TongTien = 0,
                        TrangThai = "Chưa thanh toán"
                    };
                    db.HoaDon.Add(hd);
                    db.SaveChanges();
                }
                return hd;
            }
        }

        public static HoaDon GetById(int hoaDonId)
        {
            using (var db = new Model1())
            {
                return db.HoaDon
                         .Include(h => h.BanAn)
                         .Include(h => h.NguoiDung)
                         .FirstOrDefault(h => h.HoaDonID == hoaDonId);
            }
        }

        // Thêm món vào hóa đơn
        public static void ThemMon(int hoaDonId, int monId, int soLuong)
        {
            using (var db = new Model1())
            {
                var cthd = db.ChiTietHoaDon.FirstOrDefault(c => c.HoaDonID == hoaDonId && c.MonID == monId);
                var mon = db.ThucDon.Find(monId);

                if (mon == null) throw new Exception("Không tìm thấy món ăn!");

                if (cthd != null)
                {
                    // SoLuong đã là int non-nullable
                    cthd.SoLuong += soLuong;
                }
                else
                {
                    cthd = new ChiTietHoaDon
                    {
                        HoaDonID = hoaDonId,
                        MonID = monId,
                        SoLuong = soLuong,
                        DonGia = mon.DonGia // DonGia là decimal non-nullable trong model ThucDon
                    };
                    db.ChiTietHoaDon.Add(cthd);
                }

                // Cập nhật tổng tiền (ép kiểu int -> decimal trước khi nhân)
                var hd = db.HoaDon.Find(hoaDonId);
                if (hd != null)
                {
                    // Lưu ý: Convert.ToDecimal để chắc chắn biểu thức là decimal
                    hd.TongTien = db.ChiTietHoaDon
                                    .Where(c => c.HoaDonID == hoaDonId)
                                    .AsEnumerable() // materialize để dùng Convert.ToDecimal an toàn
                                    .Sum(c => Convert.ToDecimal(c.SoLuong) * c.DonGia);
                }

                db.SaveChanges();
            }
        }

        // Lấy chi tiết hóa đơn (cho UI giỏ hàng hoặc Admin)
        public static List<object> GetChiTietHoaDon(int hoaDonId)
        {
            using (var db = new Model1())
            {
                // SoLuong và DonGia là non-nullable => không cần ??.
                return db.ChiTietHoaDon
                         .Where(c => c.HoaDonID == hoaDonId)
                         .Select(c => new
                         {
                             TenMon = c.ThucDon.TenMon,
                             SoLuong = c.SoLuong,
                             DonGia = c.DonGia,
                             ThanhTien = Convert.ToDecimal(c.SoLuong) * c.DonGia
                         })
                         .ToList<object>();
            }
        }

        #endregion

        #region Admin - Quản lý hóa đơn

        // Lấy tất cả hóa đơn (Admin)
        public static List<object> GetAllHoaDon()
        {
            using (var db = new Model1())
            {
                return db.HoaDon
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

        // Thanh toán (Admin hoặc khi in hóa đơn)
        public static void ThanhToan(int hoaDonId)
        {
            using (var db = new Model1())
            {
                var hd = db.HoaDon.Find(hoaDonId);
                if (hd != null)
                {
                    hd.TrangThai = "Đã thanh toán";

                    var ban = db.BanAn.Find(hd.BanID);
                    if (ban != null)
                        ban.TrangThai = "Trống";

                    db.SaveChanges();
                }
            }
        }

        // Xóa hóa đơn
        public static void Delete(int hoaDonId)
        {
            using (var db = new Model1())
            {
                var hd = db.HoaDon.Find(hoaDonId);
                if (hd != null)
                {
                    db.ChiTietHoaDon.RemoveRange(hd.ChiTietHoaDon); // xóa chi tiết
                    db.HoaDon.Remove(hd);
                    db.SaveChanges();
                }
            }
        }

        // Xuất PDF hóa đơn
        public static string ExportHoaDonToPdf(int hoaDonId, string filePath)
        {
            using (var db = new Model1())
            {
                var hd = db.HoaDon
                           .Include(h => h.BanAn)
                           .Include(h => h.NguoiDung)
                           .FirstOrDefault(h => h.HoaDonID == hoaDonId);

                if (hd == null) return "Không tìm thấy hóa đơn!";

                var chiTiet = db.ChiTietHoaDon
                    .Where(c => c.HoaDonID == hoaDonId)
                    .Select(c => new
                    {
                        TenMon = c.ThucDon.TenMon,
                        SoLuong = c.SoLuong,
                        DonGia = c.DonGia
                    })
                    .ToList();

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                var titleFont = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD);
                var textFont = FontFactory.GetFont("Arial", 12);

                doc.Add(new Paragraph("HÓA ĐƠN THANH TOÁN", titleFont));
                doc.Add(new Paragraph($"Mã hóa đơn: {hd.HoaDonID}", textFont));
                doc.Add(new Paragraph($"Ngày: {hd.NgayLap:dd/MM/yyyy HH:mm}", textFont));
                doc.Add(new Paragraph($"Bàn: {hd.BanAn?.TenBan}", textFont));
                doc.Add(new Paragraph($"Khách: {hd.NguoiDung?.HoTen}", textFont));
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.AddCell("Tên món");
                table.AddCell("Số lượng");
                table.AddCell("Đơn giá");
                table.AddCell("Thành tiền");

                decimal tongTien = 0;
                foreach (var item in chiTiet)
                {
                    // item.SoLuong là int, item.DonGia là decimal
                    decimal thanhTien = Convert.ToDecimal(item.SoLuong) * item.DonGia;

                    table.AddCell(item.TenMon);
                    table.AddCell(item.SoLuong.ToString());
                    table.AddCell(item.DonGia.ToString("N0"));
                    table.AddCell(thanhTien.ToString("N0"));

                    tongTien += thanhTien;
                }

                doc.Add(table);
                doc.Add(new Paragraph($"\nTỔNG CỘNG: {tongTien:N0} VNĐ", titleFont));

                doc.Close();
                return "Xuất hóa đơn thành công!";
            }
        }

        // xác định trạng thái của hóa đơn
        public static bool XacNhanThanhToan(int hoaDonId)
        {
            using (var db = new Model1())
            {
                var hd = db.HoaDon.FirstOrDefault(h => h.HoaDonID == hoaDonId);

                if (hd == null)
                    return false;

                if (hd.TrangThai == "Đã thanh toán")
                    return false; // tránh xác nhận trùng

                hd.TrangThai = "Đã thanh toán";
                hd.NgayLap = DateTime.Now; // cập nhật thời gian thanh toán

                // cập nhật trạng thái bàn về "Trống"
                var ban = db.BanAn.FirstOrDefault(b => b.BanID == hd.BanID);
                if (ban != null)
                    ban.TrangThai = "Trống";

                db.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}
