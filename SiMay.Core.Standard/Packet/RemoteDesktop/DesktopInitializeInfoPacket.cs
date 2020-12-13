using SiMay.ReflectCache;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiMay.Core.Standard
{
    public class DesktopInitializeInfoPacket : EntitySerializerBase
    {
        /// <summary>
        /// Dpix
        /// </summary>
        public float DpiX { get; set; }

        /// <summary>
        /// DpiY
        /// </summary>
        public float DpiY { get; set; }

        /// <summary>
        /// 远程桌面高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 远程桌面宽
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 主屏幕索引
        /// </summary>
        public int PrimaryScreenIndex { get; set; }

        /// <summary>
        /// 所有监视器信息
        /// </summary>
        public MonitorItem[] Monitors { get; set; }

        /// <summary>
        /// 当前目录
        /// </summary>
        public string CurrentDirectory { get; set; }

        /// <summary>
        /// 所需要dll
        /// </summary>
        public string[] DependentDlls { get; set; }
    }
}
