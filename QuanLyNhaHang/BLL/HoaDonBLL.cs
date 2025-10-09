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
            try
            {
                using (var context = new Model1())
                {
                    bool hasActiveTable = context.DatBan
                        .Any(d =>
                            d.UserID == userId &&
                            (
                                (d.TrangThai == "Chờ duyệt" && d.BanAn.TrangThai == "Đặt trước") ||
                                (d.TrangThai == "Đã duyệt" && d.BanAn.TrangThai == "Đang dùng")
                            )
                        );

                    if (hasActiveTable)
                    {
                       
                        return "OK";
                    }
                    else
                    {
                        
                        return "Bạn cần đặt bàn trước khi gọi món. Bàn của bạn có thể đã được thanh toán và giải phóng.";
                    }
                }
            }
            catch (Exception ex)
            {
                
                return "Lỗi hệ thống khi kiểm tra trạng thái bàn.";
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
        // ✨ Thêm món vào hóa đơn, sử dụng Transaction để đảm bảo an toàn
        public static string ThemMonVaoHoaDon(int userId, int monId, int soLuong)
        {
            if (userId <= 0 || monId <= 0 || soLuong <= 0) return "Thông tin không hợp lệ!";

            string kiemTra = KiemTraCoTheOrder(userId);
            if (kiemTra != "OK") return kiemTra;

            try
            {
                using (var db = new Model1())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        var hoaDon = db.HoaDon.FirstOrDefault(h => h.UserID == userId && h.TrangThai == "Chưa thanh toán");
                        if (hoaDon == null)
                        {
                            var datBan = db.DatBan.FirstOrDefault(d => d.UserID == userId && (d.TrangThai == "Chờ duyệt" || d.TrangThai == "Đã duyệt"));
                            if (datBan == null)
                            {
                                transaction.Rollback();
                                return "Không tìm thấy bàn đã đặt hoặc bàn chưa được duyệt!";
                            }

                            hoaDon = new HoaDon { BanID = datBan.BanID, UserID = userId, NgayLap = DateTime.Now, TrangThai = "Chưa thanh toán" };
                            db.HoaDon.Add(hoaDon);
                            db.SaveChanges();
                        }

                        var mon = db.ThucDon.Find(monId);
                        if (mon == null || mon.TrangThai != true)
                        {
                            transaction.Rollback();
                            return "Món ăn không có sẵn!";
                        }

                        var chiTiet = db.ChiTietHoaDon.FirstOrDefault(c => c.HoaDonID == hoaDon.HoaDonID && c.MonID == monId);
                        if (chiTiet != null)
                        {
                            chiTiet.SoLuong += soLuong;
                        }
                        else
                        {
                            db.ChiTietHoaDon.Add(new ChiTietHoaDon { HoaDonID = hoaDon.HoaDonID, MonID = monId, SoLuong = soLuong, DonGia = mon.DonGia });
                        }
                        db.SaveChanges();

                        var danhSachMon = db.ChiTietHoaDon.Where(c => c.HoaDonID == hoaDon.HoaDonID).ToList();
                        hoaDon.TongTien = danhSachMon.Sum(c => c.SoLuong * c.DonGia);
                        db.SaveChanges();

                        transaction.Commit();
                        return $"Đã thêm {soLuong} {mon.TenMon} vào hóa đơn!";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi thêm món vào hóa đơn");
                return "Lỗi hệ thống khi thêm món vào hóa đơn.";
            }
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


        // ✨ Xác nhận thanh toán, kiểm tra trạng thái nghiêm ngặt
        public static bool XacNhanThanhToan(int hoaDonId)
        {
            try
            {
                using (var db = new Model1())
                {
                    // Tìm hóa đơn cần thanh toán
                    var hd = db.HoaDon.FirstOrDefault(h => h.HoaDonID == hoaDonId);

                    // Kiểm tra nếu hóa đơn không tồn tại hoặc đã thanh toán rồi
                    if (hd == null)
                    {
                        ExceptionHelper.ShowWarningMessage("Không tìm thấy hóa đơn!");
                        return false;
                    }

                    if (hd.TrangThai == "Đã thanh toán")
                    {
                        ExceptionHelper.ShowWarningMessage("Hóa đơn này đã được thanh toán trước đó!");
                        return false;
                    }

                    // --- LOGIC KIỂM TRA TRẠNG THÁI ĐẶT BÀN ---
                    // Tìm yêu cầu đặt bàn gần nhất liên quan đến hóa đơn này (dựa trên UserID và BanID)
                    var datBan = db.DatBan
                        .Where(d => d.UserID == hd.UserID && d.BanID == hd.BanID && d.TrangThai != "Đã hủy")
                        .OrderByDescending(d => d.NgayDat)
                        .FirstOrDefault();

                    // Nếu tìm thấy yêu cầu đặt bàn và nó đang ở trạng thái "Chờ duyệt"
                    if (datBan != null && datBan.TrangThai == "Chờ duyệt")
                    {
                        // Hiển thị thông báo và không cho thanh toán
                        ExceptionHelper.ShowWarningMessage("Đơn chưa được duyệt bởi admin, yêu cầu duyệt trước khi thanh toán!");
                        return false;
                    }
                    // --- KẾT THÚC LOGIC MỚI ---

                    // Nếu đã được duyệt (hoặc không có đơn đặt bàn liên quan), tiến hành thanh toán
                    hd.TrangThai = "Đã thanh toán";
                    hd.NgayLap = DateTime.Now; // Cập nhật lại thời gian thành thời gian thanh toán

                    // Cập nhật trạng thái bàn về "Trống"
                    var ban = db.BanAn.FirstOrDefault(b => b.BanID == hd.BanID);
                    if (ban != null)
                    {
                        ban.TrangThai = "Trống";
                    }

                    db.SaveChanges(); // Lưu tất cả các thay đổi vào cơ sở dữ liệu
                    return true; // Trả về true để báo hiệu thanh toán thành công
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi xác nhận thanh toán");
                return false; // Trả về false nếu có lỗi hệ thống
            }
        }
        public static List<object> GetLichSuHoaDonByUser(int userId)
        {
            return ExceptionHelper.SafeExecute(() => hoaDonDAL.GetLichSuHoaDonForDisplay(userId), new List<object>(), "Lỗi khi lấy lịch sử hóa đơn.");
        }

        #endregion
    }
}

