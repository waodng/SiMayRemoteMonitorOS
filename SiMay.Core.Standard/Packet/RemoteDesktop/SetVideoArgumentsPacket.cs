using SiMay.ReflectCache;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiMay.Core.Standard
{
    public class SetVideoArgumentsPacket : EntitySerializerBase
    {
        public int FPS { get; set; }

        public string[] Arguments { get; set; }
    }
}
