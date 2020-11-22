using SiMay.Basic;
using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.MainApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor
{
    public partial class DownloadManagement : Form
    {
        public SessionSyncContext SessionSyncContext { get; set; }

        public WebSimpleApplication WebSimpleApplication { get; set; }

        public ShellSimpleApplication ShellSimpleApplication { get; set; }

        private IDictionary<int, DownloadTaskListViewItem> _taskListViewItem = new Dictionary<int, DownloadTaskListViewItem>();

        private bool _start = true;

        public DownloadManagement()
        {
            InitializeComponent();
        }

        private async void DownloadManagement_Load(object sender, EventArgs e)
        {
            this.taskListView.ProgressColumnIndex = 1;
            do
            {
                var downTasks = await WebSimpleApplication.GetHttpDownloadStatusContexts(SessionSyncContext.Session);
                if (!downTasks.IsNull() || !_start)
                {
                    foreach (var task in downTasks)
                        TaskEventHandler(task);
                }
                await Task.Delay(500);
            } while (_start);
        }


        private void TaskEventHandler(HttpDownloadTaskItemContext task)
        {
            if (!_taskListViewItem.ContainsKey(task.Id))
            {
                var listviewItem = new DownloadTaskListViewItem(task);
                _taskListViewItem.Add(task.Id, listviewItem);
                taskListView.Items.Add(listviewItem);
            }
            else
            {
                var listviewItem = _taskListViewItem[task.Id];
                listviewItem.UpdateStatus(task);
            }
        }

        private void DownloadManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            _start = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddDownload())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var task = await WebSimpleApplication.JoinHttpDownload(SessionSyncContext.Session, dlg.FileName, dlg.Url);
                    if (task.IsNull() || !_start)
                        TaskEventHandler(task);
                }
            }

        }
        public class DownloadTaskListViewItem : ListViewItem
        {
            /// <summary>
            /// 下载Id
            /// </summary>
            public int Id { get; set; }

            public int StatuCode { get; set; }

            public string FileName { get; set; }

            public DownloadTaskListViewItem(HttpDownloadTaskItemContext taskItemContext)
            {
                this.Text = Path.GetFileName(taskItemContext.FileName);
                this.SubItems.Add("0");
                this.SubItems.Add("计算中...");
                this.SubItems.Add("计算中...");
                Id = taskItemContext.Id;
                StatuCode = taskItemContext.Status;
                FileName = taskItemContext.FileName;
            }

            public void UpdateStatus(HttpDownloadTaskItemContext taskItemContext)
            {
                StatuCode = taskItemContext.Status;
                if (taskItemContext.Status == 1)
                {
                    this.SubItems[3].Text = GetStatus(taskItemContext.Status);
                    return;
                }
                if (taskItemContext.TotalBytesToReceive < 1)
                    return;
                this.SubItems[1].Text = Math.Round((taskItemContext.BytesReceived / (float)taskItemContext.TotalBytesToReceive * 100f), 2).ToString();
                this.SubItems[2].Text = $"{Math.Round(taskItemContext.BytesReceived / 1024f / 1024f, 2)} MB/{Math.Round(taskItemContext.TotalBytesToReceive / 1024f / 1024f, 2)} MB";
                this.SubItems[3].Text = GetStatus(taskItemContext.Status);
            }

            private string GetStatus(int code)
            {
                switch (code)
                {
                    case 0:
                        return "下载中";
                    case 1:
                        return "下载终止";
                    case 2:
                        return "完成";
                }
                return "未识别状态";
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            foreach (DownloadTaskListViewItem item in taskListView.SelectedItems)
                await WebSimpleApplication.SetHttpDownloadStatus(SessionSyncContext.Session, item.Id.ToString());
            MessageBox.Show("状态已设置!", "提示", 0, MessageBoxIcon.Information);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var validCheck = taskListView.SelectedItems.Cast<DownloadTaskListViewItem>().Any(c => c.StatuCode != 2);
            if (validCheck)
            {
                MessageBox.Show("选择项中有未下载完成的任务!", "提示", 0, MessageBoxIcon.Information);
                return;
            }
            foreach (DownloadTaskListViewItem item in taskListView.SelectedItems)
                await ShellSimpleApplication.ExecuteShell(SessionSyncContext.Session, item.FileName);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var validCheck = taskListView.SelectedItems.Cast<DownloadTaskListViewItem>().Any(c => c.StatuCode == 0);
            if (validCheck)
            {
                MessageBox.Show("选择项中有未下载完成的任务!", "提示", 0, MessageBoxIcon.Information);
                return;
            }

            foreach (DownloadTaskListViewItem item in taskListView.SelectedItems)
            {
                await WebSimpleApplication.RemoveTask(SessionSyncContext.Session, item.Id.ToString());
                _taskListViewItem.Remove(item.Id);
                taskListView.Items.Remove(item);
            }
        }
    }
}
