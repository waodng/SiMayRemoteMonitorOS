using SiMay.Basic;
using SiMay.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.MainApplication
{
    public partial class RemoteUpdateService : Form
    {
        public RemoteUpdateService()
        {
            InitializeComponent();
        }

        bool _cancel;
        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            _cancel = false;
            if (radioLocalFile.Checked)
            {
                if (!File.Exists(txtPath.Text))
                {
                    MessageBoxHelper.ShowBoxError("请选择正确的文件路径!");
                    return;
                }
            }

            if (MessageBox.Show("该操作是危险操作，请确认文件是否正确，否则可能导致上线失败!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                while (!_cancel)
                {
                    foreach (RemoteUpdateApplication app in updateList.Items)
                    {
                        if (app.StatuCode == 0)
                            await app.StartUpdate(txtPath.Text);
                    }
                    await Task.Delay(500);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Executable (*.exe)|*.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = Path.Combine(ofd.InitialDirectory, ofd.FileName);
                }
            }
        }

        private void RemoteUpdateService_Load(object sender, EventArgs e)
        {
            this.updateList.ProgressColumnIndex = 1;
            this.updateList.OwnerDraw = true;
        }

        private void RemoteUpdateService_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("退出会使正在传输或等待分配的任务中断，确定退出吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                foreach (RemoteUpdateApplication app in updateList.Items)
                {
                    if (app.StatuCode == 0 && !app.FileTransportAdapterHandler.IsManualClose())
                        app.FileTransportAdapterHandler.CloseSession();
                }
                _cancel = true;
                updateList.Items.Clear();
            }
            else
                e.Cancel = true;
        }
    }
}
