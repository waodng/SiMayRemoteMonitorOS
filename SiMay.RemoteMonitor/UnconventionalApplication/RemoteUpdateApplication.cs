using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Application.FileCommon;
using SiMay.RemoteMonitor.Attributes;
using SiMay.RemoteMonitor.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor
{
    [Rank(100)]
    [UnconventionalApplication]
    [ApplicationName("远程更新")]
    public class RemoteUpdateApplication : ListViewItem, IApplication
    {
        UListView _listView = default;

        /// <summary>
        /// 0=NONE
        /// 1=传输完成
        /// </summary>
        public int StatuCode { get; set; } = 0;

        [ApplicationAdapterHandler]
        public FileTransportAdapterHandler FileTransportAdapterHandler { get; set; }

        public void ContinueTask(ApplicationBaseAdapterHandler handler)
        {
            if (StatuCode == 0)
                this.SubItems[3].Text = "等待分配";
        }

        public void SessionClose(ApplicationBaseAdapterHandler handler)
        {
            if (StatuCode == 0)
                this.SubItems[3].Text = "连接中断";
        }


        public void SetParameter(object arg)
        {
            _listView = arg as UListView;
        }

        public void Start()
        {
            this.Text = FileTransportAdapterHandler.OriginName;
            this.SubItems.Add("0");
            this.SubItems.Add("等待分配..");
            this.SubItems.Add("等待分配..");
            _listView.Items.Add(this);
            FileTransportAdapterHandler.TransportProgressEventHandler += FileTransportAdapterHandler_TransportProgressEventHandler;
        }

        public async Task StartUpdate(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    IStream stream = new WindowsForFileStream(fs);

                    this.SubItems[3].Text = "正在传送";
                    var resultTuple = await FileTransportAdapterHandler.StartTransport(stream);
                    if (resultTuple.successed)
                    {
                        var updateSimpleApp = FileTransportAdapterHandler.SimpleApplicationCollection.GetSimpleApplication<ExecuteFileUpdateSimpleApplication>();
                        await updateSimpleApp.ChooseUpdateService(FileTransportAdapterHandler.CurrentSession, resultTuple.path);
                        StatuCode = 1;
                        this.SubItems[3].Text = "完成";
                        FileTransportAdapterHandler.CloseSession();
                    }
                    else
                        this.SubItems[3].Text = "传送异常";
                }

            }
            else this.SubItems[3].Text = "文件不存在";
        }

        private void FileTransportAdapterHandler_TransportProgressEventHandler(FileTransportAdapterHandler sender, string fileName, long sendBytesCount, long contentTotalCount)
        {
            this.SubItems[1].Text = (sendBytesCount / (float)contentTotalCount * 100F).ToString();
            this.SubItems[2].Text = $"{Math.Round(sendBytesCount / 1024F / 1024F, 2)} MB/{Math.Round(contentTotalCount / 1024F / 1024F, 2)} MB";
        }
    }
}
