using System.Windows.Forms;

namespace QuanLyNhaHang.UI
{
    partial class DuyetBanAnForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.bntHuy = new System.Windows.Forms.Button();
            this.bntDuyet = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(334, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 32);
            this.label3.TabIndex = 19;
            this.label3.Text = "Danh Sách Bàn";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bntHuy
            // 
            this.bntHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bntHuy.BackColor = System.Drawing.Color.LightCoral;
            this.bntHuy.FlatAppearance.BorderSize = 0;
            this.bntHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntHuy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.bntHuy.ForeColor = System.Drawing.Color.White;
            this.bntHuy.Location = new System.Drawing.Point(500, 498);
            this.bntHuy.Name = "bntHuy";
            this.bntHuy.Size = new System.Drawing.Size(120, 40);
            this.bntHuy.TabIndex = 15;
            this.bntHuy.Text = "Hủy";
            this.bntHuy.UseVisualStyleBackColor = false;
            this.bntHuy.Click += new System.EventHandler(this.button2_Click);
            // 
            // bntDuyet
            // 
            this.bntDuyet.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bntDuyet.BackColor = System.Drawing.Color.DodgerBlue;
            this.bntDuyet.FlatAppearance.BorderSize = 0;
            this.bntDuyet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntDuyet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.bntDuyet.ForeColor = System.Drawing.Color.White;
            this.bntDuyet.Location = new System.Drawing.Point(340, 498);
            this.bntDuyet.Name = "bntDuyet";
            this.bntDuyet.Size = new System.Drawing.Size(120, 40);
            this.bntDuyet.TabIndex = 12;
            this.bntDuyet.Text = "Duyệt";
            this.bntDuyet.UseVisualStyleBackColor = false;
            this.bntDuyet.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.dataGridView1.Location = new System.Drawing.Point(90, 55);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(757, 394);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLoad.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(653, 502);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(110, 35);
            this.btnLoad.TabIndex = 20;
            this.btnLoad.Text = "Refresh";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // DuyetBanAnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(937, 574);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bntHuy);
            this.Controls.Add(this.bntDuyet);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DuyetBanAnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Duyệt Bàn Ăn - Nhà Hàng Thịnh Phát";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bntHuy;
        private System.Windows.Forms.Button bntDuyet;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoad;
    }
}