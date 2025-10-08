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
            // Validation
            if (!ExceptionHelper.ValidateStringAndShowError(tenBan, "Tên bàn"))
                return "Tên bàn không hợp lệ!";

            if (!IsValidTrangThai(trangThai))
            {
                ExceptionHelper.ShowWarningMessage("Trạng thái bàn không hợp lệ!");
                return "Trạng thái bàn không hợp lệ!";
            }

            // Business logic: Kiểm tra tên bàn trùng (trừ bàn hiện tại)
            var existingBan = GetAll().FirstOrDefault(b => b.BanID != banId && b.TenBan.Equals(tenBan, StringComparison.OrdinalIgnoreCase));
            if (existingBan != null)
            {
                ExceptionHelper.ShowWarningMessage("Tên bàn đã tồn tại!");
                return "Tên bàn đã tồn tại!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.Update(banId, tenBan, trangThai), "Lỗi khi cập nhật bàn ăn", "Lỗi khi cập nhật bàn ăn");
        }

        public static string XoaBan(int banId)
        {
            // Business logic: Kiểm tra bàn có thể xóa không
            var ban = GetById(banId);
            if (ban == null)
            {
                ExceptionHelper.ShowWarningMessage("Không tìm thấy bàn cần xóa!");
                return "Không tìm thấy bàn cần xóa!";
            }

            if (ban.TrangThai == "Đang Dùng")
            {
                ExceptionHelper.ShowWarningMessage("Không thể xóa bàn đang được sử dụng!");
                return "Không thể xóa bàn đang được sử dụng!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.Delete(banId), "Lỗi khi xóa bàn ăn", "Lỗi khi xóa bàn ăn");
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