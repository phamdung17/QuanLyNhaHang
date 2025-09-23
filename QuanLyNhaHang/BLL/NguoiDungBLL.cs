using QuanLyNhaHang.DAL;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.BLL
{
    public class NguoiDungBLL
    {
        private NguoiDungDAL dal = new NguoiDungDAL();

        public NguoiDung Login(string username, string password)
        {
            return dal.GetUser(username, password);
        }

        public string Register(string username, string password, string fullname, string role)
        {
            if (dal.CheckUserExists(username))
            {
                return "Tên đăng nhập đã tồn tại!";
            }

            var user = new NguoiDung
            {
                TenDangNhap = username,
                MatKhau = password, // TODO: mã hóa sau
                HoTen = fullname,
                VaiTro = role // string: "Admin" hoặc "Client"
            };

            dal.AddUser(user);
            return "Đăng ký thành công!";
        }
    }
}
