using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.UnconventionalApplication
{
    [Rank(100)]
    [ApplicationName("远程更新")]
    public class RemoteUpdateApplication : ListViewItem, IApplication
    {
        [ApplicationAdapterHandler]
        public FileTransportAdapterHandler FileTransportAdapterHandler { get; set; }

        public void ContinueTask(ApplicationBaseAdapterHandler handler)
        {

        }

        public void SessionClose(ApplicationBaseAdapterHandler handler)
        {

        }

        public void SetParameter(object arg)
        {

        }

        public void Start()
        {
            this.Text = FileTransportAdapterHandler.OriginName;
            FileTransportAdapterHandler.TransportProgressEventHandler += FileTransportAdapterHandler_TransportProgressEventHandler;
        }

        private void FileTransportAdapterHandler_TransportProgressEventHandler(FileTransportAdapterHandler sender, string fileName, long sendBytesCount, long ContentTotalCount)
        {
            this.SubItems.Add((sendBytesCount / (float)ContentTotalCount * 100F).ToString());
        }
    }
}
