using QuanLyNhaHang.Models;
using System.Linq;

namespace QuanLyNhaHang.DAL
{
    public class NguoiDungDAL
    {
        private Model1 context = new Model1();

        public NguoiDung GetUser(string username, string password)
        {
            return context.NguoiDung
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);
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
    }
}
