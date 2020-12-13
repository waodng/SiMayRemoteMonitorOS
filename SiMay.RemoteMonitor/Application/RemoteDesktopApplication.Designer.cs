
namespace SiMay.RemoteMonitor.Application
{
    partial class RemoteDesktopApplication
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
            this.desktopBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.desktopBox)).BeginInit();
            this.SuspendLayout();
            // 
            // desktopBox
            // 
            this.desktopBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desktopBox.Location = new System.Drawing.Point(0, 0);
            this.desktopBox.Name = "desktopBox";
            this.desktopBox.Size = new System.Drawing.Size(1406, 714);
            this.desktopBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.desktopBox.TabIndex = 0;
            this.desktopBox.TabStop = false;
            // 
            // RemoteDesktopApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 714);
            this.Controls.Add(this.desktopBox);
            this.Name = "RemoteDesktopApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "远程桌面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteDesktopApplication_FormClosing);
            this.Load += new System.EventHandler(this.RemoteDesktopApplication_Load);
            ((System.ComponentModel.ISupportInitialize)(this.desktopBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox desktopBox;
    }
}