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

namespace SiMay.Service.Core
{
    [ServiceName("远程传输文件")]
    [ApplicationKey(ApplicationKeyConstant.REMOTE_FILE_TRANSPORT)]
    public class FileTransportService : ApplicationRemoteService
    {
        FileStream _fileStream = default;
        long _receiveCount = 0;
        long _contentCount = 0;
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
            catch (Exception) { }
        }

        public override void SessionInited(SessionProviderContext session)
        {

        }

        [PacketHandler(MessageHead.S_FILE_TRANSPORT_FRISTBLOCK)]
        public FileTransportBlockResponsePacket FristBlock(SessionProviderContext session)
        {
            var request = session.GetMessageEntity<FileTransportBlockPacket>();
            var tempPath = GetTempFilePath(".tmp");

            _contentCount = request.FileContentLength;
            var result = true;
            try
            {
                _fileStream = new FileStream(tempPath,
                        System.IO.FileMode.OpenOrCreate,
                        System.IO.FileAccess.ReadWrite,
                        System.IO.FileShare.ReadWrite);
                _fileStream.Write(request.BinaryBlock, 0, request.BinaryBlock.Length);
                _receiveCount += request.BinaryBlock.Length;

                if (_receiveCount >= _contentCount)
                    _fileStream.Close();
            }
            catch (Exception)
            {
                result = false;
                _fileStream.Close();
            }
            return new FileTransportBlockResponsePacket
            {
                FilePath = tempPath,
                IsOK = result
            };
        }

        [PacketHandler(MessageHead.S_FILE_TRANSPORT_NEXT)]
        public async void ContinueWrite(SessionProviderContext session)
        {
            try
            {
                var data = session.GetMessage();
                if (_fileStream.CanWrite)
                {
                    await _fileStream.WriteAsync(data, 0, data.Length);
                    _receiveCount += data.Length;
                }
                if (_receiveCount >= _contentCount)
                    _fileStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
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
