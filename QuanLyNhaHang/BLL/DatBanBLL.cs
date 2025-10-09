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
        public static List<object> GetLichSuDatBanForDisplay(int userId)
        {
            return ExceptionHelper.SafeExecute(() => dal.GetLichSuDatBanForDisplay(userId), new List<object>(), "Lỗi khi lấy lịch sử đặt bàn.");
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



        // ✨ Đặt bàn mới, sử dụng Transaction để đảm bảo an toàn dữ liệu
        public static string DatBanMoi(int banId, int userId, DateTime thoiGian)
        {
            if (banId <= 0) return "ID bàn không hợp lệ!";
            if (userId <= 0) return "ID người dùng không hợp lệ!";
            if (thoiGian < DateTime.Now.AddMinutes(-5)) return "Thời gian đặt bàn không hợp lệ!";

            try
            {
                using (var context = new Model1())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var ban = context.BanAn.FirstOrDefault(b => b.BanID == banId);
                        if (ban == null)
                        {
                            transaction.Rollback();
                            return "Bàn bạn chọn không còn tồn tại.";
                        }

                        if (ban.TrangThai != "Trống")
                        {
                            transaction.Rollback();
                            return $"Bàn {ban.TenBan} vừa được người khác {ban.TrangThai.ToLower()}. Vui lòng chọn bàn khác.";
                        }

                        // ✨ ĐIỀU KIỆN ĐẶT BÀN ĐÃ ĐƯỢC VIẾT LẠI ✨
                        // Kiểm tra xem người dùng có đơn đặt bàn nào đang hoạt động thực sự không
                        var existingActiveDatBan = context.DatBan
                            .Include("BanAn") // Nạp thông tin của Bàn Ăn đi kèm
                            .FirstOrDefault(d =>
                                d.UserID == userId &&
                                (
                                    // Trường hợp 1: Đang chờ duyệt và bàn đã được giữ chỗ
                                    (d.TrangThai == "Chờ duyệt" && d.BanAn.TrangThai == "Đặt trước") ||
                                    // Trường hợp 2: Đã được duyệt và khách đang dùng bàn
                                    (d.TrangThai == "Đã duyệt" && d.BanAn.TrangThai == "Đang dùng")
                                )
                            );

                        if (existingActiveDatBan != null)
                        {
                            transaction.Rollback();
                            return "Bạn đã có một bàn đang hoạt động. Vui lòng hoàn tất hoặc hủy đơn cũ.";
                        }
                        // --- KẾT THÚC PHẦN CẬP NHẬT ĐIỀU KIỆN ---

                        var datBan = new DatBan
                        {
                            BanID = banId,
                            UserID = userId,
                            NgayDat = thoiGian,
                            TrangThai = "Chờ duyệt"
                        };
                        context.DatBan.Add(datBan);

                        ban.TrangThai = "Đặt trước";

                        context.SaveChanges();
                        transaction.Commit();

                        return $"Đặt bàn {ban.TenBan} thành công!";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi hệ thống khi đặt bàn");
                return "Đã xảy ra lỗi hệ thống khi đặt bàn.";
            }
        }


        // ✨ Duyệt yêu cầu đặt bàn, kiểm tra trạng thái nghiêm ngặt
        public static string DuyetDatBan(int datBanId)
        {
            try
            {
                using (var context = new Model1())
                {
                    var datBan = context.DatBan.FirstOrDefault(d => d.DatBanID == datBanId);
                    if (datBan == null) return "Yêu cầu đặt bàn không còn tồn tại!";

                    if (datBan.TrangThai != "Chờ duyệt")
                    {
                        return "Yêu cầu này đã được xử lý hoặc đã bị hủy bởi người dùng.";
                    }

                    datBan.TrangThai = "Đã duyệt";
                    var ban = context.BanAn.Find(datBan.BanID);
                    if (ban != null) ban.TrangThai = "Đang dùng";

                    context.SaveChanges();
                    return "Duyệt bàn thành công!";
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi hệ thống khi duyệt bàn");
                return "Lỗi hệ thống khi duyệt bàn.";
            }
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
        public static string ClientHuyDatBan(int datBanId, int userId)
        {
            return ExceptionHelper.SafeExecute(() =>
            {
                var datBan = dal.GetById(datBanId);
                if (datBan == null)
                {
                    return "Không tìm thấy yêu cầu đặt bàn của bạn!";
                }

                if (datBan.UserID != userId)
                {
                    return "Bạn không có quyền hủy yêu cầu này!";
                }

                // ✨ THAY ĐỔI LOGIC KIỂM TRA TẠI ĐÂY ✨
                // Chỉ cho phép hủy khi trạng thái là "Chờ duyệt"
                if (datBan.TrangThai != "Chờ duyệt")
                {
                    return $"Không thể hủy vì yêu cầu đã được admin xử lý hoặc đã bị hủy trước đó (Trạng thái: {datBan.TrangThai}).";
                }

                // Gọi DAL để thực hiện hủy
                return dal.HuyDatBan(datBanId);

            }, "Lỗi khi hủy đặt bàn", "Đã xảy ra lỗi trong quá trình hủy đặt bàn.");
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