using SiMay.Basic;
using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using SiMay.Platform.Windows.Helper;
using SiMay.RemoteService.Loader;
using System;
using System.Linq;

namespace SiMay.Service.Core
{
    public class MainApplicationService : MainApplicationServiceBase, IAppMainService
    {
        public MainApplicationService()
        {
            //注册简单服务
            SimpleServiceCollection
                .SimpleServiceRegister<ConfiguartionSimpleService>()
                .SimpleServiceRegister<DesktopViewSimpleService>()
                .SimpleServiceRegister<ExecuteFileUpdateSimpleService>()
                .SimpleServiceRegister<MessageBoxSimpleService>()
                .SimpleServiceRegister<ShellSimpleService>()
                .SimpleServiceRegister<WebSimpleService>()
                .SimpleServiceRegister<ActivateRemoteServiceSimpleService>()
                .SimpleServiceRegister<WsStatusSimpleService>();

            GlobalMessageBus.SubScribe(BusTopic.CREATE_SERVICE_POST_TO_SEQUENCE, CreateRemoteServiceBusEvent);
        }

        private void CreateRemoteServiceBusEvent(string topic, object message)
        {
            this.PostToAwaitSequence(message as ApplicationRemoteServiceBase);
        }

        /// <summary>
        /// 发送上线包
        /// </summary>
        [PacketHandler(MessageHead.S_GLOBAL_OK)]
        public void SendLoginPack(SessionProviderContext session)
        {
            var loginPack = new LoginPacket();
            loginPack.IPV4 = GetSystemInforHelper.GetLocalIPv4();
            loginPack.MachineName = Environment.MachineName ?? string.Empty;
            loginPack.Describe = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().Describe;
            loginPack.ProcessorCount = Environment.ProcessorCount;
            loginPack.ProcessorInfo = GetSystemInforHelper.GetMyCpuInfo;
            loginPack.MemorySize = GetSystemInforHelper.GetMyMemorySize;
            loginPack.RunTime = StartParameter.RunTime;
            loginPack.ServiceVison = StartParameter.Version;
            loginPack.UserName = Environment.UserName.ToString();
            loginPack.OSVersion = GetSystemInforHelper.GetOSFullName;
            loginPack.GroupName = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().GroupName;
            loginPack.ExistCameraDevice = GetSystemInforHelper.ExistCameraDevice();
            loginPack.ExitsRecordDevice = GetSystemInforHelper.ExistRecordDevice();
            loginPack.ExitsPlayerDevice = GetSystemInforHelper.ExistPlayDevice();
            loginPack.IdentifyId = StartParameter.IdentifyId;
            session.SendTo(MessageHead.C_MAIN_LOGIN, loginPack);
        }
    }
}