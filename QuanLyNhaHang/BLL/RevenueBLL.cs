using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class RevenueBLL
    {
        private static readonly HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private static readonly ChiTietHoaDonDAL chiTietDAL = new ChiTietHoaDonDAL();

        #region Doanh Thu Theo Thời Gian

        public static List<object> GetDoanhThuTheoQuy(int nam)
        {
            return ExceptionHelper.SafeExecute(() =>
            {
                return hoaDonDAL.GetDoanhThuTheoQuy(nam);
            }, new List<object>(), "Lỗi khi lấy doanh thu theo quý");
        }

        #endregion

        #region Thống Kê Tổng Quan

        /// <summary>
        /// Lấy tổng doanh thu của một năm cụ thể.
        /// </summary>
        /// <param name="nam">Năm cần tính doanh thu.</param>
        /// <returns>Tổng doanh thu của năm đó.</returns>
        public static decimal GetTongDoanhThuNam(int nam)
        {
            return ExceptionHelper.SafeExecute(() =>
            {
                // Tận dụng lại hàm đã có để lấy doanh thu theo quý
                var doanhThuTheoQuy = hoaDonDAL.GetDoanhThuTheoQuy(nam);

                // Dùng LINQ để tính tổng cho ngắn gọn
                return doanhThuTheoQuy.Sum(item => (decimal)((dynamic)item).TongTien);

            }, 0, "Lỗi khi tính tổng doanh thu năm");
        }

        #endregion

        #region Tiện ích

        /// <summary>
        /// Định dạng một số thành chuỗi tiền tệ VNĐ.
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            return amount.ToString("C0", cultureInfo); // C0: Currency, 0 decimal places
        }

        #endregion
    }
}