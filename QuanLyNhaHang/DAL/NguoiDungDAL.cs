using QuanLyNhaHang.Models;
using QuanLyNhaHang.Utils;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class NguoiDungDAL
    {
        private Model1 context = new Model1();

        public NguoiDung GetUser(string username, string password)
        {
            // Đăng nhập với mật khẩu dạng plain-text (không mã hóa)
            var user = context.NguoiDung
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);

            return user;
        }

        public NguoiDung GetUserById(int id)
        {
            return context.NguoiDung.Find(id);
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
            // Lưu mật khẩu dạng plain-text theo yêu cầu
            context.NguoiDung.Add(user);
            context.SaveChanges();
        }

        public void Update(NguoiDung nd)
        {
            var old = context.NguoiDung.Find(nd.UserID);
            if (old != null)
            {
                old.TenDangNhap = nd.TenDangNhap;
                // Cập nhật mật khẩu dạng plain-text nếu có thay đổi
                if (!string.IsNullOrEmpty(nd.MatKhau) && nd.MatKhau != old.MatKhau)
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
    }
}
