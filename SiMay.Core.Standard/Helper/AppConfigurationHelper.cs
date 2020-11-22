using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiMay.Core
{
    public class AppConfigurationHelper<T>
    {

        static T _configuration = default;
        public static T AppConfiguration => _configuration;

        public static void SetOption(T appConfiguration)
            => _configuration = appConfiguration;

    }
}
