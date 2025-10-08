using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace QuanLyNhaHang.Utils
{
    public static class ExceptionHelper
    {
        // Log file path
        private static readonly string LogFilePath = Path.Combine(Application.StartupPath, "Logs", "ErrorLog.txt");

        // Log exception to file
        public static void LogException(Exception ex, string additionalInfo = "")
        {
            try
            {
                // Tạo thư mục Logs nếu chưa có
                Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));

                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}\n";
                if (!string.IsNullOrEmpty(additionalInfo))
                    logMessage += $"Additional Info: {additionalInfo}\n";
                logMessage += $"Stack Trace: {ex.StackTrace}\n";
                logMessage += "----------------------------------------\n";

                File.AppendAllText(LogFilePath, logMessage);
            }
            catch
            {
                // Nếu không log được thì bỏ qua
            }
        }

        // Get user-friendly error message
        public static string GetUserFriendlyMessage(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                return GetSqlExceptionMessage(sqlEx);
            }
            else if (ex is DbUpdateException dbEx)
            {
                return GetDbUpdateExceptionMessage(dbEx);
            }
            else if (ex is UnauthorizedAccessException)
            {
                return "Không có quyền truy cập. Vui lòng kiểm tra quyền hạn!";
            }
            else if (ex is FileNotFoundException)
            {
                return "Không tìm thấy file. Vui lòng kiểm tra đường dẫn!";
            }
            else if (ex is DirectoryNotFoundException)
            {
                return "Không tìm thấy thư mục. Vui lòng kiểm tra đường dẫn!";
            }
            else if (ex is ArgumentNullException)
            {
                return "Dữ liệu bắt buộc bị thiếu!";
            }
            else if (ex is ArgumentException)
            {
                return "Dữ liệu đầu vào không hợp lệ!";
            }
            else if (ex is InvalidOperationException)
            {
                return "Thao tác không hợp lệ trong trạng thái hiện tại!";
            }
            else if (ex is TimeoutException)
            {
                return "Thao tác quá thời gian chờ. Vui lòng thử lại!";
            }
            else if (ex is OutOfMemoryException)
            {
                return "Hệ thống hết bộ nhớ. Vui lòng khởi động lại ứng dụng!";
            }
            else
            {
                return "Đã xảy ra lỗi không xác định. Vui lòng liên hệ quản trị viên!";
            }
        }

        // Handle SQL exceptions
        private static string GetSqlExceptionMessage(SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case 2:
                    return "Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra kết nối mạng!";
                
                case 17:
                    return "Máy chủ SQL Server không tồn tại hoặc không thể truy cập!";
                
                case 53:
                    return "Không thể kết nối đến máy chủ cơ sở dữ liệu!";
                
                case 18456:
                    return "Tên đăng nhập hoặc mật khẩu cơ sở dữ liệu không đúng!";
                
                case 208:
                    return "Bảng hoặc cột không tồn tại trong cơ sở dữ liệu!";
                
                case 547:
                    return "Không thể xóa dữ liệu vì có dữ liệu liên quan!";
                
                case 2627:
                case 2601:
                    return "Dữ liệu đã tồn tại. Vui lòng kiểm tra lại!";
                
                case 515:
                    return "Không thể lưu dữ liệu vì thiếu thông tin bắt buộc!";
                
                case 8152:
                    return "Dữ liệu quá dài cho trường này!";
                
                default:
                    return $"Lỗi cơ sở dữ liệu: {sqlEx.Message}";
            }
        }

        // Handle Entity Framework exceptions
        private static string GetDbUpdateExceptionMessage(DbUpdateException dbEx)
        {
            if (dbEx.InnerException is SqlException sqlEx)
            {
                return GetSqlExceptionMessage(sqlEx);
            }
            
            return "Lỗi cập nhật cơ sở dữ liệu. Vui lòng kiểm tra dữ liệu đầu vào!";
        }

        // Show error message to user
        public static void ShowErrorMessage(Exception ex, string additionalInfo = "")
        {
            string userMessage = GetUserFriendlyMessage(ex);
            
            if (!string.IsNullOrEmpty(additionalInfo))
            {
                userMessage = $"{additionalInfo}\n\n{userMessage}";
            }

            MessageBox.Show(userMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            // Log exception for debugging
            LogException(ex, additionalInfo);
        }

        // Show warning message
        public static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Show info message
        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Show success message
        public static void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Show confirmation dialog
        public static bool ShowConfirmationMessage(string message, string title = "Xác nhận")
        {
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        // Safe execute with exception handling
        public static T SafeExecute<T>(Func<T> action, T defaultValue = default(T), string errorMessage = "")
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex, errorMessage);
                return defaultValue;
            }
        }

        // Safe execute void with exception handling
        public static void SafeExecute(Action action, string errorMessage = "")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex, errorMessage);
            }
        }

        // Validate and show error if invalid
        public static bool ValidateAndShowError(Func<bool> validation, string errorMessage)
        {
            try
            {
                if (!validation())
                {
                    ShowWarningMessage(errorMessage);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex, errorMessage);
                return false;
            }
        }

        // Check if string is null or empty and show error
        public static bool ValidateStringAndShowError(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                ShowWarningMessage($"{fieldName} không được để trống!");
                return false;
            }
            return true;
        }

        // Check if number is valid and show error
        public static bool ValidateNumberAndShowError(decimal value, string fieldName, decimal minValue = 0)
        {
            if (value < minValue)
            {
                ShowWarningMessage($"{fieldName} phải lớn hơn hoặc bằng {minValue}!");
                return false;
            }
            return true;
        }

        // Check if number is valid and show error
        public static bool ValidateNumberAndShowError(int value, string fieldName, int minValue = 0)
        {
            if (value < minValue)
            {
                ShowWarningMessage($"{fieldName} phải lớn hơn hoặc bằng {minValue}!");
                return false;
            }
            return true;
        }
    }
}
