using SiMay.Core;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiMay.ModelBinder;
using SiMay.Basic;

namespace SiMay.RemoteControls.Core
{
    [ApplicationKey(ApplicationKeyConstant.REMOTE_FILE_TRANSPORT)]
    public class FileTransportAdapterHandler : ApplicationBaseAdapterHandler
    {
        public event Action<FileTransportAdapterHandler, string, long, long> TransportProgressEventHandler;

        public async Task<(bool successed, string path)> StartTransport(IStream stream, string fileName)
        {
            var filePath = string.Empty;
            var buffer = new byte[1024 * 512];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            long sendBytesCount = 0;

            var responsed = await internalFileTransportBlockResponsePacket(buffer.Copy(0, readCount), stream.Length);
            if (!responsed.IsNull() && responsed.IsOK)
            {
                sendBytesCount += readCount;
                filePath = responsed.FilePath;
                TransportProgressEventHandler?.Invoke(this, filePath, sendBytesCount, stream.Length);
                while (sendBytesCount < stream.Length)
                {
                    readCount = stream.Read(buffer, 0, buffer.Length);
                    var requestResult = await internalTransportNextBlock(buffer.Copy(0, readCount));
                    if (requestResult)
                        sendBytesCount += readCount;
                    else
                        break;
                    TransportProgressEventHandler?.Invoke(this, filePath, sendBytesCount, stream.Length);
                }
            }
            return (sendBytesCount == stream.Length, filePath);
        }

        private async Task<FileTransportBlockResponsePacket> internalFileTransportBlockResponsePacket(byte[] data, long lenght)
        {
            var responsed = await SendTo(MessageHead.S_FILE_TRANSPORT_FRISTBLOCK,
                new FileTransportBlockPacket
                {
                    FileContentLength = lenght,
                    BinaryBlock = data
                });

            if (!responsed.IsNull() && responsed.IsOK)
                return responsed.Datas.GetMessageEntity<FileTransportBlockResponsePacket>();
            else
                return null;
        }

        private async Task<bool> internalTransportNextBlock(byte[] data)
        {
            var responsed = await SendTo(MessageHead.S_FILE_TRANSPORT_NEXT, data);
            return !responsed.IsNull() && responsed.IsOK;
        }
    }
}
