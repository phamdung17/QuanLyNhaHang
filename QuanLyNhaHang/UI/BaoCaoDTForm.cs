using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyNhaHang.UI
{
    public partial class BaoCaoDTForm : Form
    {
        public BaoCaoDTForm()
        {
            InitializeComponent();
            InitializeChart();
            LoadDefaultData();
        }

        #region Khởi tạo

        private void InitializeChart()
        {
            // Cấu hình biểu đồ
            chart1.ChartAreas[0].AxisX.Title = "Thời gian";
            chart1.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
            chart1.ChartAreas[0].BackColor = Color.White;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            
            // Cấu hình legend
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;
        }

        private void LoadDefaultData()
        {
            // Mặc định: thống kê theo THÁNG hiện tại
            var firstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddTicks(-1); // đến hết tháng
            dtpTuNgay.Value = firstDay;
            dtpDenNgay.Value = lastDay;
            
            LoadTongQuan();
           
        }

        #endregion

        #region Load Dữ liệu

        private void LoadTongQuan()
        {
            try
            {
                var tongQuan = RevenueBLL.GetTongQuanDoanhThu(dtpTuNgay.Value, dtpDenNgay.Value);
                if (tongQuan != null)
                {
                    dynamic data = tongQuan;
                    lblTongDoanhThu.Text = RevenueBLL.FormatCurrency(data.TongDoanhThu);
                   
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải tổng quan doanh thu");
            }
        }

        private void LoadDoanhThuTheoNgay()
        {
            try
            {
                var start = dtpTuNgay.Value.Date;
                var end = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
                var data = RevenueBLL.GetDoanhThuTheoNgay(start, end);
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show("Không có doanh thu trong khoảng thời gian đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chart1.Series.Clear();
                    chart1.Titles.Clear();
                    return;
                }
                UpdateChart(data, "Doanh thu theo ngày");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải doanh thu theo ngày");
            }
        }

        // Thống kê theo khoảng thời gian tùy chọn (chuẩn hóa đầu-cuối ngày)
        private void LoadDoanhThuTheoKhoangThoiGian()
        {
            try
            {
                var start = dtpTuNgay.Value.Date; // 00:00:00
                var end = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59.999...

                var data = RevenueBLL.GetDoanhThuTheoNgay(start, end);
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show("Không có doanh thu trong khoảng thời gian đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chart1.Series.Clear();
                    chart1.Titles.Clear();
                    return;
                }
                string title = $"Doanh thu theo ngày ({start:dd/MM/yyyy} - {end:dd/MM/yyyy})";
                UpdateChart(data, title);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải doanh thu theo khoảng thời gian");
            }
        }

        private void LoadDoanhThuTheoThang()
        {
            try
            {
                int nam = dtpDenNgay.Value.Year;
                var data = RevenueBLL.GetDoanhThuTheoThang(nam);
                UpdateChart(data, "Doanh thu theo tháng");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải doanh thu theo tháng");
            }
        }

        private void LoadDoanhThuTheoQuy()
        {
            try
            {
                int nam = dtpDenNgay.Value.Year;
                var data = RevenueBLL.GetDoanhThuTheoQuy(nam);
                UpdateChart(data, "Doanh thu theo quý");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải doanh thu theo quý");
            }
        }

        private void LoadDoanhThuTheoNam()
        {
            try
            {
                int nam = dtpDenNgay.Value.Year;
                var data = RevenueBLL.GetDoanhThuTheoNam(nam, nam);
                UpdateChart(data, "Doanh thu theo năm");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải doanh thu theo năm");
            }
        }

        private void LoadTopMonAnBanChay()
        {
            try
            {
                var data = RevenueBLL.GetTopMonAnBanChay(10, dtpTuNgay.Value, dtpDenNgay.Value);
                UpdateChart(data, "Top món ăn bán chạy");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải top món ăn bán chạy");
            }
        }

        private void LoadThongKeTheoBan()
        {
            try
            {
                var data = RevenueBLL.GetThongKeTheoBan(dtpTuNgay.Value, dtpDenNgay.Value);
                UpdateChart(data, "Thống kê theo bàn");
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải thống kê theo bàn");
            }
        }

        #endregion

        #region Cập nhật Biểu đồ

        private void UpdateChart(List<object> data, string title)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            
            chart1.Titles.Add(title);
            
            var series = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.DodgerBlue,
                BorderWidth = 2
            };

            foreach (dynamic item in data)
            {
                string xValue = "";
                decimal yValue = 0;

                // Xác định giá trị X và Y dựa trên loại dữ liệu
                if (item.GetType().GetProperty("Ngay") != null)
                {
                    xValue = ((DateTime)item.Ngay).ToString("dd/MM");
                    yValue = item.TongTien;
                }
                else if (item.GetType().GetProperty("Thang") != null)
                {
                    xValue = $"Tháng {item.Thang}";
                    yValue = item.TongTien;
                }
                else if (item.GetType().GetProperty("Quy") != null)
                {
                    xValue = $"Quý {item.Quy}";
                    yValue = item.TongTien;
                }
                else if (item.GetType().GetProperty("Nam") != null)
                {
                    xValue = $"Năm {item.Nam}";
                    yValue = item.TongTien;
                }
                else if (item.GetType().GetProperty("TenMon") != null)
                {
                    xValue = item.TenMon;
                    yValue = item.TongSoLuong;
                }
                else if (item.GetType().GetProperty("TenBan") != null)
                {
                    xValue = item.TenBan;
                    yValue = item.TongDoanhThu;
                }

                series.Points.AddXY(xValue, yValue);
            }

            chart1.Series.Add(series);
        }

        #endregion

        #region Sự kiện

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (dtpTuNgay.Value > dtpDenNgay.Value)
            {
                ExceptionHelper.ShowWarningMessage("Ngày bắt đầu không được lớn hơn ngày kết thúc!");
                return;
            }
            // Theo khoảng thời gian do người dùng chọn
            LoadDoanhThuTheoKhoangThoiGian();
        }

  

        private void btnTheoThang_Click(object sender, EventArgs e)
        {
            LoadDoanhThuTheoThang();
        }

        private void btnTheoQuy_Click(object sender, EventArgs e)
        {
            LoadDoanhThuTheoQuy();
        }

        private void btnTheoNam_Click(object sender, EventArgs e)
        {
            LoadDoanhThuTheoNam();
        }

        private void btnTopMonAn_Click(object sender, EventArgs e)
        {
            LoadTopMonAnBanChay();
        }

        private void btnTheoBan_Click(object sender, EventArgs e)
        {
            LoadThongKeTheoBan();
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf",
                    Title = "Xuất báo cáo doanh thu"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;
                    string extension = System.IO.Path.GetExtension(filePath).ToLower();

                    if (extension == ".xlsx")
                    {
                        ExportToExcel(filePath);
                    }
                    else if (extension == ".pdf")
                    {
                        ExportToPDF(filePath);
                    }

                    ExceptionHelper.ShowSuccessMessage("Xuất báo cáo thành công!");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi xuất báo cáo");
            }
        }

        #endregion

        #region Xuất báo cáo

        private void ExportToExcel(string filePath)
        {
            // TODO: Implement Excel export
            ExceptionHelper.ShowInfoMessage("Tính năng xuất Excel đang được phát triển!");
        }

        private void ExportToPDF(string filePath)
        {
            // TODO: Implement PDF export
            ExceptionHelper.ShowInfoMessage("Tính năng xuất PDF đang được phát triển!");
        }

        #endregion

        private void BaoCaoDTForm_Load(object sender, EventArgs e)
        {
            // Đã cấu hình mặc định trong LoadDefaultData -> không cần lặp lại
            LoadDoanhThuTheoThang();
        }
    }
}
