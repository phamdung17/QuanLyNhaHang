namespace QuanLyNhaHang.UI
{
    partial class ChiTietHoaDon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3 (Title - Bold, center, tươi sáng)
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;  // Top responsive
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);  // Lớn, đậm cho title
            this.label3.ForeColor = System.Drawing.Color.Navy;  // Xanh đậm chuyên nghiệp
            this.label3.Location = new System.Drawing.Point(350, 20);  // Center top (tính từ ClientSize/2)
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "QUẢN LÝ HÓA ĐƠN";  // Giữ nguyên text, nhưng font làm nổi bật
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;  // Center text
                                                                                   // 
                                                                                   // button3 (Đóng - Flat tươi sáng)
                                                                                   // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;  // Bottom right responsive (hoặc center nếu muốn)
            this.button3.BackColor = System.Drawing.Color.DodgerBlue;  // Xanh dương tươi
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;  // Flat design
            this.button3.FlatAppearance.BorderSize = 0;  // Không border, bo tròn
            this.button3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);  // Font lớn, đậm
            this.button3.ForeColor = System.Drawing.Color.White;  // Chữ trắng
            this.button3.Location = new System.Drawing.Point(780, 520);  // Bottom right (từ ClientSize)
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 40);  // Tăng size cho dễ click
            this.button3.TabIndex = 23;
            this.button3.Text = "Đóng";
            this.button3.UseVisualStyleBackColor = false;

                                                                                // 
                                                                                // dataGridView1 (Chi tiết hóa đơn - Styling chuyên nghiệp)
                                                                                // 
            //this.dataGridView1.AllowUser ToAddRows = false;  // Không add row thủ công
            //this.dataGridView1.AllowUser ToDeleteRows = false;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle() { BackColor = System.Drawing.Color.LightGray };  // Hàng xen kẽ xám nhạt
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;  // Full form trừ title/button
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;  // Nền trắng sạch
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;  // Border mỏng
            this.dataGridView1.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle()
            {
                BackColor = System.Drawing.Color.RoyalBlue,  // Header xanh dương
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold)
            };  // Font header đậm
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.LightBlue;  // Grid xanh nhạt
            this.dataGridView1.Location = new System.Drawing.Point(23, 56);  // Giữ nguyên, nhưng anchor sẽ resize
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;  // Chỉ đọc
            this.dataGridView1.RowHeadersVisible = false;  // Ẩn row headers
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;  // Chọn cả row
            this.dataGridView1.Size = new System.Drawing.Size(891, 450);  // Tăng size full width (từ 529), height đến button
            this.dataGridView1.TabIndex = 20;
            // 
            // ChiTietHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;  // Nền trắng sáng tổng
            this.ClientSize = new System.Drawing.Size(937, 574);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);  // Font mặc định hiện đại
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;  // Không resize
            this.MaximizeBox = false;  // Không maximize
            this.Name = "ChiTietHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;  // Căn giữa màn hình
            this.Text = "Chi Tiết Hóa Đơn - Nhà Hàng Thịnh Phát";  // Title đẹp hơn
        
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}