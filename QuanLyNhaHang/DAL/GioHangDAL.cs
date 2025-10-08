using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class GioHangDAL
    {
        private Model1 context = new Model1();

        // Lưu ý: GioHangItem là class tạm thời, không lưu vào database
        // Chỉ sử dụng để xử lý logic giỏ hàng trong session/memory

        public List<ThucDon> GetThucDonDangBan()
        {
            return context.ThucDon
                .Where(t => t.TrangThai == true)
                .OrderBy(t => t.TenMon)
                .ToList();
        }

        public ThucDon GetThucDonById(int monId)
        {
            return context.ThucDon
                .FirstOrDefault(t => t.MonID == monId && t.TrangThai == true);
        }

        public List<ThucDon> GetThucDonByTen(string tenMon)
        {
            return context.ThucDon
                .Where(t => t.TenMon.Contains(tenMon) && t.TrangThai == true)
                .OrderBy(t => t.TenMon)
                .ToList();
        }

        public List<ThucDon> GetThucDonByGia(decimal giaMin, decimal giaMax)
        {
            return context.ThucDon
                .Where(t => t.TrangThai == true && 
                           t.DonGia >= giaMin && 
                           t.DonGia <= giaMax)
                .OrderBy(t => t.DonGia)
                .ToList();
        }

        public bool IsThucDonDangBan(int monId)
        {
            return context.ThucDon
                .Any(t => t.MonID == monId && t.TrangThai == true);
        }

        public decimal GetGiaThucDon(int monId)
        {
            var thucDon = context.ThucDon.Find(monId);
            return thucDon?.DonGia ?? 0;
        }

        public string GetTenThucDon(int monId)
        {
            var thucDon = context.ThucDon.Find(monId);
            return thucDon?.TenMon ?? "";
        }

        public string GetDonViTinh(int monId)
        {
            var thucDon = context.ThucDon.Find(monId);
            return thucDon?.DonViTinh ?? "";
        }

        // Helper methods cho GioHangItem
        public GioHangItem CreateGioHangItem(int monId, int soLuong)
        {
            var thucDon = GetThucDonById(monId);
            if (thucDon == null)
                return null;

            return new GioHangItem
            {
                MonID = thucDon.MonID,
                TenMon = thucDon.TenMon,
                DonGia = thucDon.DonGia,
                SoLuong = soLuong
            };
        }

        public List<GioHangItem> ConvertThucDonToGioHangItems(List<ThucDon> thucDons, int soLuongMacDinh = 1)
        {
            return thucDons.Select(t => new GioHangItem
            {
                MonID = t.MonID,
                TenMon = t.TenMon,
                DonGia = t.DonGia,
                SoLuong = soLuongMacDinh
            }).ToList();
        }

        // Validation methods
        public string ValidateGioHangItem(int monId, int soLuong)
        {
            if (soLuong <= 0)
                return "Số lượng phải lớn hơn 0!";

            var thucDon = GetThucDonById(monId);
            if (thucDon == null)
                return "Món ăn không tồn tại hoặc đã ngừng bán!";

            return "OK";
        }

        public string ValidateGioHang(List<GioHangItem> gioHang)
        {
            if (gioHang == null || gioHang.Count == 0)
                return "Giỏ hàng trống!";

            foreach (var item in gioHang)
            {
                string validation = ValidateGioHangItem(item.MonID, item.SoLuong);
                if (validation != "OK")
                    return validation;
            }

            return "OK";
        }

        // Tính toán methods
        public decimal TinhTongTienGioHang(List<GioHangItem> gioHang)
        {
            if (gioHang == null || gioHang.Count == 0)
                return 0;

            return gioHang.Sum(item => item.ThanhTien);
        }

        public int TinhTongSoLuongGioHang(List<GioHangItem> gioHang)
        {
            if (gioHang == null || gioHang.Count == 0)
                return 0;

            return gioHang.Sum(item => item.SoLuong);
        }

        public int DemSoMonTrongGioHang(List<GioHangItem> gioHang)
        {
            return gioHang?.Count ?? 0;
        }

        // Utility methods
        public GioHangItem TimMonTrongGioHang(List<GioHangItem> gioHang, int monId)
        {
            return gioHang?.FirstOrDefault(item => item.MonID == monId);
        }

        public List<GioHangItem> ThemMonVaoGioHang(List<GioHangItem> gioHang, int monId, int soLuong)
        {
            if (gioHang == null)
                gioHang = new List<GioHangItem>();

            var existingItem = TimMonTrongGioHang(gioHang, monId);
            if (existingItem != null)
            {
                existingItem.SoLuong += soLuong;
            }
            else
            {
                var newItem = CreateGioHangItem(monId, soLuong);
                if (newItem != null)
                    gioHang.Add(newItem);
            }

            return gioHang;
        }

        public List<GioHangItem> XoaMonKhoiGioHang(List<GioHangItem> gioHang, int monId)
        {
            if (gioHang == null)
                return gioHang;

            gioHang.RemoveAll(item => item.MonID == monId);
            return gioHang;
        }

        public List<GioHangItem> CapNhatSoLuongMon(List<GioHangItem> gioHang, int monId, int soLuong)
        {
            if (gioHang == null)
                return gioHang;

            var item = TimMonTrongGioHang(gioHang, monId);
            if (item != null)
            {
                if (soLuong <= 0)
                    gioHang.Remove(item);
                else
                    item.SoLuong = soLuong;
            }

            return gioHang;
        }

        public List<GioHangItem> XoaTatCaGioHang()
        {
            return new List<GioHangItem>();
        }
    }
}
