using SiMay.Basic;
using SiMay.Core;
using SiMay.Core.Standard;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.RemoteControls.Core
{
    [ApplicationName(ApplicationNameConstant.REMOTE_DESKTOP)]
    public class RemoteDesktopAdapterHandler : ApplicationBaseAdapterHandler
    {
        public event Action<byte[]> VideoStreamEventHandler;

        [PacketHandler(MessageHead.C_DESKTOP_STREAM)]
        public void VideoStreamReceive(SessionProviderContext session)
            => VideoStreamEventHandler?.Invoke(session.GetMessage());

        public async Task<DesktopInitializeInfoPacket> GetInitializeInfor()
        {
            var responsd = await SendTo(MessageHead.S_DESKTOP_INIT_INFO);
            if (!responsd.IsNull() && responsd.IsOK)
                return responsd.Datas.GetMessageEntity<DesktopInitializeInfoPacket>();
            else
                return default;
        }

        public void StartPullStream(int fps, string[] arguments)
        {
            SendToAsync(MessageHead.S_DESKTOP_START_PUSH,
                new SetVideoArgumentsPacket()
                {
                    FPS = fps,
                    Arguments = arguments
                });
        }
    }
}
