namespace QuanLyNhaHang.UI
{
    partial class QLBanAnForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1 (Danh sách bàn - Styling chuyên nghiệp)
            // 
          
            this.dataGridView1.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle() { BackColor = System.Drawing.Color.LightGray };  // Hàng xen kẽ xám nhạt
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;  // Full middle space
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
            this.dataGridView1.Location = new System.Drawing.Point(50, 80);  // Adjust nhẹ để center hơn
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;  // Chỉ đọc
            this.dataGridView1.RowHeadersVisible = false;  // Ẩn row headers
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;  // Chọn cả row
            this.dataGridView1.Size = new System.Drawing.Size(837, 250);  // Tăng size full width (từ 567), height vừa phải
            this.dataGridView1.TabIndex = 0;
            // 
            // comboBox1 (Trạng thái - Styling tươi sáng)
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;  // Bottom responsive
            this.comboBox1.BackColor = System.Drawing.Color.White;  // Nền trắng
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;  // Không edit trực tiếp
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 10F);  // Font rõ
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "Trống", "Đã Đặt", "Đang Sử Dụng" });  // Items mẫu cho trạng thái
            this.comboBox1.Location = new System.Drawing.Point(450, 350);  // Align với label2
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(150, 25);  // Tăng size nhẹ
            this.comboBox1.TabIndex = 1;
            // 
            // button1 (Thêm - Flat xanh tươi)
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;  // Bottom responsive
            this.button1.BackColor = System.Drawing.Color.DodgerBlue;  // Xanh dương tươi cho thêm
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;  // Flat design
            this.button1.FlatAppearance.BorderSize = 0;  // Không border, bo tròn
            this.button1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);  // Font lớn, đậm
            this.button1.ForeColor = System.Drawing.Color.White;  // Chữ trắng
            this.button1.Location = new System.Drawing.Point(100, 410);  // Left trong buttons row
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 35);  // Tăng size cho dễ click
            this.button1.TabIndex = 2;
            this.button1.Text = "Thêm";
            this.button1.UseVisualStyleBackColor = false;

                                                                                // 
                                                                                // label1 (Tên bàn - Bold, align với input)
                                                                                // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;  // Bottom responsive
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);  // Font đậm
            this.label1.ForeColor = System.Drawing.Color.Navy;  // Xanh đậm
            this.label1.Location = new System.Drawing.Point(100, 355);  // Align với textBox1
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tên bàn";
            // 
            // label2 (Trạng thái - Bold, align với combo)
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(350, 355);  // Align với comboBox1
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Trạng thái";
            // 
            // button2 (Sửa - Flat cam)
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.BackColor = System.Drawing.Color.Orange;  // Cam cho sửa
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(230, 410);  // Tiếp theo button1
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "Sửa";
            this.button2.UseVisualStyleBackColor = false;
 
            // 
            // button3 (Xóa - Flat đỏ nhạt)
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button3.BackColor = System.Drawing.Color.LightCoral;  // Đỏ nhạt cho xóa
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(360, 410);  // Tiếp theo button2
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 35);
            this.button3.TabIndex = 6;
            this.button3.Text = "Xóa";
            this.button3.UseVisualStyleBackColor = false;
     
            // 
            // button4 (Refresh - Flat xanh)
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button4.BackColor = System.Drawing.Color.DodgerBlue;  // Xanh cho refresh
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(490, 410);  // Right trong buttons row
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 35);
            this.button4.TabIndex = 7;
            this.button4.Text = "Refresh";
            this.button4.UseVisualStyleBackColor = false;

            // 
            // textBox1 (Tên bàn input - Border rõ)
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox1.BackColor = System.Drawing.Color.White;  // Nền trắng
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;  // Border mỏng
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F);  // Font rõ
            this.textBox1.Location = new System.Drawing.Point(160, 350);  // Align với label1
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 25);  // Tăng size
            this.textBox1.TabIndex = 8;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;  // Left align
                                                                                      // 
                                                                                      // label3 (Title - Bold, center)
                                                                                      // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;  // Top responsive
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);  // Lớn, đậm cho title
            this.label3.ForeColor = System.Drawing.Color.Navy;  // Xanh đậm
            this.label3.Location = new System.Drawing.Point(420, 30);  // Center top (ClientSize/2 ≈ 468)
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Danh Sách Bàn";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QLBanAnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;  // Nền trắng sáng tổng
            this.ClientSize = new System.Drawing.Size(937, 574);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);  // Font mặc định hiện đại
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;  // Không resize
            this.MaximizeBox = false;  // Không maximize
            this.Name = "QLBanAnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;  // Căn giữa màn hình
            this.Text = "Quản Lý Bàn Ăn - Nhà Hàng Thịnh Phát";  // Title đẹp hơn
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
    }
}