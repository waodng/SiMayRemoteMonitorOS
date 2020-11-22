using SiMay.Basic;
using System.Collections.Generic;
using System;
using System.IO;
using SiMay.Core;

namespace SiMay.RemoteControls.Core
{
    public class AppConfiguration
    {
        /// <summary>
        /// 服务器监听地址
        /// </summary>
        public string IPAddress { get; set; } = "0.0.0.0";

        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; set; } = 5200;

        /// <summary>
        /// 主控端连接验证密码
        /// </summary>
        public string ValidatePassWord { get; set; } = "5200";

        /// <summary>
        /// 最大连接等待数量
        /// </summary>
        public int MaxConnectCount { get; set; } = 0;

        /// <summary>
        /// 会话模式
        /// </summary>
        public int SessionMode { get; set; } = 0;

        /// <summary>
        /// 主控端表示
        /// </summary>
        public long AccessId { get; set; } = 123456789;

        /// <summary>
        /// 中间服务登录Key
        /// </summary>
        public long AccessKey { get; set; } = 5200;

        /// <summary>
        /// 中间代理服务地址
        /// </summary>
        public string MiddlerProxyIPAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// 中间代理服务端口
        /// </summary>
        public int MiddlerProxyPort { get; set; } = 520;

        /// <summary>
        /// 登录中间服务是否匿名
        /// </summary>
        public bool EnabledAnonyMous { get; set; } = true;


        static object _application = default;
        public static T GetApplicationConfiguration<T>()
            where T : AppConfiguration, new()
        {
            return _application.ConvertTo<T>();
        }

        public static void SetOption<T>(T appConfiguration)
            where T : AppConfiguration, new()
            => _application = appConfiguration;
    }
}