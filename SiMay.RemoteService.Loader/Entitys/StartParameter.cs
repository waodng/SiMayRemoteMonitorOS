using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiMay.RemoteService.Loader
{
    public class StartParameter
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string IdentifyId { get; set; }

        /// <summary>
        /// 远程服务器主机名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 远程服务器端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string DefaultDescribe { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 自启动
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// 隐藏文件
        /// </summary>
        public bool HideExe { get; set; }

        /// <summary>
        /// 会话模式
        /// </summary>
        public int SessionMode { get; set; }


        /// <summary>
        /// 连接Key
        /// </summary>
        public long AccessKey { get; set; }

        /// <summary>
        /// 是否互斥
        /// </summary>
        public bool IsMutex { get; set; }

        /// <summary>
        /// 服务版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime RunTime { get; set; }

        /// <summary>
        /// 系统服务启动
        /// </summary>
        public bool AutoService { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务显示名称
        /// </summary>
        public string ServiceDisplayName { get; set; }

        /// <summary>
        /// 是否服务启动
        /// </summary>
        public bool SystemPermission { get; set; }
    }
}
