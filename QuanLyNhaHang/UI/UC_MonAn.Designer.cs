namespace QuanLyNhaHang.UI
{
    partial class UC_MonAn
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picMon;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.Button btnThem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            // dispose image để tránh lock file
            if (picMon?.Image != null) { picMon.Image.Dispose(); picMon.Image = null; }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.picMon = new System.Windows.Forms.PictureBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.lblLoai = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMon)).BeginInit();
            this.SuspendLayout();
            // 
            // picMon
            // 
            this.picMon.BackColor = System.Drawing.Color.White;
            this.picMon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMon.Location = new System.Drawing.Point(5, 5);
            this.picMon.Name = "picMon";
            this.picMon.Size = new System.Drawing.Size(160, 120);
            this.picMon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMon.TabIndex = 0;
            this.picMon.TabStop = false;
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTen.Location = new System.Drawing.Point(8, 130);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(0, 20);
            this.lblTen.TabIndex = 1;
            this.lblTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGia.ForeColor = System.Drawing.Color.DarkRed;
            this.lblGia.Location = new System.Drawing.Point(8, 155);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(0, 19);
            this.lblGia.TabIndex = 2;
            // 
            // lblLoai
            // 
            this.lblLoai.AutoSize = true;
            this.lblLoai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblLoai.ForeColor = System.Drawing.Color.Gray;
            this.lblLoai.Location = new System.Drawing.Point(8, 175);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(0, 15);
            this.lblLoai.TabIndex = 3;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(25, 195);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 30);
            this.btnThem.TabIndex = 4;
            this.btnThem.Text = "Thêm vào giỏ";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // UC_MonAn
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.picMon);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.lblGia);
            this.Controls.Add(this.lblLoai);
            this.Controls.Add(this.btnThem);
            this.Name = "UC_MonAn";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(180, 240);
            this.Load += new System.EventHandler(this.UC_MonAn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picMon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
