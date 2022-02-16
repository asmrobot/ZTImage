using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Extensions
{
    public static class Int32Extensions
    {
        /// <summary>
        /// 整型转布尔值,大于0为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this int value)
        {
            return value > 0 ? true : false;
        }

        /// <summary>
        /// int转枚举
        /// </summary>
        /// <typeparam name="Tenum"></typeparam>
        /// <param name="enu"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Tenum ToEnum<Tenum>(this int enu, Tenum defaultValue) where Tenum : struct
        {
            Tenum en;
            if (Enum.TryParse<Tenum>(enu.ToString(), out en))
            {
                return en;
            }
            return defaultValue;
        }

        internal static char[] ZTCODE_KEYs = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public static string ToZTCode(this Int32 num)
        {
            string ret = "";
            UInt32 _n = (UInt32)num;
            do
            {
                uint mod = _n % 36;
                ret = ZTCODE_KEYs[mod] + ret;
                _n = _n / 36;

            }
            while (_n != 0);
            return ret;
        }

    }
}
