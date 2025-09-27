namespace QuanLyNhaHang.UI
{
    partial class UC_BanAn
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
        private System.Windows.Forms.Label lblTenBan;
        private System.Windows.Forms.Label lblTrangThai;

        private void InitializeComponent()
        {
            this.lblTenBan = new System.Windows.Forms.Label();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.chkChon = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblTenBan
            // 
            this.lblTenBan.AutoSize = true;
            this.lblTenBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTenBan.Location = new System.Drawing.Point(10, 10);
            this.lblTenBan.Name = "lblTenBan";
            this.lblTenBan.Size = new System.Drawing.Size(54, 19);
            this.lblTenBan.TabIndex = 0;
            this.lblTenBan.Text = "Bàn 01";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(11, 38);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(35, 13);
            this.lblTrangThai.TabIndex = 1;
            this.lblTrangThai.Text = "Trống";
            // 
            // chkChon
            // 
            this.chkChon.AutoSize = true;
            this.chkChon.BackColor = System.Drawing.Color.LightYellow;
            this.chkChon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChon.Location = new System.Drawing.Point(23, 82);
            this.chkChon.Name = "chkChon";
            this.chkChon.Size = new System.Drawing.Size(80, 19);
            this.chkChon.TabIndex = 2;
            this.chkChon.Text = "Chọn Bàn";
            this.chkChon.UseVisualStyleBackColor = false;
            this.chkChon.CheckedChanged += new System.EventHandler(this.chkChon_CheckedChanged);
            // 
            // UC_BanAn
            // 
            this.BackColor = System.Drawing.Color.LightGreen;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chkChon);
            this.Controls.Add(this.lblTenBan);
            this.Controls.Add(this.lblTrangThai);
            this.Name = "UC_BanAn";
            this.Size = new System.Drawing.Size(124, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkChon;
    }
}