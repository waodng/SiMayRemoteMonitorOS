
namespace SiMay.RemoteMonitor.Application
{
    partial class FileTransportApplication
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtfile = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "进度：";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(80, 105);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(403, 13);
            this.progressBar.TabIndex = 1;
            // 
            // txtfile
            // 
            this.txtfile.AutoSize = true;
            this.txtfile.Location = new System.Drawing.Point(118, 45);
            this.txtfile.Name = "txtfile";
            this.txtfile.Size = new System.Drawing.Size(95, 15);
            this.txtfile.TabIndex = 2;
            this.txtfile.Text = "loading....";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SiMay.RemoteMonitor.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(16, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 76);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // FileTransportApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 154);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtfile);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FileTransportApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "插件传输【请不要关闭窗口】";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label txtfile;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}