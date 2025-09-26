using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class GioHangBLL
    {
        private readonly List<GioHangItem> items = new List<GioHangItem>();

        // Thêm món (tự động cộng dồn)
        public void Add(ThucDonViewModel mon, int soLuong = 1)
        {
            if (mon == null || soLuong <= 0) return;

            var item = items.FirstOrDefault(i => i.MonID == mon.MonID);
            if (item != null)
                item.SoLuong += soLuong;
            else
                items.Add(new GioHangItem
                {
                    MonID = mon.MonID,
                    TenMon = mon.TenMon,
                    DonGia = mon.DonGia,
                    SoLuong = soLuong
                });
        }

        // Cập nhật số lượng (0 thì xóa)
        public void UpdateQuantity(int monId, int soLuong)
        {
            var item = items.FirstOrDefault(i => i.MonID == monId);
            if (item == null) return;

            if (soLuong <= 0)
                items.Remove(item);
            else
                item.SoLuong = soLuong;
        }

        // Xóa món
        public void Remove(int monId) => items.RemoveAll(i => i.MonID == monId);

        // Xóa giỏ
        public void Clear() => items.Clear();

        // Lấy danh sách hiển thị
        public List<GioHangItem> GetItems() => items.ToList();

        // Tổng số lượng
        public int GetTotalQuantity() => items.Sum(i => i.SoLuong);

        // Tổng tiền
        public decimal GetTotal() => items.Sum(i => i.ThanhTien);
        public string GetTotalFormatted()
        {
            return GetTotal().ToString("N0") + " đ";
        }

        // Kiểm tra rỗng
        public bool IsEmpty() => !items.Any();
    }
}
