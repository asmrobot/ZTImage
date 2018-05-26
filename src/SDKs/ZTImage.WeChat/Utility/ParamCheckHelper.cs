using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Utility
{
    public class ParamCheckHelper
    {
        /// <summary>
        /// 空字符验证
        /// </summary>
        public static void NullThrow(string value,string msg)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(msg);
            }
        }

        /// <summary>
        /// 空串验证
        /// </summary>
        public static void WhiteSpaceThrow(string value,string msg)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(msg);
            }
        }

        /// <summary>
        /// 字符串长度限制错误
        /// </summary>
        public static void LimitLengthThrow(string value,Int32 length,string msg)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(msg);
            }

            if (value.Length > length)
            {
                throw new ArgumentOutOfRangeException(msg);
            }
        }
    }
}
