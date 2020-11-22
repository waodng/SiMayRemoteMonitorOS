using SiMay.ReflectCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiMay.RemoteService.Loader
{
    public class ServiceOptions : EntitySerializerBase
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// 默认备注
        /// </summary>
        public string DefaultDescrible { get; set; }

        /// <summary>
        /// 默认分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool HideExe { get; set; }

        /// <summary>
        /// 自启动
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// 会话模式
        /// </summary>
        public int SessionMode { get; set; }

        /// <summary>
        /// 连接Key
        /// </summary>
        public int AccessKey { get; set; }

        /// <summary>
        /// 是否互斥
        /// </summary>
        public bool IsMutex { get; set; }

        /// <summary>
        /// 服务启动
        /// </summary>
        public bool InstallService { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务显示名
        /// </summary>
        public string ServiceDisplayName { get; set; }
    }
}
