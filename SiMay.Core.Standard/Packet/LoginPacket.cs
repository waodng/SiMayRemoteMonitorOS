using SiMay.ReflectCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiMay.Core
{
    public class LoginPacket : EntitySerializerBase
    {
        public string IPV4 { get; set; }

        public string MachineName { get; set; }

        public string Describe { get; set; }

        public string GroupName { get; set; }

        public int ProcessorCount { get; set; }

        public string ProcessorInfo { get; set; }

        public long MemorySize { get; set; }

        public string ServiceVison { get; set; }

        public string UserName { get; set; }

        public string OSVersion { get; set; }

        public bool ExistCameraDevice { get; set; }

        public bool ExitsRecordDevice { get; set; }

        public bool ExitsPlayerDevice { get; set; }

        public string IdentifyId { get; set; }

        public DateTime RunTime { get; set; }
    }
}
