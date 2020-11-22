using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.RemoteMonitor.MainApplication
{
    public class DesktopViewSettingContext
    {
        /// <summary>
        /// 轮播间隔
        /// </summary>
        public int ViewCarouselInterval
        {
            get
            {
                return AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().CarouselInterval;
            }
            set
            {
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().CarouselInterval = value;
            }
        }

        /// <summary>
        /// 视图刷新间隔
        /// </summary>
        public int ViewFreshInterval
        {
            get
            {
                return AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().DesktopViewRefreshInterval;
            }
            set
            {
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().DesktopViewRefreshInterval = value;
            }
        }

        /// <summary>
        /// 视图行
        /// </summary>
        public int ViewRow
        {
            get
            {
                return AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ViewRow;
            }
            set
            {
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ViewRow = value;
            }
        }

        /// <summary>
        /// 视图列
        /// </summary>
        public int ViewColum
        {
            get
            {
                return AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ViewColumn;
            }
            set
            {
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ViewColumn = value;
            }
        }

        /// <summary>
        /// 停留视图
        /// </summary>
        public IList<UDesktopView> AlwaysViews { get; set; }

        /// <summary>
        /// 视图高
        /// </summary>
        public int ViewHeight { get; set; }

        /// <summary>
        /// 视图宽
        /// </summary>
        public int ViewWidth { get; set; }

        /// <summary>
        /// 是否启用轮播
        /// </summary>
        public bool CarouselEnabled
        {
            get
            {
                return AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().CarouselEnabled;
            }
            set
            {
                AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().CarouselEnabled = value;
            }
        }

        public DesktopViewSettingContext()
        {
            AlwaysViews = new List<UDesktopView>();
        }
    }
}
