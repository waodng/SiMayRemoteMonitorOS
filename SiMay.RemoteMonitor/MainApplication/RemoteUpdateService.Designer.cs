namespace SiMay.RemoteMonitor.MainApplication
{
    partial class RemoteUpdateService
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
            this.radioLocalFile = new System.Windows.Forms.RadioButton();
            this.groupLocalFile = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_caption = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.updateList = new SiMay.RemoteMonitor.UserControls.UListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupLocalFile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioLocalFile
            // 
            this.radioLocalFile.AutoSize = true;
            this.radioLocalFile.Checked = true;
            this.radioLocalFile.Location = new System.Drawing.Point(28, 28);
            this.radioLocalFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioLocalFile.Name = "radioLocalFile";
            this.radioLocalFile.Size = new System.Drawing.Size(88, 19);
            this.radioLocalFile.TabIndex = 6;
            this.radioLocalFile.TabStop = true;
            this.radioLocalFile.Text = "上传更新";
            this.radioLocalFile.UseVisualStyleBackColor = true;
            // 
            // groupLocalFile
            // 
            this.groupLocalFile.Controls.Add(this.btnBrowse);
            this.groupLocalFile.Controls.Add(this.txtPath);
            this.groupLocalFile.Controls.Add(this.label1);
            this.groupLocalFile.Location = new System.Drawing.Point(31, 55);
            this.groupLocalFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupLocalFile.Name = "groupLocalFile";
            this.groupLocalFile.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupLocalFile.Size = new System.Drawing.Size(639, 54);
            this.groupLocalFile.TabIndex = 7;
            this.groupLocalFile.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(507, 19);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 29);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(79, 19);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(419, 25);
            this.txtPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(483, 130);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(184, 29);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update Client";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_caption);
            this.panel1.Location = new System.Drawing.Point(26, 159);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 36);
            this.panel1.TabIndex = 12;
            // 
            // m_caption
            // 
            this.m_caption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_caption.Location = new System.Drawing.Point(0, 0);
            this.m_caption.Name = "m_caption";
            this.m_caption.Size = new System.Drawing.Size(439, 36);
            this.m_caption.TabIndex = 0;
            this.m_caption.Text = "这是一个危险的操作，请确保在您的新客户端使用相同的设置或者URL路径正确后再进行!";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(483, 167);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 29);
            this.button1.TabIndex = 13;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // updateList
            // 
            this.updateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.updateList.FullRowSelect = true;
            this.updateList.HideSelection = false;
            this.updateList.Location = new System.Drawing.Point(28, 203);
            this.updateList.Name = "updateList";
            this.updateList.ProgressColumnIndex = -1;
            this.updateList.Size = new System.Drawing.Size(639, 264);
            this.updateList.TabIndex = 14;
            this.updateList.UseCompatibleStateImageBehavior = false;
            this.updateList.UseWindowsThemStyle = true;
            this.updateList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "远程描述";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "进度";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "详细";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 100;
            // 
            // RemoteUpdateService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 489);
            this.Controls.Add(this.updateList);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.radioLocalFile);
            this.Controls.Add(this.groupLocalFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemoteUpdateService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "远程服务更新";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteUpdateService_FormClosing);
            this.Load += new System.EventHandler(this.RemoteUpdateService_Load);
            this.groupLocalFile.ResumeLayout(false);
            this.groupLocalFile.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioLocalFile;
        private System.Windows.Forms.GroupBox groupLocalFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label m_caption;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        public UserControls.UListView updateList;
    }
}