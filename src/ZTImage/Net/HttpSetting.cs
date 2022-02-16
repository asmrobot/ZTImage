using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Net
{
    /// <summary>
    /// http请求配置
    /// </summary>
    public class HttpSetting
    {
        public static readonly HttpSetting Default = new HttpSetting() { 
            TimeoutMillSecond=15000
        };


        /// <summary>
        /// 超时，毫秒
        /// </summary>
        public Int32 TimeoutMillSecond { get; set; }
    }
}
