using QuanLyNhaHang.Models;
using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class BanAnBLL
    {
        private static BanAnDAL dal = new BanAnDAL();

        #region Public Methods

        public static List<BanAn> GetAll()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetAll(), new List<BanAn>(), "Lỗi khi lấy danh sách bàn ăn");
        }

        public static List<BanAn> GetBanTrong()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetBanTrong(), new List<BanAn>(), "Lỗi khi lấy danh sách bàn trống");
        }

        public static List<BanAn> GetBanDangDung()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetBanDangDung(), new List<BanAn>(), "Lỗi khi lấy danh sách bàn đang dùng");
        }

        public static List<BanAn> GetBanDatTruoc()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetBanDatTruoc(), new List<BanAn>(), "Lỗi khi lấy danh sách bàn đặt trước");
        }

        public static BanAn GetById(int id)
        {
            return ExceptionHelper.SafeExecute(() => dal.GetById(id), null, "Lỗi khi lấy thông tin bàn ăn");
        }

        #endregion

        #region Business Logic Methods

        public static string ThemBan(string tenBan, string trangThai = "Trống")
        {
            // Validation
            if (!ExceptionHelper.ValidateStringAndShowError(tenBan, "Tên bàn"))
                return "Tên bàn không hợp lệ!";

            // Business logic: Kiểm tra tên bàn trùng
            var existingBan = GetAll().FirstOrDefault(b => b.TenBan.Equals(tenBan, StringComparison.OrdinalIgnoreCase));
            if (existingBan != null)
            {
                ExceptionHelper.ShowWarningMessage("Tên bàn đã tồn tại!");
                return "Tên bàn đã tồn tại!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.Add(tenBan, trangThai), "Lỗi khi thêm bàn ăn", "Lỗi khi thêm bàn ăn");
        }

        public static string SuaBan(int banId, string tenBan, string trangThai)
        {
            // 1. Validation các thông tin đầu vào
            if (!ExceptionHelper.ValidateStringAndShowError(tenBan, "Tên bàn"))
                return "Tên bàn không hợp lệ!";

            if (!IsValidTrangThai(trangThai))
            {
                ExceptionHelper.ShowWarningMessage("Trạng thái bàn không hợp lệ!");
                return "Trạng thái bàn không hợp lệ!";
            }

            // 2. Sử dụng Transaction để đảm bảo toàn vẹn dữ liệu khi sửa nhiều bảng
            try
            {
                using (var context = new Model1())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        // Lấy thông tin bàn hiện tại từ CSDL
                        var banHienTai = context.BanAn.FirstOrDefault(b => b.BanID == banId);
                        if (banHienTai == null)
                        {
                            transaction.Rollback();
                            return "Không tìm thấy bàn để cập nhật!";
                        }

                        // ✨ LOGIC KIỂM TRA MỚI ✨
                        // Nếu bàn đang có người dùng ("Đang dùng" hoặc "Đặt trước") và admin muốn chuyển về "Trống"
                        if ((banHienTai.TrangThai == "Đang dùng" || banHienTai.TrangThai == "Đặt trước") && trangThai == "Trống")
                        {
                            // Kiểm tra xem bàn có hóa đơn "Chưa thanh toán" hay không
                            bool coHoaDon = context.HoaDon.Any(h => h.BanID == banId && h.TrangThai == "Chưa thanh toán");

                            if (coHoaDon)
                            {
                                // Nếu có hóa đơn, không cho sửa và báo lỗi
                                transaction.Rollback();
                                return "Không thể sửa bàn về 'Trống' vì còn hóa đơn chưa thanh toán. Vui lòng xử lý hóa đơn trước!";
                            }
                            else
                            {
                                // Nếu không có hóa đơn, tìm và hủy đơn đặt bàn tương ứng
                                var datBan = context.DatBan.FirstOrDefault(d => d.BanID == banId && (d.TrangThai == "Đã duyệt" || d.TrangThai == "Chờ duyệt"));
                                if (datBan != null)
                                {
                                    datBan.TrangThai = "Đã hủy";
                                }
                            }
                        }
                        // --- KẾT THÚC LOGIC MỚI ---

                        // 3. Kiểm tra tên bàn trùng (trừ bàn hiện tại)
                        if (context.BanAn.Any(b => b.BanID != banId && b.TenBan.Equals(tenBan, StringComparison.OrdinalIgnoreCase)))
                        {
                            transaction.Rollback();
                            return "Tên bàn này đã tồn tại!";
                        }

                        // 4. Cập nhật thông tin bàn và lưu tất cả thay đổi
                        banHienTai.TenBan = tenBan;
                        banHienTai.TrangThai = trangThai;

                        context.SaveChanges();
                        transaction.Commit(); // Hoàn tất giao dịch

                        return "Sửa bàn thành công!";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi hệ thống khi sửa bàn");
                return "Đã xảy ra lỗi hệ thống khi sửa bàn.";
            }
        }

        public static string XoaBan(int banId)
        {
            // 1. Lấy thông tin bàn từ CSDL
            var ban = dal.GetById(banId);
            if (ban == null)
            {
                return "Không tìm thấy bàn cần xóa!";
            }

            // 2. ✨ KIỂM TRA NGHIÊM NGẶT ✨
            // Regel 1: Không cho xóa nếu bàn đang "Đang dùng"
            if (ban.TrangThai == "Đang dùng")
            {
                return "Không thể xóa bàn đang được sử dụng!";
            }

            // Regel 2: Không cho xóa nếu bàn có hóa đơn "Chưa thanh toán"
            if (HoaDonBLL.BanCoHoaDonChuaThanhToan(banId))
            {
                return "Không thể xóa bàn vì còn hóa đơn chưa thanh toán!";
            }

            // 3. Nếu tất cả kiểm tra đều qua, gọi DAL để thực hiện xóa
            // DAL sẽ lo việc xóa dữ liệu liên quan (hóa đơn, đặt bàn)
            return dal.Delete(banId);
        }

        public static string CapNhatTrangThai(int banId, string trangThai)
        {
            // Validation
            if (!IsValidTrangThai(trangThai))
            {
                ExceptionHelper.ShowWarningMessage("Trạng thái bàn không hợp lệ!");
                return "Trạng thái bàn không hợp lệ!";
            }

            // Business logic: Kiểm tra bàn có tồn tại không
            var ban = GetById(banId);
            if (ban == null)
            {
                ExceptionHelper.ShowWarningMessage("Không tìm thấy bàn!");
                return "Không tìm thấy bàn!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.UpdateTrangThai(banId, trangThai), "Lỗi khi cập nhật trạng thái bàn", "Lỗi khi cập nhật trạng thái bàn");
        }

        #endregion

        #region Helper Methods

        private static bool IsValidTrangThai(string trangThai)
        {
            string[] validStatuses = { "Trống", "Đặt trước", "Đang Dùng" };
            return validStatuses.Contains(trangThai);
        }

        public static bool IsBanTrong(int banId)
        {
            return ExceptionHelper.SafeExecute(() => dal.IsBanTrong(banId), false, "Lỗi khi kiểm tra trạng thái bàn");
        }

        public static bool IsBanDangDung(int banId)
        {
            return ExceptionHelper.SafeExecute(() => dal.IsBanDangDung(banId), false, "Lỗi khi kiểm tra trạng thái bàn");
        }

        #endregion
    }
}