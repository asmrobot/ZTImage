using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Extensions
{
    public static class Int64Extensions
    {
        private const Int64 MAX_SECOND = 253402272000;

        /// <summary>
        /// 把时间戳转化为DateTime类型(UTC时间)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long date, DateTime defVal)
        {
            if (date > MAX_SECOND)
            {
                return defVal;
            }
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(date);
        }

        /// <summary>
        /// 把时间戳转化为DateTime类型(UTC时间)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long date)
        {
            if (date > MAX_SECOND)
            {
                throw new Exception("超出Unix时间戳能表示的最大秒数");
            }
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(date);
        }

        /// <summary>
        /// IP地址转字符串
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static string ToIPString(this long ipaddress)
        {
            long sip1 = ipaddress / 256 / 256 / 256;
            long sip12 = sip1 * 256 * 256 * 256;
            long sip2 = (ipaddress - sip12) / 256 / 256;
            long sip3 = (ipaddress - sip12 - sip2 * 256 * 256) / 256;
            long sip4 = ipaddress % 256;

            return sip1 + "." + sip2 + "." + sip3 + "." + sip4;
        }
    }
}
