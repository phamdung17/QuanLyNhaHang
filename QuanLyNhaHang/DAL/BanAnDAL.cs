using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace QuanLyNhaHang.DAL
{
    public class BanAnDAL
    {
        
        public List<BanAn> GetAll()
        {
            // ✨ Bổ sung using để luôn tạo context mới, đảm bảo dữ liệu mới nhất
            using (var context = new Model1())
            {
                return context.BanAn.OrderBy(b => b.BanID).ToList();
            }
        }

        public BanAn GetById(int id)
        {
            using (var context = new Model1())
            {
                return context.BanAn.Find(id);
            }
        }

        public List<BanAn> GetBanTrong()
        {
            using (var context = new Model1())
            {
                return context.BanAn.Where(b => b.TrangThai == "Trống").ToList();
            }
        }

        public List<BanAn> GetBanDangDung()
        {
            using (var context = new Model1())
            {
                return context.BanAn.Where(b => b.TrangThai == "Đang dùng").ToList();
            }
        }

        public List<BanAn> GetBanDatTruoc()
        {
            using (var context = new Model1())
            {
                return context.BanAn.Where(b => b.TrangThai == "Đặt trước").ToList();
            }
        }

        public string Add(string tenBan, string trangThai = "Trống")
        {
            using (var context = new Model1())
            {
                if (string.IsNullOrWhiteSpace(tenBan))
                    return "Tên bàn không được để trống!";

                if (context.BanAn.Any(b => b.TenBan == tenBan))
                    return "Tên bàn đã tồn tại!";

                var ban = new BanAn { TenBan = tenBan, TrangThai = trangThai };
                context.BanAn.Add(ban);
                context.SaveChanges();
                return "Thêm bàn thành công!";
            }
        }

        public string Update(int id, string tenBan, string trangThai)
        {
            using (var context = new Model1())
            {
                var ban = context.BanAn.Find(id);
                if (ban == null) 
                    return "Không tìm thấy bàn!";

                if (string.IsNullOrWhiteSpace(tenBan))
                    return "Tên bàn không được để trống!";

                if (context.BanAn.Any(b => b.TenBan == tenBan && b.BanID != id))
                    return "Tên bàn đã tồn tại!";

                ban.TenBan = tenBan;
                ban.TrangThai = trangThai;
                context.SaveChanges();
                return "Cập nhật bàn thành công!";
            }
        }

        public string UpdateTrangThai(int id, string trangThai)
        {
            using (var context = new Model1())
            {
                var ban = context.BanAn.Find(id);
                if (ban == null) 
                    return "Không tìm thấy bàn!";

                ban.TrangThai = trangThai;
                context.SaveChanges();
                return "Cập nhật trạng thái thành công!";
            }
        }

        public string Delete(int id)
        {
            using (var context = new Model1())
            {
                var ban = context.BanAn.Find(id);
                if (ban == null)
                    return "Không tìm thấy bàn!";

                // Kiểm tra các ràng buộc trước khi xóa
                if (ban.TrangThai == "Đang dùng" || ban.TrangThai == "Đặt trước")
                    return $"Không thể xóa bàn đang ở trạng thái '{ban.TrangThai}'!";

                if (context.HoaDon.Any(h => h.BanID == id && h.TrangThai == "Chưa thanh toán"))
                    return "Không thể xóa bàn vì còn hóa đơn chưa thanh toán!";

                if (context.DatBan.Any(d => d.BanID == id && d.TrangThai != "Đã hủy"))
                    return "Không thể xóa bàn vì còn yêu cầu đặt bàn đang hoạt động!";
                
                context.BanAn.Remove(ban);
                context.SaveChanges();
                return "Xóa bàn thành công!";
            }
        }

        public bool IsBanTrong(int banId)
        {
            using (var context = new Model1())
            {
                var ban = context.BanAn.Find(banId);
                return ban != null && ban.TrangThai == "Trống";
            }
        }
        public bool IsBanDangDung(int banId)
        {
            using (var context = new Model1())
            {
                var ban = context.BanAn.Find(banId);
                return ban != null && ban.TrangThai == "Đang dùng";
            }
        }
        
    }
}