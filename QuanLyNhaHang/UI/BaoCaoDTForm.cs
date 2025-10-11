using QuanLyNhaHang.BLL;
using QuanLyNhaHang.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace QuanLyNhaHang.UI
{
    public partial class BaoCaoDTForm : Form
    {
        public BaoCaoDTForm()
        {
            InitializeComponent();
            InitializeForm();
           
        }

        /// <summary>
        /// Thiết lập các giá trị mặc định khi form được mở.
        /// </summary>
        private void InitializeForm()
        {
            // Cấu hình ô chọn năm
            numNam.Minimum = 2020;
            numNam.Maximum = 2050;
            numNam.Value = DateTime.Now.Year;

            // Cấu hình giao diện biểu đồ
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            
            // Tải dữ liệu mặc định cho năm hiện tại khi mở form
            LoadBaoCaoTheoQuy();
        }

        /// <summary>
        /// Hàm chính để tải và hiển thị dữ liệu doanh thu theo quý.
        /// </summary>
        private void LoadBaoCaoTheoQuy()
        {
            try
            {
                int nam = (int)numNam.Value;

                // 1. Gọi BLL để lấy dữ liệu cho biểu đồ
                var dataBieuDo = RevenueBLL.GetDoanhThuTheoQuy(nam);

                // 2. Gọi BLL để lấy tổng doanh thu cho Label
                decimal tongDoanhThuNam = RevenueBLL.GetTongDoanhThuNam(nam);
                lblTongDoanhThu.Text = RevenueBLL.FormatCurrency(tongDoanhThuNam);

                // 3. Kiểm tra dữ liệu và hiển thị
                if (dataBieuDo == null || dataBieuDo.Count == 0)
                {
                    MessageBox.Show($"Không có dữ liệu doanh thu cho năm {nam}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chart1.Series.Clear(); // Xóa biểu đồ cũ
                    return;
                }

                // 4. Xóa dữ liệu cũ và chuẩn bị hiển thị
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add($"Doanh thu các quý trong năm {nam}");

                var series = new Series("Doanh thu")
                {
                    ChartType = SeriesChartType.Line, // <-- THAY ĐỔI Ở ĐÂY
                    IsValueShownAsLabel = true,
                    LabelFormat = "N0",
                    BorderWidth = 3, // Làm cho đường kẻ dày hơn
                    MarkerStyle = MarkerStyle.Circle, // Thêm dấu tròn ở các điểm dữ liệu
                    MarkerSize = 8
                };

                // 5. Nạp dữ liệu vào biểu đồ
                foreach (dynamic item in dataBieuDo)
                {
                    series.Points.AddXY($"Quý {item.Quy}", item.TongTien);
                }

                chart1.Series.Add(series);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Lỗi khi tải báo cáo doanh thu theo quý");
            }
        }

        /// <summary>
        /// Sự kiện click nút Thống Kê: Tải lại dữ liệu theo năm đã chọn.
        /// </summary>
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadBaoCaoTheoQuy();
        }

        /// <summary>
        /// Sự kiện click nút Xuất Báo Cáo.
        /// </summary>
        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (chart1.Series.Count == 0 || chart1.Series[0].Points.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Xuất báo cáo doanh thu",
                FileName = $"BaoCaoDoanhThuQuy_Nam{numNam.Value}_{DateTime.Now:ddMMyyyy}.pdf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(saveDialog.FileName);
            }
        }

        /// <summary>
        /// Hàm thực hiện việc xuất dữ liệu ra file PDF.
        /// </summary>
        private void ExportToPDF(string filePath)
        {
            try
            {
                // Lưu ảnh biểu đồ vào bộ nhớ
                using (MemoryStream chartImageStream = new MemoryStream())
                {
                    chart1.SaveImage(chartImageStream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(chartImageStream.ToArray());

                    // Khởi tạo tài liệu PDF
                    Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();

                    // Chuẩn bị Font tiếng Việt
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.NORMAL);

                    // Thêm nội dung vào PDF
                    Paragraph title = new Paragraph(chart1.Titles[0].Text.ToUpper(), titleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20 };
                    document.Add(title);
                    document.Add(new Paragraph($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", normalFont) { Alignment = Element.ALIGN_RIGHT });
                    document.Add(new Paragraph($"Tổng Doanh Thu Năm {numNam.Value}: {lblTongDoanhThu.Text}", boldFont) { SpacingAfter = 20 });

                    chartImage.ScaleToFit(document.PageSize.Width - 100, document.PageSize.Height * 0.4f);
                    chartImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(chartImage);

                    // Thêm bảng dữ liệu chi tiết
                    document.Add(new Paragraph("Chi Tiết Dữ Liệu:", boldFont) { SpacingBefore = 25, SpacingAfter = 15 });
                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 60;
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(new Phrase("Quý", boldFont));
                    table.AddCell(new Phrase("Doanh thu (VNĐ)", boldFont));

                    foreach (var point in chart1.Series[0].Points)
                    {
                        table.AddCell(new Phrase(point.AxisLabel, normalFont));
                        table.AddCell(new Phrase(((decimal)point.YValues[0]).ToString("N0"), normalFont));
                    }
                    document.Add(table);

                    document.Close();
                }

                MessageBox.Show($"Xuất báo cáo PDF thành công!\nĐã lưu tại: {filePath}", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException)
            {
                MessageBox.Show("Lỗi xuất PDF: File có thể đang được sử dụng. Vui lòng đóng file và thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, "Đã xảy ra lỗi khi xuất file PDF.");
            }
        }

        private void BaoCaoDTForm_Load(object sender, EventArgs e)
        {
            // load form
            InitializeForm();

        }

        private void numNam_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}