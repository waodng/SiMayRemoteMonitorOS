using Newtonsoft.Json;
using SiMay.Basic;
using SiMay.Core;
using SiMay.RemoteService.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SiMay.Service.Core
{
    public class AppConfiguration
    {
        /// <summary>
        /// 分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 启动参数
        /// </summary>
        [JsonIgnore]
        public StartParameter StartParameter { get; set; }

        static object _application = default;
        public static T GetApplicationConfiguration<T>()
            where T : AppConfiguration, new()
        {
            return _application.ConvertTo<T>();
        }

        public static void SetOption<T>(T appConfiguration)
            where T : AppConfiguration, new()
            => _application = appConfiguration;

        public void Flush()
        {
            var configJson = JsonConvert.SerializeObject(this);
            AppConfigRegValueHelper.SetValue("SiMayConfig", configJson);
        }
    }
}
