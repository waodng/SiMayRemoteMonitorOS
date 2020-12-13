using SiMay.Basic;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Application.FileCommon;
using SiMay.RemoteMonitor.Attributes;
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

namespace SiMay.RemoteMonitor.Application
{
    [Rank(110)]
    [UnconventionalApplication]
    [ApplicationName("远程语音")]
    public partial class FileTransportApplication : Form, IApplication
    {
        private const int PATH_DEST = 0;
        private const int FILE_NAME = 1;
        private const int APP_FULL_NAME = 2;
        private const int TITLE = 3;

        [ApplicationAdapterHandler]
        public FileTransportAdapterHandler FileTransportAdapterHandler { get; set; }

        public FileTransportApplication()
        {
            InitializeComponent();
        }

        public void ContinueTask(ApplicationBaseAdapterHandler handler)
        {
            this.StartTransportFile();
        }

        public void SessionClose(ApplicationBaseAdapterHandler handler)
        {
        }

        public void Start()
        {
            this.FileTransportAdapterHandler.TransportProgressEventHandler += FileTransportAdapterHandler_TransportProgressEventHandler;
            this.Show();
            this.StartTransportFile();
        }

        private void FileTransportAdapterHandler_TransportProgressEventHandler(FileTransportAdapterHandler sender, string fileName, long sendBytesCount, long contentTotalCount)
        {
            this.progressBar.Value = (int)(sendBytesCount / (float)contentTotalCount * 100F);
            this.txtfile.Text = $"正在传输 {Path.GetFileName(fileName)} ({Math.Round(sendBytesCount / 1024F / 1024F, 2)} MB/{Math.Round(contentTotalCount / 1024F / 1024F, 2)} MB)";
        }

        private async void StartTransportFile()
        {
            var destFolder = FileTransportAdapterHandler.StartParamenter[PATH_DEST];
            var fileNames = FileTransportAdapterHandler.StartParamenter[FILE_NAME].Split(',');

            var applicationName = string.Empty;

            if (FileTransportAdapterHandler.StartParamenter.Length > APP_FULL_NAME)
                applicationName = FileTransportAdapterHandler.StartParamenter[APP_FULL_NAME];

            if (FileTransportAdapterHandler.StartParamenter.Length > TITLE)
                this.Text = FileTransportAdapterHandler.StartParamenter[TITLE];

            FileTransportAdapterHandler.BufferSize = 1024 * 256;

            int successedCount = 0;
            //出于安全考虑，fileName仅能为相对运行目录的地址
            foreach (var fileName in fileNames)
            {
                var fullPath = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(fullPath))
                {
                    using (var fs = new FileStream(
                        fullPath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite))
                    {
                        using (IStream stream = new WindowsForFileStream(fs))
                        {
                            var destPath = Path.Combine(destFolder, Path.GetFileName(fullPath));
                            var resultTupe = await FileTransportAdapterHandler.StartTransport(stream, destPath);
                            if (resultTupe.successed)
                                successedCount++;
                        }
                    }

                }
                else
                    throw new ArgumentException($"not found:{fullPath}!");
            }

            //插件传输完成打开应用
            if (!applicationName.IsNullOrEmpty() && successedCount == fileNames.Length)
            {
                var keys = Type.GetType(applicationName).GetActivateApplicationKey();
                var activeRemoteSimpleApplication = SimpleApplicationHelper.SimpleApplicationCollection.GetSimpleApplication<ActivateRemoteServiceSimpleApplication>();
                foreach (var key in keys)
                    await activeRemoteSimpleApplication.RemoteActivateService(FileTransportAdapterHandler.CurrentSession, key);
            }
            this.FileTransportAdapterHandler.CloseSession();
            this.Close();
        }
    }
}
