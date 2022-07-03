using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
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


    }
}
