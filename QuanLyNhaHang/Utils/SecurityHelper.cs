using System;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNhaHang.Utils
{
    public static class SecurityHelper
    {
        // Sử dụng SHA256 để hash mật khẩu
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Mật khẩu không được để trống!");

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Chuyển đổi mật khẩu thành byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi byte array thành string hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Verify mật khẩu
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            string hashedPassword = HashPassword(password);
            return hashedPassword.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        // Tạo salt ngẫu nhiên (nâng cao bảo mật)
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Hash mật khẩu với salt
        public static string HashPasswordWithSalt(string password, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
                throw new ArgumentException("Mật khẩu và salt không được để trống!");

            string saltedPassword = password + salt;
            return HashPassword(saltedPassword);
        }

        // Verify mật khẩu với salt
        public static bool VerifyPasswordWithSalt(string password, string salt, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(hash))
                return false;

            string hashedPassword = HashPasswordWithSalt(password, salt);
            return hashedPassword.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        // Kiểm tra độ mạnh mật khẩu
        public static string ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Mật khẩu không được để trống!";

            if (password.Length < 6)
                return "Mật khẩu phải có ít nhất 6 ký tự!";

            if (password.Length > 50)
                return "Mật khẩu không được quá 50 ký tự!";

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            if (!hasUpper)
                return "Mật khẩu phải có ít nhất 1 chữ hoa!";

            if (!hasLower)
                return "Mật khẩu phải có ít nhất 1 chữ thường!";

            if (!hasDigit)
                return "Mật khẩu phải có ít nhất 1 số!";

            return "OK";
        }

        // Tạo mật khẩu ngẫu nhiên
        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        // Mã hóa chuỗi đơn giản (cho dữ liệu nhạy cảm khác)
        public static string EncryptString(string plainText, string key = "QuanLyNhaHang2024")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(data);
        }

        // Giải mã chuỗi
        public static string DecryptString(string cipherText, string key = "QuanLyNhaHang2024")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] data = Convert.FromBase64String(cipherText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
                }

                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return cipherText; // Trả về chuỗi gốc nếu không giải mã được
            }
        }

        // Kiểm tra tên đăng nhập hợp lệ
        public static string ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Tên đăng nhập không được để trống!";

            if (username.Length < 3)
                return "Tên đăng nhập phải có ít nhất 3 ký tự!";

            if (username.Length > 50)
                return "Tên đăng nhập không được quá 50 ký tự!";

            // Chỉ cho phép chữ cái, số, dấu gạch dưới
            foreach (char c in username)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới!";
            }

            return "OK";
        }

        // Kiểm tra họ tên hợp lệ
        public static string ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "Họ tên không được để trống!";

            if (fullName.Length < 2)
                return "Họ tên phải có ít nhất 2 ký tự!";

            if (fullName.Length > 100)
                return "Họ tên không được quá 100 ký tự!";

            // Chỉ cho phép chữ cái, khoảng trắng, dấu tiếng Việt
            foreach (char c in fullName)
            {
                if (!char.IsLetter(c) && c != ' ' && c != 'à' && c != 'á' && c != 'ạ' && c != 'ả' && c != 'ã' &&
                    c != 'â' && c != 'ầ' && c != 'ấ' && c != 'ậ' && c != 'ẩ' && c != 'ẫ' &&
                    c != 'ă' && c != 'ằ' && c != 'ắ' && c != 'ặ' && c != 'ẳ' && c != 'ẵ' &&
                    c != 'è' && c != 'é' && c != 'ẹ' && c != 'ẻ' && c != 'ẽ' &&
                    c != 'ê' && c != 'ề' && c != 'ế' && c != 'ệ' && c != 'ể' && c != 'ễ' &&
                    c != 'ì' && c != 'í' && c != 'ị' && c != 'ỉ' && c != 'ĩ' &&
                    c != 'ò' && c != 'ó' && c != 'ọ' && c != 'ỏ' && c != 'õ' &&
                    c != 'ô' && c != 'ồ' && c != 'ố' && c != 'ộ' && c != 'ổ' && c != 'ỗ' &&
                    c != 'ơ' && c != 'ờ' && c != 'ớ' && c != 'ợ' && c != 'ở' && c != 'ỡ' &&
                    c != 'ù' && c != 'ú' && c != 'ụ' && c != 'ủ' && c != 'ũ' &&
                    c != 'ư' && c != 'ừ' && c != 'ứ' && c != 'ự' && c != 'ử' && c != 'ữ' &&
                    c != 'ỳ' && c != 'ý' && c != 'ỵ' && c != 'ỷ' && c != 'ỹ' &&
                    c != 'đ')
                {
                    return "Họ tên chỉ được chứa chữ cái và khoảng trắng!";
                }
            }

            return "OK";
        }
        // vì vấn đề tương thích nên không sử dụng các tính năng mới hơn nhóm xin dừng không phát triển thêm
    }
}
