using Newtonsoft.Json;
using SiMay.Basic;
using SiMay.RemoteControls.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.RemoteMonitor
{
    //    private Dictionary<string, string> _defaultConfig = new Dictionary<string, string>()
    //    {
    //        { nameof(IPAddress) ,"0.0.0.0" },
    //        { "Port" , "5200" },
    //        { "ConnectPassWord", "5200" },
    //        { "MaxConnectCount", "100000" },
    //        { "Maximize", "false" },
    //        { "lHosts", "127.0.0.1:5200" },
    //        { "DbClickViewExc", "" },
    //        { "LockPassWord", "5200" },
    //        { "WindowsIsLock", "false" },
    //        { "DesktopRefreshInterval", "1500" },
    //        { "AudioSamplesPerSecond", "8000" },
    //        { "AudioBitsPerSample", "16" },
    //        { "AudioChannels", "1" },
    //        { "SessionMode", "0" },
    //        { "ServiceIPAddress", "127.0.0.1" },
    //        { "ServicePort", "522" },
    //        { "EnabledCarousel", "true" },
    //        { "CarouselInterval", "5000" },
    //        { "ViewColumn", "4" },
    //        { "ViewRow", "3" },
    //        { "EnabledAnonyMous", "true" },
    //        { "AccessId" , "123456789" },
    //        { "MainAppAccessKey", "5200" },
    //        { "AccessKey", "5200" }
    //    };
    public class SystemAppConfig : AppConfiguration
    {

        public bool WindowMaximize { get; set; }

        public List<string> HistoryCreateHosting { get; set; } = new List<string>();

        public List<string> AutoDesktopViewList { get; set; } = new List<string>();

        public string DbClickViewExc { get; set; }

        public string UnLockeCredential { get; set; }

        public bool Haslock { get; set; }

        public int DesktopViewRefreshInterval { get; set; } = 1500;

        public short AudioSamplesPerSecond { get; set; } = 8000;

        public short AudioBitsPerSample { get; set; } = 16;

        public short AudioChannels { get; set; } = 1;

        public bool CarouselEnabled { get; set; }

        public int CarouselInterval { get; set; } = 5000;

        public int ViewColumn { get; set; } = 4;

        public int ViewRow { get; set; } = 3;

        public async void Flush()
        {
            if (File.Exists(SysConstantsExtend.ConfigPath))
                File.Delete(SysConstantsExtend.ConfigPath);
            var configJson = JsonConvert.SerializeObject(this);
            using (StreamWriter fs = new StreamWriter(SysConstantsExtend.ConfigPath, true))
                await fs.WriteAsync(configJson);
        }

    }
}
