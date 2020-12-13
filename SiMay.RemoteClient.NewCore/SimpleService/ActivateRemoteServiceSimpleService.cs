using SiMay.Basic;
using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using SiMay.Platform.Windows.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.Service.Core
{
    public class ActivateRemoteServiceSimpleService : RemoteSimpleServiceBase
    {
        [PacketHandler(MessageHead.S_SIMPLE_ACTIVATE_REMOTE_SERVICE)]
        public void ActivateApplicationService(SessionProviderContext session)
        {
            var activateServiceRequest = session.GetMessageEntity<ActivateRemoteServicePacket>();
            string applicationKey = activateServiceRequest.CommandText.Split('.').Last<string>();

            //获取当前消息发送源主控端标识
            long accessId = session.GetAccessId();
            var context = SysUtil.RemoteServiceTypes.FirstOrDefault(x => x.RemoteServiceKey.Equals(applicationKey));
            if (!context.IsNull())
            {
                var serviceName = context.RemoteServiceType.GetCustomAttribute<ServiceNameAttribute>(true);
                SystemMessageNotify.ShowTip($"正在进行远程操作:{(serviceName.IsNull() ? context.RemoteServiceKey : serviceName.Name) }");
                var applicationService = Activator.CreateInstance(context.RemoteServiceType, null) as ApplicationRemoteServiceBase;
                applicationService.ApplicationKey = context.RemoteServiceKey;
                applicationService.ActivatedCommandText = activateServiceRequest.CommandText;
                applicationService.AccessId = accessId;
                applicationService.StartParameter = activateServiceRequest.StartParameter;
                GlobalMessageBus.Publish(BusTopic.CREATE_SERVICE_POST_TO_SEQUENCE, applicationService);
            }
        }
    }
}
