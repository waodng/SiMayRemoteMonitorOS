using SiMay.Net.SessionProvider;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.RemoteMonitor
{
    public class BasicApplication
    {
        public static async Task OpenFileTransportApplication(SessionProviderContext session, IApplication application, string title, string remoteRoot, string[] localPhysicsPath)
        {
            var keys = typeof(FileTransportApplication).GetActivateApplicationKey();
            var simpleApplication = SimpleApplicationHelper.SimpleApplicationCollection.GetSimpleApplication<ActivateRemoteServiceSimpleApplication>();
            foreach (var key in keys)
                await simpleApplication.RemoteActivateService(session, key, remoteRoot, string.Join(",", localPhysicsPath), application.GetType().FullName, title);
        }
    }
}
