using QuanLyNhaHang.Models;
using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Utils;
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
        private static HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private static ChiTietHoaDonDAL chiTietDAL = new ChiTietHoaDonDAL();

        #region Client - Đặt món

        // Kiểm tra khách đã có bàn được đặt chưa (logic mới: không cần duyệt)
        public static BanAn GetBanDaDat(int userId)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                var datBan = DatBanBLL.GetBanDaDuyetByUser(userId);
                return datBan?.BanAn;
            }, null, "Lỗi khi kiểm tra bàn đã đặt");
        }

        // Lấy hóa đơn hiện tại của khách (đã được tạo khi đặt bàn)
        public static HoaDon GetHoaDonHienTai(int userId)
        {
            using (var db = new Model1())
            {
                // Tìm hóa đơn chưa thanh toán của user này
                var hd = db.HoaDon
                    .Include(h => h.BanAn)
                    .Include(h => h.NguoiDung)
                    .FirstOrDefault(h => h.UserID.HasValue && h.UserID.Value == userId && h.TrangThai == "Chưa thanh toán");
                
                return hd;
            }
        }

        // Kiểm tra khách có thể order món không
        public static string KiemTraCoTheOrder(int userId)
        {
            // Lấy bàn đang đặt của user
            var datBan = DatBanBLL.GetBanDaDuyetByUser(userId);
            if (datBan == null)
                return "Bạn chưa có bàn nào đang hoạt động hoặc chờ duyệt!";

            // Cho phép order nếu bàn đang chờ duyệt hoặc đã duyệt
            if (datBan.TrangThai == "Chờ duyệt" || datBan.TrangThai == "Đã duyệt")
                return "OK";

            return $"Bàn {datBan.BanAn?.TenBan ?? ""} hiện không khả dụng để order!";
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

        // Thêm món vào hóa đơn (logic mới: tạo hóa đơn khi order món đầu tiên)
        public static string ThemMonVaoHoaDon(int userId, int monId, int soLuong)
        {
            // Validation đầu vào
            if (userId <= 0)
                return "ID người dùng không hợp lệ!";
            
            if (monId <= 0)
                return "ID món ăn không hợp lệ!";
            
            if (soLuong <= 0)
                return "Số lượng phải lớn hơn 0!";
            
            if (soLuong > 100)
                return "Số lượng không được vượt quá 100!";

            // Kiểm tra có thể order không
            string kiemTra = KiemTraCoTheOrder(userId);
            if (kiemTra != "OK")
                return kiemTra;

            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    // Lấy hoặc tạo hóa đơn hiện tại
                    var hoaDon = GetHoaDonHienTai(userId);
                    if (hoaDon == null)
                    {
                        // Tạo hóa đơn mới khi order món đầu tiên
                        var datBan = db.DatBan
    .Include("BanAn")
    .FirstOrDefault(d => d.UserID.HasValue && d.UserID.Value == userId &&
        (d.TrangThai == "Chờ duyệt" || d.TrangThai == "Đã duyệt"));


                        if (datBan == null)
                            return "Không tìm thấy thông tin đặt bàn!";

                        hoaDon = new HoaDon
                        {
                            BanID = datBan.BanID,
                            UserID = (int?)userId, // Explicit cast to nullable int
                            NgayLap = DateTime.Now,
                            TongTien = 0,
                            TrangThai = "Chưa thanh toán"
                        };
                        db.HoaDon.Add(hoaDon);
                        db.SaveChanges(); // Lưu để có HoaDonID
                    }

                    var cthd = db.ChiTietHoaDon.FirstOrDefault(c => c.HoaDonID == hoaDon.HoaDonID && c.MonID == monId);
                    var mon = db.ThucDon.Find(monId);

                    if (mon == null) 
                        return "Không tìm thấy món ăn!";

                    if (mon.TrangThai != true || mon.TrangThai == null)
                        return "Món ăn hiện không có sẵn!";

                    if (cthd != null)
                    {
                        cthd.SoLuong += soLuong;
                    }
                    else
                    {
                        cthd = new ChiTietHoaDon
                        {
                            HoaDonID = hoaDon.HoaDonID,
                            MonID = monId,
                            SoLuong = soLuong,
                            DonGia = mon.DonGia
                        };
                        db.ChiTietHoaDon.Add(cthd);
                    }

                    // Cập nhật tổng tiền
                    var hd = db.HoaDon.Find(hoaDon.HoaDonID);
                    if (hd != null)
                    {
                        hd.TongTien = db.ChiTietHoaDon
                                        .Where(c => c.HoaDonID == hoaDon.HoaDonID)
                                        .AsEnumerable()
                                        .Sum(c => Convert.ToDecimal(c.SoLuong) * c.DonGia);
                    }

                    db.SaveChanges();
                    return $"Đã thêm {soLuong} {mon.TenMon} vào hóa đơn!";
                }
            }, "Lỗi khi thêm món vào hóa đơn", "Lỗi khi thêm món vào hóa đơn");
        }

        // Thêm món vào hóa đơn (method cũ - giữ lại để tương thích)
        public static void ThemMon(int hoaDonId, int monId, int soLuong)
        {
            using (var db = new Model1())
            {
                var cthd = db.ChiTietHoaDon.FirstOrDefault(c => c.HoaDonID == hoaDonId && c.MonID == monId);
                var mon = db.ThucDon.Find(monId);

                if (mon == null) throw new Exception("Không tìm thấy món ăn!");

                if (cthd != null)
                {
                    cthd.SoLuong += soLuong;
                }
                else
                {
                    cthd = new ChiTietHoaDon
                    {
                        HoaDonID = hoaDonId,
                        MonID = monId,
                        SoLuong = soLuong,
                        DonGia = mon.DonGia
                    };
                    db.ChiTietHoaDon.Add(cthd);
                }

                // Cập nhật tổng tiền
                var hd = db.HoaDon.Find(hoaDonId);
                if (hd != null)
                {
                    hd.TongTien = db.ChiTietHoaDon
                                    .Where(c => c.HoaDonID == hoaDonId)
                                    .AsEnumerable()
                                    .Sum(c => Convert.ToDecimal(c.SoLuong) * c.DonGia);
                }

                db.SaveChanges();
            }
        }

        // Lấy chi tiết hóa đơn hiện tại của user (cho UI giỏ hàng)
        public static List<object> GetChiTietHoaDonHienTai(int userId)
        {
            var hoaDon = GetHoaDonHienTai(userId);
            if (hoaDon == null)
                return new List<object>();

            return GetChiTietHoaDon(hoaDon.HoaDonID);
        }

        // Xóa món khỏi hóa đơn
        public static string XoaMonKhoiHoaDon(int hoaDonId, int monId)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    var cthd = db.ChiTietHoaDon.FirstOrDefault(c => c.HoaDonID == hoaDonId && c.MonID == monId);
                    if (cthd == null)
                        return "Không tìm thấy món trong hóa đơn!";

                    db.ChiTietHoaDon.Remove(cthd);

                    // Cập nhật tổng tiền
                    var hd = db.HoaDon.Find(hoaDonId);
                    if (hd != null)
                    {
                        hd.TongTien = db.ChiTietHoaDon
                                        .Where(c => c.HoaDonID == hoaDonId)
                                        .AsEnumerable()
                                        .Sum(c => Convert.ToDecimal(c.SoLuong) * c.DonGia);
                    }

                    db.SaveChanges();
                    return "Xóa món khỏi hóa đơn thành công!";
                }
            }, "Lỗi khi xóa món khỏi hóa đơn", "Lỗi khi xóa món khỏi hóa đơn");
        }

        // Lấy chi tiết hóa đơn (cho UI giỏ hàng hoặc Admin)
        public static List<object> GetChiTietHoaDon(int hoaDonId)
        {
            using (var db = new Model1())
            {
                var query = db.ChiTietHoaDon
                              .Where(c => c.HoaDonID == hoaDonId)
                              .AsEnumerable() // ⚡ Lấy dữ liệu về trước khi tính toán
                              .Select(c => new
                              {
                                  MonID = c.MonID, // Thêm MonID để UI có thể sử dụng
                                  TenMon = c.ThucDon.TenMon,
                                  c.SoLuong,
                                  c.DonGia,
                                  ThanhTien = c.SoLuong * c.DonGia
                              })
                              .ToList<object>();
                return query;
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

                // 🆕 Đăng ký font Unicode hỗ trợ tiếng Việt (Arial, Tahoma...)
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font titleFont = new Font(bf, 18, Font.BOLD);
                Font textFont = new Font(bf, 12, Font.NORMAL);

                // 🔹 Tiêu đề
                Paragraph title = new Paragraph("HÓA ĐƠN THANH TOÁN", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph("\n"));

                // 🔹 Thông tin hóa đơn
                doc.Add(new Paragraph($"Mã hóa đơn: {hd.HoaDonID}", textFont));
                doc.Add(new Paragraph($"Ngày: {hd.NgayLap:dd/MM/yyyy HH:mm}", textFont));
                doc.Add(new Paragraph($"Bàn: {hd.BanAn?.TenBan}", textFont));
                doc.Add(new Paragraph($"Khách hàng: {hd.NguoiDung?.HoTen}", textFont));
                doc.Add(new Paragraph("\n"));

                // 🔹 Bảng chi tiết
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                float[] widths = new float[] { 40f, 15f, 20f, 25f };
                table.SetWidths(widths);

                // Header
                string[] headers = { "Tên món", "Số lượng", "Đơn giá", "Thành tiền" };
                foreach (var h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, textFont))
                    {
                        BackgroundColor = new iTextSharp.text.BaseColor(230, 230, 250),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                // Dữ liệu món ăn
                decimal tongTien = 0;
                foreach (var item in chiTiet)
                {
                    decimal thanhTien = Convert.ToDecimal(item.SoLuong) * item.DonGia;

                    table.AddCell(new PdfPCell(new Phrase(item.TenMon, textFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.SoLuong.ToString(), textFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.DonGia.ToString("N0"), textFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(thanhTien.ToString("N0"), textFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    tongTien += thanhTien;
                }

                doc.Add(table);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph($"TỔNG CỘNG: {tongTien:N0} VNĐ", titleFont));

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

