using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    partial class UC_MonAn
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox picMon;
        private Label lblTen;
        private Label lblGia;
        private Button btnThem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.picMon = new System.Windows.Forms.PictureBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMon)).BeginInit();
            this.SuspendLayout();
            // 
            // picMon
            // 
            this.picMon.Location = new System.Drawing.Point(3, 3);
            this.picMon.Name = "picMon";
            this.picMon.Size = new System.Drawing.Size(144, 80);
            this.picMon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMon.TabIndex = 0;
            this.picMon.TabStop = false;
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTen.Location = new System.Drawing.Point(3, 86);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(66, 19);
            this.lblTen.TabIndex = 1;
            this.lblTen.Text = "Tên món";
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.ForeColor = System.Drawing.Color.DarkRed;
            this.lblGia.Location = new System.Drawing.Point(3, 110);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(23, 13);
            this.lblGia.TabIndex = 2;
            this.lblGia.Text = "0 đ";
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(80, 106);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(60, 25);
            this.btnThem.TabIndex = 3;
            this.btnThem.Text = "Thêm";
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // UC_MonAn
            // 
            this.Controls.Add(this.picMon);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.lblGia);
            this.Controls.Add(this.btnThem);
            this.Name = "UC_MonAn";
            this.Size = new System.Drawing.Size(150, 140);
            ((System.ComponentModel.ISupportInitialize)(this.picMon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
