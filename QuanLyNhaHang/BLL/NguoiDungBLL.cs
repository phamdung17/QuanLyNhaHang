using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Models;
using System.Collections.Generic;

namespace QuanLyNhaHang.BLL
{
    public class NguoiDungBLL
    {
        private NguoiDungDAL dal = new NguoiDungDAL();

        // Đăng nhập
        public NguoiDung Login(string username, string password)
        {
            return dal.GetUser(username, password);
        }

        // Đăng ký (Client tự tạo)
        public string Register(string username, string password, string fullname, string role = "Client")
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return "Tên đăng nhập và mật khẩu không được để trống!";

            if (dal.CheckUserExists(username))
                return "Tên đăng nhập đã tồn tại!";

            var user = new NguoiDung
            {
                TenDangNhap = username,
                MatKhau = password,
                HoTen = fullname,
                VaiTro = role
            };

            dal.AddUser(user);
            return "Đăng ký thành công!";
        }

        // Admin thêm tài khoản (Admin / Client)
        public string Add(string tenDangNhap, string matKhau, string hoTen, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
                return "Tên đăng nhập và mật khẩu không được để trống!";

            if (dal.CheckUserExists(tenDangNhap))
                return "Tên đăng nhập đã tồn tại!";

            var nd = new NguoiDung
            {
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau,
                HoTen = hoTen,
                VaiTro = vaiTro
            };

            dal.AddUser(nd);
            return "Thêm thành công!";
        }

        // Admin sửa tài khoản
        public string Update(int userId, string tenDangNhap, string matKhau, string hoTen, string vaiTro)
        {
            var existingUser = dal.GetUserById(userId);
            if (existingUser == null)
                return "Người dùng không tồn tại!";

            if (existingUser.TenDangNhap != tenDangNhap && dal.CheckUserExists(tenDangNhap))
                return "Tên đăng nhập đã tồn tại!";

            existingUser.TenDangNhap = tenDangNhap;
            existingUser.MatKhau = matKhau;
            existingUser.HoTen = hoTen;
            existingUser.VaiTro = vaiTro;

            dal.Update(existingUser);
            return "Cập nhật thành công!";
        }

        // Admin xoá tài khoản
        public string Delete(int userId)
        {
            var existingUser = dal.GetUserById(userId);
            if (existingUser == null)
                return "Người dùng không tồn tại!";

            dal.Delete(userId);
            return "Xóa thành công!";
        }

        // Lấy tất cả tài khoản (cho DataGridView)
        public List<NguoiDung> GetAll()
        {
            return dal.GetAll();
        }
    }
}
