using SiMay.ReflectCache;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiMay.Core
{
    public class FileTransportBlockPacket : EntitySerializerBase
    {
        /// <summary>
        /// 远程目标地址，可空(使用临时路径)
        /// </summary>
        public string DestPath { get; set; }

        public byte[] BinaryBlock { get; set; }

        public long FileContentLength { get; set; }
    }

    public class FileTransportBlockResponsePacket : EntitySerializerBase
    {
        public string FilePath { get; set; }

        /// <summary>
        /// 文件是否打开成功
        /// </summary>
        public bool IsOK { get; set; }
    }
}
