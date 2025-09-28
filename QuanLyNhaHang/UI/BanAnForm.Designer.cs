using System;

namespace QuanLyNhaHang.UI
{
    partial class BanAnForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDatBan = new System.Windows.Forms.Button();
            this.dtpThoiGian = new System.Windows.Forms.DateTimePicker();
            this.numSoNguoi = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.flowBanAn = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnDatBan);
            this.groupBox1.Controls.Add(this.dtpThoiGian);
            this.groupBox1.Controls.Add(this.numSoNguoi);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(483, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(20);
            this.groupBox1.Size = new System.Drawing.Size(419, 526);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Đặt Bàn";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(14, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Thời Gian";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label4.ForeColor = System.Drawing.Color.DarkBlue;
            this.label4.Location = new System.Drawing.Point(14, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Số Người";
            // 
            // btnDatBan
            // 
            this.btnDatBan.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnDatBan.FlatAppearance.BorderSize = 0;
            this.btnDatBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatBan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDatBan.ForeColor = System.Drawing.Color.White;
            this.btnDatBan.Location = new System.Drawing.Point(18, 223);
            this.btnDatBan.Name = "btnDatBan";
            this.btnDatBan.Size = new System.Drawing.Size(104, 36);
            this.btnDatBan.TabIndex = 9;
            this.btnDatBan.Text = "Đặt Bàn";
            this.btnDatBan.UseVisualStyleBackColor = false;
            this.btnDatBan.Click += new System.EventHandler(this.btnDatBan_Click);
            // 
            // dtpThoiGian
            // 
            this.dtpThoiGian.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpThoiGian.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpThoiGian.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpThoiGian.Location = new System.Drawing.Point(92, 125);
            this.dtpThoiGian.Name = "dtpThoiGian";
            this.dtpThoiGian.Size = new System.Drawing.Size(200, 25);
            this.dtpThoiGian.TabIndex = 7;
            this.dtpThoiGian.Value = new System.DateTime(2025, 9, 26, 14, 54, 58, 700);
            this.dtpThoiGian.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dateTimePicker1_KeyPress);
            // 
            // numSoNguoi
            // 
            this.numSoNguoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numSoNguoi.Location = new System.Drawing.Point(92, 56);
            this.numSoNguoi.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSoNguoi.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoNguoi.Name = "numSoNguoi";
            this.numSoNguoi.Size = new System.Drawing.Size(120, 25);
            this.numSoNguoi.TabIndex = 6;
            this.numSoNguoi.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(150, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 30);
            this.label2.TabIndex = 12;
            this.label2.Text = "Sơ Đồ Bàn Ăn";
            // 
            // flowBanAn
            // 
            this.flowBanAn.AutoScroll = true;
            this.flowBanAn.BackColor = System.Drawing.Color.Ivory;
            this.flowBanAn.Location = new System.Drawing.Point(10, 36);
            this.flowBanAn.Name = "flowBanAn";
            this.flowBanAn.Padding = new System.Windows.Forms.Padding(20);
            this.flowBanAn.Size = new System.Drawing.Size(444, 526);
            this.flowBanAn.TabIndex = 11;
            this.flowBanAn.Paint += new System.Windows.Forms.PaintEventHandler(this.flowBanAn_Paint);
            // 
            // BanAnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(937, 574);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowBanAn);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BanAnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Đặt Bàn - Nhà Hàng";
            this.Load += new System.EventHandler(this.BanAnForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDatBan;
        private System.Windows.Forms.DateTimePicker dtpThoiGian;
        private System.Windows.Forms.NumericUpDown numSoNguoi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowBanAn;
    }
}