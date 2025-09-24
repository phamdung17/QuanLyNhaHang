namespace QuanLyNhaHang
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabThucDon = new System.Windows.Forms.TabPage();
            this.flowThucDon = new System.Windows.Forms.FlowLayoutPanel();
            this.tabGioHang = new System.Windows.Forms.TabPage();
            this.tabDatBan = new System.Windows.Forms.TabPage();
            this.tabLichSu = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabThucDon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabThucDon);
            this.tabControl.Controls.Add(this.tabGioHang);
            this.tabControl.Controls.Add(this.tabDatBan);
            this.tabControl.Controls.Add(this.tabLichSu);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1203, 691);
            this.tabControl.TabIndex = 0;
            // 
            // tabThucDon
            // 
            this.tabThucDon.Controls.Add(this.flowThucDon);
            this.tabThucDon.Location = new System.Drawing.Point(4, 33);
            this.tabThucDon.Name = "tabThucDon";
            this.tabThucDon.Padding = new System.Windows.Forms.Padding(3);
            this.tabThucDon.Size = new System.Drawing.Size(1195, 654);
            this.tabThucDon.TabIndex = 0;
            this.tabThucDon.Text = "Thực Đơn";
            this.tabThucDon.UseVisualStyleBackColor = true;
            // 
            // flowThucDon
            // 
            this.flowThucDon.AutoScroll = true;
            this.flowThucDon.BackColor = System.Drawing.Color.Ivory;
            this.flowThucDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowThucDon.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowThucDon.WrapContents = true;
            this.flowThucDon.Location = new System.Drawing.Point(3, 3);
            this.flowThucDon.Name = "flowThucDon";
            this.flowThucDon.Size = new System.Drawing.Size(1189, 648);
            this.flowThucDon.TabIndex = 0;
            // 
            // tabGioHang
            // 
            this.tabGioHang.Location = new System.Drawing.Point(4, 33);
            this.tabGioHang.Name = "tabGioHang";
            this.tabGioHang.Padding = new System.Windows.Forms.Padding(3);
            this.tabGioHang.Size = new System.Drawing.Size(1195, 654);
            this.tabGioHang.TabIndex = 1;
            this.tabGioHang.Text = "Giỏ Hàng";
            this.tabGioHang.UseVisualStyleBackColor = true;
            // 
            // tabDatBan
            // 
            this.tabDatBan.Location = new System.Drawing.Point(4, 33);
            this.tabDatBan.Name = "tabDatBan";
            this.tabDatBan.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatBan.Size = new System.Drawing.Size(1195, 654);
            this.tabDatBan.TabIndex = 2;
            this.tabDatBan.Text = "Đặt Bàn";
            this.tabDatBan.UseVisualStyleBackColor = true;
            // 
            // tabLichSu
            // 
            this.tabLichSu.Location = new System.Drawing.Point(4, 33);
            this.tabLichSu.Name = "tabLichSu";
            this.tabLichSu.Padding = new System.Windows.Forms.Padding(3);
            this.tabLichSu.Size = new System.Drawing.Size(1195, 654);
            this.tabLichSu.TabIndex = 3;
            this.tabLichSu.Text = "Lịch Sử";
            this.tabLichSu.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 691);
            this.Controls.Add(this.tabControl);
            this.Name = "ClientForm";
            this.Text = "Client - Nhà hàng";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabThucDon.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabThucDon;
        private System.Windows.Forms.FlowLayoutPanel flowThucDon;
        private System.Windows.Forms.TabPage tabGioHang;
        private System.Windows.Forms.TabPage tabDatBan;
        private System.Windows.Forms.TabPage tabLichSu;
    }
}
