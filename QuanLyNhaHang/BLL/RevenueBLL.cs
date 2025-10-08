using QuanLyNhaHang.Models;
using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class RevenueBLL
    {
        private static HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private static ChiTietHoaDonDAL chiTietDAL = new ChiTietHoaDonDAL();

        #region Doanh Thu Theo Thời Gian

        public static List<object> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                return hoaDonDAL.GetDoanhThuTheoNgay(tuNgay, denNgay);
            }, new List<object>(), "Lỗi khi lấy doanh thu theo ngày");
        }

        public static List<object> GetDoanhThuTheoThang(int nam)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                return hoaDonDAL.GetDoanhThuTheoThang(nam);
            }, new List<object>(), "Lỗi khi lấy doanh thu theo tháng");
        }

        public static List<object> GetDoanhThuTheoQuy(int nam)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                return hoaDonDAL.GetDoanhThuTheoQuy(nam);
            }, new List<object>(), "Lỗi khi lấy doanh thu theo quý");
        }

        public static List<object> GetDoanhThuTheoNam(int tuNam, int denNam)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                var result = new List<object>();
                for (int nam = tuNam; nam <= denNam; nam++)
                {
                    var doanhThuNam = hoaDonDAL.GetDoanhThuTheoThang(nam);
                    decimal tongTien = 0;
                    int soHoaDon = 0;

                    foreach (dynamic item in doanhThuNam)
                    {
                        tongTien += item.TongTien;
                        soHoaDon += item.SoHoaDon;
                    }

                    result.Add(new
                    {
                        Nam = nam,
                        SoHoaDon = soHoaDon,
                        TongTien = tongTien
                    });
                }
                return result;
            }, new List<object>(), "Lỗi khi lấy doanh thu theo năm");
        }

        #endregion

        #region Thống Kê Tổng Quan

        public static object GetTongQuanDoanhThu(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                var tuNgayFilter = tuNgay ?? DateTime.Now.AddMonths(-1);
                var denNgayFilter = denNgay ?? DateTime.Now;

                decimal tongDoanhThu = hoaDonDAL.GetTongDoanhThu(tuNgayFilter, denNgayFilter);
                int soHoaDon = hoaDonDAL.GetSoHoaDon(tuNgayFilter, denNgayFilter);
                decimal doanhThuTrungBinh = soHoaDon > 0 ? tongDoanhThu / soHoaDon : 0;

                return new
                {
                    TongDoanhThu = tongDoanhThu,
                    SoHoaDon = soHoaDon,
                    DoanhThuTrungBinh = doanhThuTrungBinh,
                    TuNgay = tuNgayFilter,
                    DenNgay = denNgayFilter
                };
            }, null, "Lỗi khi lấy tổng quan doanh thu");
        }

        public static List<object> GetTopMonAnBanChay(int top = 10, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                return chiTietDAL.GetTopMonAnBanChay(top, tuNgay, denNgay);
            }, new List<object>(), "Lỗi khi lấy top món ăn bán chạy");
        }

        public static List<object> GetThongKeTheoGio(DateTime ngay)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    var startOfDay = ngay.Date;
                    var endOfDay = startOfDay.AddDays(1);

                    var result = new List<object>();
                    for (int gio = 0; gio < 24; gio++)
                    {
                        var startHour = startOfDay.AddHours(gio);
                        var endHour = startOfDay.AddHours(gio + 1);

                        var doanhThuGio = db.HoaDon
                            .Where(h => h.TrangThai == "Đã thanh toán" && 
                                       h.NgayLap >= startHour && 
                                       h.NgayLap < endHour)
                            .Sum(h => h.TongTien) ?? 0;

                        result.Add(new
                        {
                            Gio = gio,
                            DoanhThu = doanhThuGio,
                            SoHoaDon = db.HoaDon.Count(h => h.TrangThai == "Đã thanh toán" && 
                                                           h.NgayLap >= startHour && 
                                                           h.NgayLap < endHour)
                        });
                    }
                    return result;
                }
            }, new List<object>(), "Lỗi khi lấy thống kê theo giờ");
        }

        #endregion

        #region Thống Kê Theo Bàn

        public static List<object> GetThongKeTheoBan(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    var tuNgayFilter = tuNgay ?? DateTime.Now.AddMonths(-1);
                    var denNgayFilter = denNgay ?? DateTime.Now;

                    return db.HoaDon
                        .Where(h => h.TrangThai == "Đã thanh toán" && 
                                   h.NgayLap >= tuNgayFilter && 
                                   h.NgayLap <= denNgayFilter)
                        .GroupBy(h => h.BanAn.TenBan)
                        .Select(g => new
                        {
                            TenBan = g.Key,
                            SoHoaDon = g.Count(),
                            TongDoanhThu = g.Sum(x => x.TongTien),
                            DoanhThuTrungBinh = g.Average(x => x.TongTien)
                        })
                        .OrderByDescending(x => x.TongDoanhThu)
                        .ToList<object>();
                }
            }, new List<object>(), "Lỗi khi lấy thống kê theo bàn");
        }

        #endregion

        #region Thống Kê Theo Khách Hàng

        public static List<object> GetThongKeTheoKhachHang(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    var tuNgayFilter = tuNgay ?? DateTime.Now.AddMonths(-1);
                    var denNgayFilter = denNgay ?? DateTime.Now;

                    return db.HoaDon
                        .Where(h => h.TrangThai == "Đã thanh toán" && 
                                   h.NgayLap >= tuNgayFilter && 
                                   h.NgayLap <= denNgayFilter)
                        .GroupBy(h => new { h.UserID, h.NguoiDung.HoTen })
                        .Select(g => new
                        {
                            TenKhachHang = g.Key.HoTen,
                            SoHoaDon = g.Count(),
                            TongDoanhThu = g.Sum(x => x.TongTien),
                            DoanhThuTrungBinh = g.Average(x => x.TongTien),
                            LanCuoiDen = g.Max(x => x.NgayLap)
                        })
                        .OrderByDescending(x => x.TongDoanhThu)
                        .ToList<object>();
                }
            }, new List<object>(), "Lỗi khi lấy thống kê theo khách hàng");
        }

        #endregion

        #region So Sánh Doanh Thu

        public static object SoSanhDoanhThu(DateTime ngay1, DateTime ngay2)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                var doanhThu1 = hoaDonDAL.GetTongDoanhThu(ngay1.Date, ngay1.Date.AddDays(1));
                var doanhThu2 = hoaDonDAL.GetTongDoanhThu(ngay2.Date, ngay2.Date.AddDays(1));

                decimal chenhLech = doanhThu2 - doanhThu1;
                decimal phanTramThayDoi = doanhThu1 > 0 ? (chenhLech / doanhThu1) * 100 : 0;

                return new
                {
                    Ngay1 = ngay1,
                    DoanhThu1 = doanhThu1,
                    Ngay2 = ngay2,
                    DoanhThu2 = doanhThu2,
                    ChenhLech = chenhLech,
                    PhanTramThayDoi = phanTramThayDoi,
                    XuHuong = chenhLech > 0 ? "Tăng" : chenhLech < 0 ? "Giảm" : "Không đổi"
                };
            }, null, "Lỗi khi so sánh doanh thu");
        }

        public static object SoSanhDoanhThuThang(int thang1, int nam1, int thang2, int nam2)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                var ngay1 = new DateTime(nam1, thang1, 1);
                var ngay2 = new DateTime(nam2, thang2, 1);

                var doanhThu1 = hoaDonDAL.GetTongDoanhThu(ngay1, ngay1.AddMonths(1).AddDays(-1));
                var doanhThu2 = hoaDonDAL.GetTongDoanhThu(ngay2, ngay2.AddMonths(1).AddDays(-1));

                decimal chenhLech = doanhThu2 - doanhThu1;
                decimal phanTramThayDoi = doanhThu1 > 0 ? (chenhLech / doanhThu1) * 100 : 0;

                return new
                {
                    Thang1 = $"{thang1}/{nam1}",
                    DoanhThu1 = doanhThu1,
                    Thang2 = $"{thang2}/{nam2}",
                    DoanhThu2 = doanhThu2,
                    ChenhLech = chenhLech,
                    PhanTramThayDoi = phanTramThayDoi,
                    XuHuong = chenhLech > 0 ? "Tăng" : chenhLech < 0 ? "Giảm" : "Không đổi"
                };
            }, null, "Lỗi khi so sánh doanh thu tháng");
        }

        #endregion

        #region Dự Báo Doanh Thu

        public static List<object> DuBaoDoanhThu(int soNgayTiepTheo = 7)
        {
            return ExceptionHelper.SafeExecute(() => 
            {
                using (var db = new Model1())
                {
                    // Lấy doanh thu 30 ngày gần nhất
                    var ngayBatDau = DateTime.Now.AddDays(-30);
                    var doanhThu30Ngay = db.HoaDon
                        .Where(h => h.TrangThai == "Đã thanh toán" && h.NgayLap >= ngayBatDau)
                        .GroupBy(h => h.NgayLap.Value.Date)
                        .Select(g => new
                        {
                            Ngay = g.Key,
                            DoanhThu = g.Sum(x => x.TongTien)
                        })
                        .OrderBy(x => x.Ngay)
                        .ToList();

                    if (doanhThu30Ngay.Count == 0)
                        return new List<object>();

                    // Tính trung bình doanh thu
                    decimal doanhThuTrungBinh = doanhThu30Ngay.Average(x => x.DoanhThu ?? 0);

                    // Tính xu hướng (tăng/giảm)
                    var doanhThuGanNhat = doanhThu30Ngay.Skip(Math.Max(0, doanhThu30Ngay.Count - 7)).Average(x => x.DoanhThu ?? 0);
                    var doanhThuXaNhat = doanhThu30Ngay.Take(7).Average(x => x.DoanhThu ?? 0);
                    decimal xuHuong = doanhThuGanNhat - doanhThuXaNhat;

                    // Dự báo
                    var result = new List<object>();
                    for (int i = 1; i <= soNgayTiepTheo; i++)
                    {
                        var ngayDuBao = DateTime.Now.AddDays(i);
                        decimal doanhThuDuBao = doanhThuTrungBinh + (xuHuong * i / 7);

                        result.Add(new
                        {
                            Ngay = ngayDuBao,
                            DoanhThuDuBao = Math.Max(0, doanhThuDuBao), // Không âm
                            DoanhThuTrungBinh = doanhThuTrungBinh,
                            XuHuong = xuHuong > 0 ? "Tăng" : xuHuong < 0 ? "Giảm" : "Ổn định"
                        });
                    }

                    return result;
                }
            }, new List<object>(), "Lỗi khi dự báo doanh thu");
        }

        #endregion

        #region Helper Methods

        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0") + " VNĐ";
        }

        public static string FormatPercentage(decimal percentage)
        {
            return percentage.ToString("F2") + "%";
        }

        public static Color GetColorByTrend(decimal change)
        {
            if (change > 0) return Color.Green;
            if (change < 0) return Color.Red;
            return Color.Blue;
        }

        #endregion
    }
}
