using QuanLyNhaHang.Models;
using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class DatBanBLL
    {
        private static DatBanDAL dal = new DatBanDAL();

        #region Public Methods

        public static List<BanAn> GetBanTrong()
        {
            return ExceptionHelper.SafeExecute(() => BanAnBLL.GetBanTrong(), new List<BanAn>(), "Lỗi khi lấy danh sách bàn trống");
        }

        public static List<DatBan> GetDatBanByUser(int userId)
        {
            return ExceptionHelper.SafeExecute(() => dal.GetByUserId(userId), new List<DatBan>(), "Lỗi khi lấy danh sách đặt bàn của người dùng");
        }

        public static List<DatBan> GetChoDuyet()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetChoDuyet(), new List<DatBan>(), "Lỗi khi lấy danh sách đặt bàn chờ duyệt");
        }

        public static List<DatBan> GetDaDuyet()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetDaDuyet(), new List<DatBan>(), "Lỗi khi lấy danh sách đặt bàn đã duyệt");
        }

        public static List<object> GetDanhSachDatBan()
        {
            return ExceptionHelper.SafeExecute(() => dal.GetDanhSachDatBan(), new List<object>(), "Lỗi khi lấy danh sách đặt bàn");
        }

        #endregion

        #region Business Logic Methods

        public static string DatBanMoi(int banId, int userId, DateTime thoiGian)
        {
            if (banId <= 0)
            {
                ExceptionHelper.ShowWarningMessage("ID bàn không hợp lệ!");
                return "ID bàn không hợp lệ!";
            }

            if (userId <= 0)
            {
                ExceptionHelper.ShowWarningMessage("ID người dùng không hợp lệ!");
                return "ID người dùng không hợp lệ!";
            }

            if (thoiGian < DateTime.Now.AddHours(-1))
            {
                ExceptionHelper.ShowWarningMessage("Thời gian đặt bàn không hợp lệ!");
                return "Thời gian đặt bàn không hợp lệ!";
            }

            if (thoiGian > DateTime.Now.AddDays(30))
            {
                ExceptionHelper.ShowWarningMessage("Không thể đặt bàn quá 30 ngày!");
                return "Không thể đặt bàn quá 30 ngày!";
            }

            if (!BanAnBLL.IsBanTrong(banId))
            {
                ExceptionHelper.ShowWarningMessage("Bàn hiện không trống!");
                return "Bàn hiện không trống!";
            }

            // Logic chính: chỉ đặt bàn, không tạo hóa đơn
            return ExceptionHelper.SafeExecute(
                () => dal.Add(banId, userId, thoiGian, "Chờ duyệt"),
                "Lỗi khi đặt bàn",
                "Lỗi khi đặt bàn"
            );
        }


        public static string DuyetDatBan(int datBanId)
        {
            // Validation
            if (datBanId <= 0)
            {
                ExceptionHelper.ShowWarningMessage("ID đặt bàn không hợp lệ!");
                return "ID đặt bàn không hợp lệ!";
            }

            // Business logic: Kiểm tra yêu cầu đặt bàn có tồn tại và đang chờ duyệt không
            var datBan = ExceptionHelper.SafeExecute(() => dal.GetById(datBanId), null, "Lỗi khi lấy thông tin đặt bàn");
            if (datBan == null)
            {
                ExceptionHelper.ShowWarningMessage("Không tìm thấy yêu cầu đặt bàn!");
                return "Không tìm thấy yêu cầu đặt bàn!";
            }

            if (datBan.TrangThai != "Chờ duyệt")
            {
                ExceptionHelper.ShowWarningMessage("Chỉ duyệt yêu cầu đang ở trạng thái 'Chờ duyệt'!");
                return "Chỉ duyệt yêu cầu đang ở trạng thái 'Chờ duyệt'!";
            }

            // Gọi DAL: duyệt yêu cầu
            return ExceptionHelper.SafeExecute(() => dal.DuyetDatBan(datBanId), "Lỗi khi cập nhật trạng thái bàn", "Lỗi khi cập nhật trạng thái bàn");
        }

        public static string HuyDatBan(int datBanId)
        {
            // Validation
            if (datBanId <= 0)
            {
                ExceptionHelper.ShowWarningMessage("ID đặt bàn không hợp lệ!");
                return "ID đặt bàn không hợp lệ!";
            }

            // Business logic: Kiểm tra yêu cầu đặt bàn có tồn tại không
            var datBan = ExceptionHelper.SafeExecute(() => dal.GetById(datBanId), null, "Lỗi khi lấy thông tin đặt bàn");
            if (datBan == null)
            {
                ExceptionHelper.ShowWarningMessage("Không tìm thấy yêu cầu đặt bàn!");
                return "Không tìm thấy yêu cầu đặt bàn!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.HuyDatBan(datBanId), "Lỗi khi hủy đặt bàn", "Lỗi khi hủy đặt bàn");
        }

        public static string XoaDatBan(int datBanId)
        {
            // Validation
            if (datBanId <= 0)
            {
                ExceptionHelper.ShowWarningMessage("ID đặt bàn không hợp lệ!");
                return "ID đặt bàn không hợp lệ!";
            }

            // Business logic: Chỉ cho phép xóa yêu cầu đã hủy
            var datBan = ExceptionHelper.SafeExecute(() => dal.GetById(datBanId), null, "Lỗi khi lấy thông tin đặt bàn");
            if (datBan == null)
            {
                ExceptionHelper.ShowWarningMessage("Không tìm thấy yêu cầu đặt bàn!");
                return "Không tìm thấy yêu cầu đặt bàn!";
            }

            if (datBan.TrangThai != "Đã hủy")
            {
                ExceptionHelper.ShowWarningMessage("Chỉ có thể xóa yêu cầu đặt bàn đã hủy!");
                return "Chỉ có thể xóa yêu cầu đặt bàn đã hủy!";
            }

            // Gọi DAL
            return ExceptionHelper.SafeExecute(() => dal.Delete(datBanId), "Lỗi khi xóa đặt bàn", "Lỗi khi xóa đặt bàn");
        }

        #endregion

        #region Helper Methods

        public static bool IsUserCoBanDaDuyet(int userId)
        {
            return ExceptionHelper.SafeExecute(() => dal.IsUserCoBanDaDuyet(userId), false, "Lỗi khi kiểm tra trạng thái đặt bàn");
        }

        public static DatBan GetBanDaDuyetByUser(int userId)
        {
            return ExceptionHelper.SafeExecute(() => dal.GetBanDaDuyetByUser(userId), null, "Lỗi khi lấy bàn đang đặt");
        }


        #endregion
    }
}