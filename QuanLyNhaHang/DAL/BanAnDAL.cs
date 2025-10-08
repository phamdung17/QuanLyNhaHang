using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class BanAnDAL
    {
        private Model1 context = new Model1();

        public List<BanAn> GetAll()
        {
            return context.BanAn.ToList();
        }

        public BanAn GetById(int id)
        {
            return context.BanAn.Find(id);
        }

        public List<BanAn> GetBanTrong()
        {
            return context.BanAn.Where(b => b.TrangThai == "Trống").ToList();
        }

        public List<BanAn> GetBanDangDung()
        {
            return context.BanAn.Where(b => b.TrangThai == "Đang Dùng").ToList();
        }

        public List<BanAn> GetBanDatTruoc()
        {
            return context.BanAn.Where(b => b.TrangThai == "Đặt trước").ToList();
        }

        public string Add(string tenBan, string trangThai = "Trống")
        {
            if (string.IsNullOrWhiteSpace(tenBan))
                return "Tên bàn không được để trống!";

            if (context.BanAn.Any(b => b.TenBan == tenBan))
                return "Tên bàn đã tồn tại!";

            var ban = new BanAn
            {
                TenBan = tenBan,
                TrangThai = trangThai
            };

            context.BanAn.Add(ban);
            context.SaveChanges();
            return "Thêm bàn thành công!";
        }

        public string Update(int id, string tenBan, string trangThai)
        {
            var ban = context.BanAn.Find(id);
            if (ban == null)
                return "Không tìm thấy bàn!";

            if (string.IsNullOrWhiteSpace(tenBan))
                return "Tên bàn không được để trống!";

            // Kiểm tra tên bàn trùng (trừ bàn hiện tại)
            if (context.BanAn.Any(b => b.TenBan == tenBan && b.BanID != id))
                return "Tên bàn đã tồn tại!";

            ban.TenBan = tenBan;
            ban.TrangThai = trangThai;
            context.SaveChanges();
            return "Cập nhật bàn thành công!";
        }

        public string UpdateTrangThai(int id, string trangThai)
        {
            var ban = context.BanAn.Find(id);
            if (ban == null)
                return "Không tìm thấy bàn!";

            ban.TrangThai = trangThai;
            context.SaveChanges();
            return "Cập nhật trạng thái thành công!";
        }

        public string Delete(int id)
        {
            var ban = context.BanAn.Find(id);
            if (ban == null)
                return "Không tìm thấy bàn!";

            // Kiểm tra bàn có đang được sử dụng không
            if (ban.TrangThai == "Đang Dùng")
                return "Không thể xóa bàn đang được sử dụng!";

            // Kiểm tra bàn có hóa đơn chưa thanh toán không
            if (context.HoaDon.Any(h => h.BanID == id && h.TrangThai == "Chưa thanh toán"))
                return "Không thể xóa bàn có hóa đơn chưa thanh toán!";

            context.BanAn.Remove(ban);
            context.SaveChanges();
            return "Xóa bàn thành công!";
        }

        public bool IsBanTrong(int banId)
        {
            var ban = context.BanAn.Find(banId);
            return ban != null && ban.TrangThai == "Trống";
        }

        public bool IsBanDangDung(int banId)
        {
            var ban = context.BanAn.Find(banId);
            return ban != null && ban.TrangThai == "Đang Dùng";
        }
    }
}
