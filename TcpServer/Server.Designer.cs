namespace TcpServer
{
    partial class Server
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
            this.historyTextBox = new System.Windows.Forms.RichTextBox();
            this.listenButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // historyTextBox
            // 
            this.historyTextBox.Location = new System.Drawing.Point(85, 62);
            this.historyTextBox.Name = "historyTextBox";
            this.historyTextBox.Size = new System.Drawing.Size(627, 344);
            this.historyTextBox.TabIndex = 0;
            this.historyTextBox.Text = "";
            // 
            // listenButton
            // 
            this.listenButton.Location = new System.Drawing.Point(608, 22);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(104, 34);
            this.listenButton.TabIndex = 1;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.listenButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(81, 28);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(67, 20);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Status: ";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.listenButton);
            this.Controls.Add(this.historyTextBox);
            this.Name = "Server";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox historyTextBox;
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.Label statusLabel;
    }
}