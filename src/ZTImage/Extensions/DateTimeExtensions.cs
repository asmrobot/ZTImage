using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 转为时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Int64 ToTimeStamp(this DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        private const Int64 DAY_SECOND = 86400;
        private const Int64 UTC_PRE_SECOND = 8 * 60 * 60;//北京时间比UTC时间多的秒数
        /// <summary>
        /// 转为UTC时间当天00:00:00的时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Int64 ToDayTimeStamp(this DateTime date)
        {
            Int64 t = date.ToTimeStamp();
            return (t - t % DAY_SECOND);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Int64 ToTimeStampMilliseconds(this DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalMilliseconds);
        }




    }
}
