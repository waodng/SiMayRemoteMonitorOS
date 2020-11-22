using SiMay.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.MainApplication
{
    public partial class AddDownload : Form
    {
        public string FileName { get; set; }

        public string Url { get; set; }

        public AddDownload()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (fileName.Text.IsNullOrEmpty() || url.Text.IsNullOrEmpty())
            {
                MessageBox.Show("文件名或者Url输入不能为空!", "提示", 0, MessageBoxIcon.Error);
                return;
            }
            FileName = this.fileName.Text;
            Url = url.Text;
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
