using System;
using SiMay.Basic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using System.Windows.Forms;
using System.Threading;

namespace SiMay.Service.Core
{
    [ServiceName("远程传输文件")]
    [ApplicationName(ApplicationNameConstant.REMOTE_FILE_TRANSPORT)]
    public class FileTransportService : ApplicationRemoteServiceBase
    {
        private string _destionPath = string.Empty;
        private FileStream _fileStream = default;
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private long _receiveCount = 0;
        private long _contentCount = 0;
        public override void SessionClosed()
        {
            try
            {
                if (!_fileStream.IsNull())
                {
                    _fileStream.Close();
                    _fileStream.Dispose();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _autoResetEvent.Set();
            }
        }

        public override void SessionInited(SessionProviderContext session)
        {

        }

        [PacketHandler(MessageHead.S_FILE_TRANSPORT_FRISTBLOCK)]
        public FileTransportBlockResponsePacket FristBlock(SessionProviderContext session)
        {
            var request = session.GetMessageEntity<FileTransportBlockPacket>();
            var filePath = request.DestPath.IsNullOrEmpty() ? GetTempFilePath(".tmp") : request.DestPath;
            _contentCount = request.FileContentLength;
            _destionPath = filePath;
            var result = true;

            if (Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath) && new FileInfo(filePath).Length == request.FileContentLength)
                result = false;
            else
            {
                try
                {
                    _autoResetEvent.WaitOne();
                    _receiveCount = 0;
                    _fileStream = new FileStream(filePath + ".temp",
                            System.IO.FileMode.OpenOrCreate,
                            System.IO.FileAccess.ReadWrite,
                            System.IO.FileShare.ReadWrite);
                    _fileStream.Write(request.BinaryBlock, 0, request.BinaryBlock.Length);
                    _receiveCount += request.BinaryBlock.Length;

                    if (_receiveCount >= _contentCount)
                        this.Close();
                }
                catch (Exception)
                {
                    result = false;
                    this.Close();
                }
            }
            return new FileTransportBlockResponsePacket
            {
                FilePath = filePath,
                IsOK = result
            };
        }

        [PacketHandler(MessageHead.S_FILE_TRANSPORT_NEXT)]
        public void ContinueWrite(SessionProviderContext session)
        {
            try
            {
                var data = session.GetMessage();
                if (_fileStream.CanWrite)
                {
                    _fileStream.Write(data, 0, data.Length);
                    _receiveCount += data.Length;
                }
                if (_receiveCount >= _contentCount)
                    this.Close();
            }
            catch (Exception)
            {
            }
        }

        private void Close()
        {
            try
            {
                _fileStream.Flush();
                _fileStream.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                _fileStream.Dispose();
                _autoResetEvent.Set();
            }
            File.Move(_fileStream.Name, _destionPath);
        }

        private string GetTempFilePath(string extension)
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location == string.Empty ? Application.ExecutablePath : Assembly.GetExecutingAssembly().Location);
            string tempFilePath;
            do
            {
                tempFilePath = Path.Combine(currentPath, Guid.NewGuid().ToString() + extension);
            } while (File.Exists(tempFilePath));

            return tempFilePath;
        }
    }
}
