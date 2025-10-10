using QuanLyNhaHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class NguoiDungDAL
    {
        // ✨ Sửa lỗi 1: Sử dụng một instance 'context' chung cho toàn bộ lớp DAL.
        // Điều này đảm bảo các đối tượng được trả về vẫn được "theo dõi" (tracked)
        // và có thể sử dụng ở các lớp khác mà không bị lỗi.
        private Model1 context = new Model1();

        public NguoiDung GetUser(string username, string password)
        {
            return context.NguoiDung
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);
        }

        public NguoiDung GetById(int userId)
        {
            // Bây giờ hàm này sẽ sử dụng context chung và hoạt động đúng
            return context.NguoiDung.Find(userId);
        }

        public List<NguoiDung> GetAll()
        {
            return context.NguoiDung.ToList();
        }

        public bool CheckUserExists(string username)
        {
            return context.NguoiDung.Any(u => u.TenDangNhap == username);
        }

        public void AddUser(NguoiDung user)
        {
            context.NguoiDung.Add(user);
            context.SaveChanges();
        }

        public void Update(NguoiDung nd)
        {
            var old = context.NguoiDung.Find(nd.UserID);
            if (old != null)
            {
                old.TenDangNhap = nd.TenDangNhap;
                if (!string.IsNullOrEmpty(nd.MatKhau))
                {
                    old.MatKhau = nd.MatKhau;
                }
                old.HoTen = nd.HoTen;
                old.VaiTro = nd.VaiTro;
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var nd = context.NguoiDung.Find(id);
            if (nd != null)
            {
                context.NguoiDung.Remove(nd);
                context.SaveChanges();
            }
        }

        // Hàm cập nhật thông tin cá nhân
        public string UpdateInfo(int userId, string newHoTen)
        {
            var user = context.NguoiDung.Find(userId);
            if (user == null) return "Không tìm thấy người dùng!";

            user.HoTen = newHoTen;
            context.SaveChanges();
            return "Cập nhật thông tin thành công!";
        }

        // Hàm thay đổi mật khẩu
        public string ChangePassword(int userId, string newPassword)
        {
            var user = context.NguoiDung.Find(userId);
            if (user == null) return "Không tìm thấy người dùng!";

            user.MatKhau = newPassword;
            context.SaveChanges();
            return "Đổi mật khẩu thành công!";
        }
    }
}