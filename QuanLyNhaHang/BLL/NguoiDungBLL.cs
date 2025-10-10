using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Utils; // Giả sử ExceptionHelper nằm ở đây
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.BLL
{
    public class NguoiDungBLL
    {
        private static NguoiDungDAL dal = new NguoiDungDAL();

        // --- CÁC HÀM CŨ GIỮ NGUYÊN ---
        public NguoiDung Login(string username, string password)
        {
            return dal.GetUser(username, password);
        }

        public string Register(string username, string password, string fullname, string role = "Client")
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return "Tên đăng nhập và mật khẩu không được để trống!";

            if (dal.CheckUserExists(username))
                return "Tên đăng nhập đã tồn tại!";

            var user = new NguoiDung { TenDangNhap = username, MatKhau = password, HoTen = fullname, VaiTro = role };
            dal.AddUser(user);
            return "Đăng ký thành công!";
        }

        public List<NguoiDung> GetAll()
        {
            return dal.GetAll();
        }

        // --- CÁC HÀM XỬ LÝ ĐÃ ĐƯỢC TỐI ƯU ---

        // ✨ Dành cho Admin
        public string Add(string tenDangNhap, string matKhau, string hoTen, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau) || string.IsNullOrWhiteSpace(hoTen))
                return "Các trường thông tin không được để trống!";

            if (dal.CheckUserExists(tenDangNhap))
                return "Tên đăng nhập đã tồn tại!";

            var user = new NguoiDung { TenDangNhap = tenDangNhap, MatKhau = matKhau, HoTen = hoTen, VaiTro = vaiTro };
            dal.AddUser(user);
            return "Thêm tài khoản thành công!";
        }

        // ✨ Dành cho Admin
        public string Update(int userId, string tenDangNhap, string matKhau, string hoTen, string vaiTro)
        {
            if (userId <= 0) return "Vui lòng chọn một tài khoản!";
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(hoTen))
                return "Tên đăng nhập và họ tên không được để trống!";

            var user = dal.GetById(userId);
            if (user == null) return "Không tìm thấy tài khoản!";

            // Kiểm tra trùng tên đăng nhập với người khác
            if (dal.GetAll().Any(u => u.UserID != userId && u.TenDangNhap == tenDangNhap))
                return "Tên đăng nhập này đã được sử dụng bởi tài khoản khác!";

            user.TenDangNhap = tenDangNhap;
            // Chỉ cập nhật mật khẩu nếu người dùng nhập mật khẩu mới
            if (!string.IsNullOrWhiteSpace(matKhau))
            {
                user.MatKhau = matKhau;
            }
            user.HoTen = hoTen;
            user.VaiTro = vaiTro;
            dal.Update(user);
            return "Cập nhật tài khoản thành công!";
        }

        // ✨ Dành cho Admin
        public string Delete(int userId)
        {
            if (userId <= 0) return "Vui lòng chọn một tài khoản!";
            var user = dal.GetById(userId);
            if (user == null) return "Không tìm thấy tài khoản!";
            if (user.VaiTro == "Admin") return "Không thể xóa tài khoản Admin!";

            dal.Delete(userId);
            return "Xóa tài khoản thành công!";
        }

        // ✨ Dành cho Client
        public static string CapNhatThongTin(int userId, string newHoTen)
        {
            if (string.IsNullOrWhiteSpace(newHoTen))
                return "Họ và tên không được để trống!";

            return dal.UpdateInfo(userId, newHoTen);
        }

        // ✨ Dành cho Client
        public static string DoiMatKhau(int userId, string oldPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
                return "Mật khẩu không được để trống!";
            if (newPassword != confirmPassword)
                return "Mật khẩu mới và mật khẩu xác nhận không khớp!";

            var user = dal.GetById(userId);
            if (user == null) return "Không tìm thấy tài khoản!";
            if (user.MatKhau != oldPassword) return "Mật khẩu hiện tại không chính xác!";

            return dal.ChangePassword(userId, newPassword);
        }
    }
}