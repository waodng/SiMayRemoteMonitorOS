using SiMay.Core;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.RemoteControls.Core
{
    public class ActivateRemoteServiceSimpleApplication : SimpleApplicationBase
    {
        public async Task RemoteActivateService(SessionProviderContext session, string appKey, params string[] startParameter)
        {
            await CallSimpleService(session, MessageHead.S_SIMPLE_ACTIVATE_REMOTE_SERVICE,
                new ActivateRemoteServicePacket()
                {
                    CommandText = appKey,
                    StartParameter = startParameter
                });
        }
    }
}
