using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Configuration
{
    /// <summary>
    /// 配置缓存帮助类
    /// </summary>
    internal class ConfigCacheHelper
    {
        public Object Config
        {
            get;
            private set;
        }

        public DateTime LastCacheTime
        {
            get;
            private set;
        }

        public ConfigCacheHelper(DateTime dt, Object config)
        {
            this.Config = config;
            this.LastCacheTime = dt;
        }
    }
}
