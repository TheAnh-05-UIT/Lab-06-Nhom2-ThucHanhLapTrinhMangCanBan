namespace TcpClient
{
    partial class frm_KhachHang
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
            this.dgv_Thucdon = new System.Windows.Forms.DataGridView();
            this.btn_PlaceOrder = new System.Windows.Forms.Button();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.lbl_NhapSoBan = new System.Windows.Forms.Label();
            this.nud_BanSo = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Thucdon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BanSo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Thucdon
            // 
            this.dgv_Thucdon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Thucdon.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv_Thucdon.Location = new System.Drawing.Point(0, 0);
            this.dgv_Thucdon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgv_Thucdon.Name = "dgv_Thucdon";
            this.dgv_Thucdon.RowHeadersWidth = 62;
            this.dgv_Thucdon.RowTemplate.Height = 28;
            this.dgv_Thucdon.Size = new System.Drawing.Size(711, 199);
            this.dgv_Thucdon.TabIndex = 0;
            // 
            // btn_PlaceOrder
            // 
            this.btn_PlaceOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PlaceOrder.Location = new System.Drawing.Point(86, 280);
            this.btn_PlaceOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_PlaceOrder.Name = "btn_PlaceOrder";
            this.btn_PlaceOrder.Size = new System.Drawing.Size(150, 48);
            this.btn_PlaceOrder.TabIndex = 1;
            this.btn_PlaceOrder.Text = "Place Order ";
            this.btn_PlaceOrder.UseVisualStyleBackColor = true;
            this.btn_PlaceOrder.Click += new System.EventHandler(this.btn_PlaceOrder_Click);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Connect.Location = new System.Drawing.Point(460, 258);
            this.btn_Connect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(137, 46);
            this.btn_Connect.TabIndex = 2;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // lbl_NhapSoBan
            // 
            this.lbl_NhapSoBan.AutoSize = true;
            this.lbl_NhapSoBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NhapSoBan.Location = new System.Drawing.Point(47, 230);
            this.lbl_NhapSoBan.Name = "lbl_NhapSoBan";
            this.lbl_NhapSoBan.Size = new System.Drawing.Size(90, 20);
            this.lbl_NhapSoBan.TabIndex = 3;
            this.lbl_NhapSoBan.Text = "Đặt bàn số";
            // 
            // nud_BanSo
            // 
            this.nud_BanSo.Location = new System.Drawing.Point(175, 232);
            this.nud_BanSo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nud_BanSo.Name = "nud_BanSo";
            this.nud_BanSo.Size = new System.Drawing.Size(107, 22);
            this.nud_BanSo.TabIndex = 4;
            // 
            // frm_KhachHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.nud_BanSo);
            this.Controls.Add(this.lbl_NhapSoBan);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.btn_PlaceOrder);
            this.Controls.Add(this.dgv_Thucdon);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_KhachHang";
            this.Text = "frm_KhachHang";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Thucdon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BanSo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Thucdon;
        private System.Windows.Forms.Button btn_PlaceOrder;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Label lbl_NhapSoBan;
        private System.Windows.Forms.NumericUpDown nud_BanSo;
    }
}