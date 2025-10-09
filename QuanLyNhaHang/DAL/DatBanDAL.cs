using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class DatBanDAL
    {
        private Model1 context = new Model1();

        public List<DatBan> GetAll()
        {
            return context.DatBan
                .Include("BanAn")
                .Include("NguoiDung")
                .OrderByDescending(d => d.NgayDat)
                .ToList();
        }

        public DatBan GetById(int id)
        {
            return context.DatBan
                .Include("BanAn")
                .Include("NguoiDung")
                .FirstOrDefault(d => d.DatBanID == id);
        }

        public List<DatBan> GetByUserId(int userId)
        {
            return context.DatBan
                .Include("BanAn")
                .Include("NguoiDung")
                .Where(d => d.UserID.HasValue && d.UserID.Value == userId)
                .OrderByDescending(d => d.NgayDat)
                .ToList();
        }

        public List<DatBan> GetByTrangThai(string trangThai)
        {
            return context.DatBan
                .Include("BanAn")
                .Include("NguoiDung")
                .Where(d => d.TrangThai == trangThai)
                .OrderByDescending(d => d.NgayDat)
                .ToList();
        }

        public List<DatBan> GetChoDuyet()
        {
            return GetByTrangThai("Chờ duyệt");
        }

        public List<DatBan> GetDaDuyet()
        {
            return GetByTrangThai("Đã duyệt");
        }

        public List<DatBan> GetDaHuy()
        {
            return GetByTrangThai("Đã hủy");
        }



        // File: DatBanDAL.cs

        public string Add(int banId, int userId, DateTime ngayDat, string trangThai = "Chờ duyệt")
        {
            var ban = context.BanAn.Find(banId);
            if (ban == null)
                return "Không tìm thấy bàn!";

            if (ban.TrangThai != "Trống")
                return $"Bàn {ban.TenBan} hiện đang {ban.TrangThai.ToLower()}!";
            var existingActiveDatBan = context.DatBan
                .Include("BanAn") 
                .FirstOrDefault(d =>
                    d.UserID.HasValue && d.UserID.Value == userId &&
                    (
                        // ✨ SỬA LỖI 2: Sửa lại trạng thái kiểm tra cho đúng logic
                        (d.TrangThai == "Chờ duyệt" && d.BanAn.TrangThai == "Đặt trước") ||
                        (d.TrangThai == "Đã duyệt" && d.BanAn.TrangThai == "Đang dùng")
                    )
                );

            if (existingActiveDatBan != null)
            {
                return "Bạn đã đặt bàn rồi! Vui lòng hoàn tất hoặc hủy đơn cũ trước khi đặt bàn mới.";
            }
            // --- KẾT THÚC SỬA LỖI ---

            var datBan = new DatBan
            {
                BanID = banId,
                UserID = userId,
                NgayDat = ngayDat,
                TrangThai = trangThai
            };

            context.DatBan.Add(datBan);
            ban.TrangThai = "Đặt trước";
            context.SaveChanges();

            return $"Đặt bàn {ban.TenBan} thành công!";
        }
        public string UpdateTrangThai(int id, string trangThai)
        {
            var datBan = context.DatBan.Find(id);
            if (datBan == null)
                return "Không tìm thấy yêu cầu đặt bàn!";

            var ban = context.BanAn.Find(datBan.BanID);
            if (ban == null)
                return "Không tìm thấy bàn!";

            string oldTrangThai = datBan.TrangThai;
            datBan.TrangThai = trangThai;

            // Cập nhật trạng thái bàn theo trạng thái đặt bàn
            switch (trangThai)
            {
                case "Đã duyệt":
                    ban.TrangThai = "Đang dùng";
                    break;
                case "Đã hủy":
                    ban.TrangThai = "Trống";
                    break;
                case "Đặt trước":
                    ban.TrangThai = "Đặt trước";
                    break;
            }

            context.SaveChanges();
            return $"Cập nhật trạng thái thành công! ({oldTrangThai} → {trangThai})";
        }

        public string DuyetDatBan(int id)
        {
            return UpdateTrangThai(id, "Đã duyệt");
        }

        public string HuyDatBan(int id)
        {
            // Khi hủy đặt bàn: hủy yêu cầu và xóa hóa đơn chưa thanh toán liên quan
            var datBan = context.DatBan.Find(id);
            if (datBan == null)
                return "Không tìm thấy yêu cầu đặt bàn!";

            // Tìm và xóa hóa đơn chưa thanh toán gắn với bàn và user này
            var hoaDon = context.HoaDon.FirstOrDefault(h => 
                h.BanID.HasValue && datBan.BanID.HasValue && h.BanID.Value == datBan.BanID.Value && 
                h.UserID.HasValue && datBan.UserID.HasValue && h.UserID.Value == datBan.UserID.Value && 
                h.TrangThai == "Chưa thanh toán");
            if (hoaDon != null)
            {
                // Xóa chi tiết trước
                context.ChiTietHoaDon.RemoveRange(hoaDon.ChiTietHoaDon);
                context.HoaDon.Remove(hoaDon);
            }

            var result = UpdateTrangThai(id, "Đã hủy");
            return result;
        }
        
        public string Delete(int id)
        {
            var datBan = context.DatBan.Find(id);
            if (datBan == null)
                return "Không tìm thấy yêu cầu đặt bàn!";

            // Chỉ cho phép xóa yêu cầu đã hủy
            if (datBan.TrangThai != "Đã hủy")
                return "Chỉ có thể xóa yêu cầu đã hủy!";

            context.DatBan.Remove(datBan);
            context.SaveChanges();
            return "Xóa yêu cầu đặt bàn thành công!";
        }

        public bool IsUserCoBanDaDuyet(int userId)
        {
            // Cho phép order nếu user đã có bàn Chờ duyệt, Đã duyệt, hoặc Đang dùng
            return context.DatBan.Any(d =>
                d.UserID.HasValue && d.UserID.Value == userId &&
                (d.TrangThai == "Chờ duyệt" || d.TrangThai == "Đã duyệt" || d.TrangThai == "Đang dùng"));
        }

        public DatBan GetBanDaDuyetByUser(int userId)
        {
            return context.DatBan
                .Include("BanAn")
                .FirstOrDefault(d =>
                    d.UserID.HasValue && d.UserID.Value == userId &&
                    (d.TrangThai == "Chờ duyệt" || d.TrangThai == "Đã duyệt"));
        }

        public List<object> GetDanhSachDatBan()
        {
            return context.DatBan
                .Include("BanAn")
                .Include("NguoiDung")
                .Select(d => new
                {
                    d.DatBanID,
                    TenBan = d.BanAn != null ? d.BanAn.TenBan : "N/A",
                    NguoiDat = d.NguoiDung != null ? d.NguoiDung.HoTen : "N/A",
                    d.NgayDat,
                    TrangThaiYeuCau = d.TrangThai,
                    TrangThaiBan = d.BanAn != null ? d.BanAn.TrangThai : "N/A"
                })
                .OrderByDescending(d => d.NgayDat)
                .ToList<object>();
        }
        // Phương thức mới để lấy lịch sử đặt bàn cho Client UI
        public List<object> GetLichSuDatBanForDisplay(int userId)
        {
            return context.DatBan
                .Where(d => d.UserID.HasValue && d.UserID.Value == userId)
                .OrderByDescending(d => d.NgayDat)
                .Select(d => new
                {
                    d.DatBanID,
                    // Lấy Tên Bàn từ đối tượng liên quan và kiểm tra null
                    TenBan = d.BanAn != null ? d.BanAn.TenBan : "N/A",
                    d.NgayDat,
                    d.TrangThai
                })
                .ToList<object>();
        }
        // Method mới: Đặt bàn (không tạo hóa đơn ngay, chỉ tạo khi order món)
        public string DatBanVaTaoHoaDon(int banId, int userId, DateTime ngayDat)
        {
            // Kiểm tra bàn có trống không
            var ban = context.BanAn.Find(banId);
            if (ban == null)
                return "Không tìm thấy bàn!";

            if (ban.TrangThai != "Trống")
                return "Bàn hiện không trống!";

            // Kiểm tra user đã có đặt bàn chưa
            var existingDatBan = context.DatBan.FirstOrDefault(d => d.UserID.HasValue && d.UserID.Value == userId && 
                (d.TrangThai == "Đặt trước" || d.TrangThai == "Đang dùng"));
            if (existingDatBan != null)
                return "Bạn đã có đặt bàn đang hoạt động!";

            // Tạo đặt bàn với trạng thái "Đặt trước"
            var datBan = new DatBan
            {
                BanID = (int?)banId,  // Explicit cast to nullable int
                UserID = (int?)userId, // Explicit cast to nullable int
                NgayDat = (DateTime?)ngayDat, // Explicit cast to nullable DateTime
                TrangThai = "Đặt trước"
            };

            context.DatBan.Add(datBan);
            
            // Cập nhật trạng thái bàn thành "Đặt trước"
            ban.TrangThai = "Đặt trước";
            
            context.SaveChanges();
            
            return "Đặt bàn thành công! Bạn có thể bắt đầu gọi món ngay.";
        }
    }
}
