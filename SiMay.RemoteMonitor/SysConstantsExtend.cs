using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiMay.RemoteMonitor
{
    public class SysConstantsExtend
    {
        public const string SessionListItem = "SessionListItem";

        public const string OpenAutoDesktopViews = "OpenAutoDesktopViews";

        public const string DesktopView = "DesktopView";

        public static readonly string ConfigPath = Path.Combine(Environment.CurrentDirectory, "SiMayConfig.json");
    }
}
