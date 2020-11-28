using SiMay.Basic;
using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiMay.Service.Core
{
    public class ExecuteFileUpdateSimpleService : RemoteSimpleServiceBase
    {
        private readonly string _localExePath = Assembly.GetExecutingAssembly().Location == string.Empty ? Application.ExecutablePath : Assembly.GetExecutingAssembly().Location;

        [PacketHandler(MessageHead.S_SIMPLE_SERVICE_UPDATE)]
        public void UpdateService(SessionProviderContext session)
        {
            try
            {
                var pack = session.GetMessageEntity<RemoteUpdatePacket>();

                string tempFile = this.GetTempFilePath(".exe");
                if (pack.UrlOrFileUpdate == RemoteUpdateKind.File)
                {
                    using (var stream = File.Open(tempFile, FileMode.Create, FileAccess.Write))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Write(pack.FileData, 0, pack.FileData.Length);
                    }
                }
                else if (pack.UrlOrFileUpdate == RemoteUpdateKind.Url)
                {
                    using (WebClient c = new WebClient())
                    {
                        c.Proxy = null;
                        c.DownloadFile(pack.DownloadUrl, tempFile);
                    }
                }

                if (File.Exists(tempFile) && new FileInfo(tempFile).Length > 0)
                {
                    var batchFile = CreateBatch(_localExePath, tempFile);
                    if (!batchFile.IsNullOrEmpty())
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = true,
                            FileName = batchFile
                        };
                        Process.Start(startInfo);

                        Environment.Exit(0);//退出程序
                    }
                    else
                    {
                        LogHelper.WriteErrorByCurrentMethod("远程更新失败，更新脚本创建失败!");
                    }
                }
                else
                {
                    LogHelper.WriteErrorByCurrentMethod("远程更新失败，服务端文件不存在!");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorByCurrentMethod(ex);
            }


        }

        private string CreateBatch(string currentFilePath, string newFilePath)
        {
            try
            {
                string tempFilePath = this.GetTempFilePath(".bat");

                var processName = Process.GetCurrentProcess().ProcessName;
                bool systemPromission = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().StartParameter.SystemPermission;
                var serviceName = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().StartParameter.ServiceName;
                string updateBatch =
                    "@echo off" + "\r\n" +
                    $"taskkill /f /im {processName}.exe" + Environment.NewLine +
                    "chcp 65001" + "\r\n" +
                    "echo DONT CLOSE THIS WINDOW!" + "\r\n" +
                    "ping -n 10 localhost > nul" + "\r\n" +
                    "del /a /q /f " + "\"" + currentFilePath + "\"" + "\r\n" +
                    "move /y " + "\"" + newFilePath + "\"" + " " + "\"" + currentFilePath + "\"" + "\r\n" +
                    (systemPromission ? $"sc start {serviceName}" : "start \"\" " + "\"" + currentFilePath + "\"") + "\r\n" +
                    "del /a /q /f " + "\"" + tempFilePath + "\"";

                File.WriteAllText(tempFilePath, updateBatch, new UTF8Encoding(false));
                return tempFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetTempFilePath(string extension)
        {
            var currentPath = Path.GetDirectoryName(_localExePath);
            string tempFilePath;
            do
            {
                tempFilePath = Path.Combine(currentPath, Guid.NewGuid().ToString() + extension);
            } while (File.Exists(tempFilePath));

            return tempFilePath;
        }

        [PacketHandler(MessageHead.S_SIMPLE_CHOOES_FILE_UPDATE)]
        public void ChooseFileUpdate(SessionProviderContext session)
        {
            bool systemPromission = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().StartParameter.SystemPermission;

            var filePath = session.GetMessage().ToUnicodeString();
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                var batchFile = CreateBatch(_localExePath, filePath);
                if (!batchFile.IsNullOrEmpty())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        FileName = batchFile
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    LogHelper.WriteErrorByCurrentMethod("远程更新失败，更新脚本创建失败!");
                }
            }
        }
    }
}
