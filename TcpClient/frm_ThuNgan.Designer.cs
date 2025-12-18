namespace TcpClient
{
    partial class frm_ThuNgan
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
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.colBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMonAn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTilte = new System.Windows.Forms.Label();
            this.txtSoban = new System.Windows.Forms.TextBox();
            this.btnCharge = new System.Windows.Forms.Button();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.btnXuat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBan,
            this.colMonAn});
            this.dgvData.Location = new System.Drawing.Point(13, 67);
            this.dgvData.Margin = new System.Windows.Forms.Padding(4);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(599, 334);
            this.dgvData.TabIndex = 0;
            // 
            // colBan
            // 
            this.colBan.HeaderText = "Bàn";
            this.colBan.Name = "colBan";
            // 
            // colMonAn
            // 
            this.colMonAn.HeaderText = "Món ăn";
            this.colMonAn.Name = "colMonAn";
            this.colMonAn.Width = 500;
            // 
            // lblTilte
            // 
            this.lblTilte.AutoSize = true;
            this.lblTilte.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTilte.Location = new System.Drawing.Point(36, 26);
            this.lblTilte.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTilte.Name = "lblTilte";
            this.lblTilte.Size = new System.Drawing.Size(142, 32);
            this.lblTilte.TabIndex = 1;
            this.lblTilte.Text = "THU NGÂN";
            // 
            // txtSoban
            // 
            this.txtSoban.Location = new System.Drawing.Point(741, 87);
            this.txtSoban.Name = "txtSoban";
            this.txtSoban.Size = new System.Drawing.Size(100, 25);
            this.txtSoban.TabIndex = 2;
            // 
            // btnCharge
            // 
            this.btnCharge.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCharge.Location = new System.Drawing.Point(861, 89);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(114, 23);
            this.btnCharge.TabIndex = 3;
            this.btnCharge.Text = "Thanh toán";
            this.btnCharge.UseVisualStyleBackColor = true;
            this.btnCharge.Click += new System.EventHandler(this.btnCharge_Click);
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.Location = new System.Drawing.Point(764, 226);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(130, 21);
            this.lblTongTien.TabIndex = 4;
            this.lblTongTien.Text = "Hiển thị tổng tiền";
            // 
            // btnXuat
            // 
            this.btnXuat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuat.Location = new System.Drawing.Point(861, 118);
            this.btnXuat.Name = "btnXuat";
            this.btnXuat.Size = new System.Drawing.Size(114, 23);
            this.btnXuat.TabIndex = 5;
            this.btnXuat.Text = "Xuất";
            this.btnXuat.UseVisualStyleBackColor = true;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);
            // 
            // frm_ThuNgan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 414);
            this.Controls.Add(this.btnXuat);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.btnCharge);
            this.Controls.Add(this.txtSoban);
            this.Controls.Add(this.lblTilte);
            this.Controls.Add(this.dgvData);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frm_ThuNgan";
            this.Text = "frm_ThuNgan";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Label lblTilte;
        private System.Windows.Forms.TextBox txtSoban;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMonAn;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Button btnXuat;
    }
}