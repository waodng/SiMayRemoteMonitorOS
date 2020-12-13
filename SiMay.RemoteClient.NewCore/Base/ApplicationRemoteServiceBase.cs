using SiMay.Basic;
using SiMay.Core;
using SiMay.ModelBinder;
using SiMay.Net.SessionProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.Service.Core
{
    /// <summary>
    /// 远程应用服务
    /// </summary>
    public abstract class ApplicationRemoteServiceBase : ApplicationProtocolService
    {
        /// <summary>
        /// 当前连接的主控端标识
        /// </summary>
        public long AccessId { get; set; }

        /// <summary>
        /// 服务唯一标识
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// 创建命令
        /// </summary>
        public string ActivatedCommandText { get; set; }

        /// <summary>
        /// 启动参数
        /// </summary>
        public string[] StartParameter { get; set; }

        /// <summary>
        /// 当前会话是否已关闭
        /// </summary>
        public bool WhetherClosed { get; set; } = false;

        [PacketHandler(MessageHead.S_GLOBAL_OK)]
        public void InitializeCompleted(SessionProviderContext session)
        {
            session.SendTo(MessageHead.C_MAIN_ACTIVE_APP,
                new ActivateResponsdApplicationPacket()
                {
                    IdentifyId = AppConfiguration.GetApplicationConfiguration<AppConfiguration>().StartParameter.IdentifyId,
                    ApplicationKey = this.ApplicationKey,
                    ActivatedCommandText = this.ActivatedCommandText,
                    StartParameter = this.StartParameter,
                    OriginName = Environment.MachineName + "@" + AppConfiguration.GetApplicationConfiguration<AppConfiguration>().Describe
                });
            this.SessionInited(session);
        }

        [PacketHandler(MessageHead.S_GLOBAL_ONCLOSE)]
        public void SessionClosed(SessionProviderContext session)
        {
            if (this.WhetherClosed)
                return;
            this.WhetherClosed = true;
            this.CloseSession();
            this.SessionClosed();
            this.HandlerBinder.Dispose();
        }

        public abstract void SessionInited(SessionProviderContext session);

        public abstract void SessionClosed();

        [PacketHandler(MessageHead.S_GLOBAL_SYNC_CALL)]
        public void CallFunctionSync(SessionProviderContext session)
        {
            var callRequest = session.GetMessageEntity<CallSyncPacket>();

            session.CompletedBuffer = callRequest.Datas;

            var targetMessageHead = session.GetMessageHead();

            var operationResult = this.HandlerBinder.CallFunctionPacketHandler(session, targetMessageHead, this, out var returnEntity);
            var syncResultPacket = new CallSyncResultPacket
            {
                Id = callRequest.Id,
                Datas = returnEntity.IsNull() ? Array.Empty<byte>() : SiMay.Serialize.Standard.PacketSerializeHelper.SerializePacket(returnEntity),
                IsOK = operationResult.successed,
                Message = operationResult.ex
            };
            session.SendTo(MessageHead.C_GLOBAL_SYNC_RESULT, syncResultPacket);
        }
    }
}
