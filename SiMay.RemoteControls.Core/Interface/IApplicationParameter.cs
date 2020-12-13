using System;
using System.Collections.Generic;
using System.Text;

namespace SiMay.RemoteControls.Core
{
    public interface IApplicationParameter
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="arg"></param>
        void SetParameter(object arg);
    }
}
