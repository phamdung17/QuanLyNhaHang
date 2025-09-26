namespace QuanLyNhaHang.Models
{
    public class GioHangItem
    {
        public int MonID { get; set; }
        public string TenMon { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        // Tính trực tiếp
        public decimal ThanhTien => DonGia * SoLuong;
    }
}
