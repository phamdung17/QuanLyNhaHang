namespace QuanLyNhaHang.UI
{
    partial class UC_MonAn
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTenMon = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.btnThemGio = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 120);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lblTenMon
            // 
            this.lblTenMon.AutoSize = true;
            this.lblTenMon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTenMon.Location = new System.Drawing.Point(10, 130);
            this.lblTenMon.Name = "lblTenMon";
            this.lblTenMon.Size = new System.Drawing.Size(66, 19);
            this.lblTenMon.TabIndex = 2;
            this.lblTenMon.Text = "Tên món";
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGia.ForeColor = System.Drawing.Color.Red;
            this.lblGia.Location = new System.Drawing.Point(10, 155);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(40, 15);
            this.lblGia.TabIndex = 1;
            this.lblGia.Text = "0 VNĐ";
            // 
            // btnThemGio
            // 
            this.btnThemGio.Location = new System.Drawing.Point(10, 180);
            this.btnThemGio.Name = "btnThemGio";
            this.btnThemGio.Size = new System.Drawing.Size(100, 30);
            this.btnThemGio.TabIndex = 0;
            this.btnThemGio.Text = "Thêm giỏ";
            // 
            // UC_MonAn
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(164, 181);
            this.Controls.Add(this.btnThemGio);
            this.Controls.Add(this.lblGia);
            this.Controls.Add(this.lblTenMon);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UC_MonAn";
            //this.Load += new System.EventHandler(this.UC_MonAn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTenMon;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.Button btnThemGio;
    }
}
