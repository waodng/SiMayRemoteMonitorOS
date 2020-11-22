using SiMay.Basic;
using SiMay.Core;
using SiMay.RemoteControls.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.MainApplication
{
    public partial class LockWindow : Form
    {
        public LockWindow()
        {
            InitializeComponent();
        }

        bool _ifree = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (pwdTextBox.Text.Equals(AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().UnLockeCredential))
            {
                this._ifree = true;
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().Haslock = false;
                this.Close();
            }
            else
            {
                MessageBoxHelper.ShowBoxExclamation("解锁密码错误~");
            }
        }

        private void LockWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_ifree)
            {
                e.Cancel = true;
                MessageBoxHelper.ShowBoxExclamation("请输入密码进行解锁哦~");
            }
        }
    }
}
