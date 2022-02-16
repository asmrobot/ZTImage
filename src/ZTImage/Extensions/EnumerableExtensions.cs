using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="split"></param>
        /// <param name="concatNull"></param>
        /// <returns></returns>
        public static string Concat<T>(this IEnumerable<T> values,string split="",bool concatNull=false)
        {
            if (values == null)
            {
                return string.Empty;
            }

            string builder = string.Empty;
            foreach (var item in values)
            {
                if (!concatNull && item == null)
                {
                    continue;
                }
                if (builder != string.Empty)
                {
                    builder += split;
                }
                builder += item.ToString();
            }

            return builder;
        }

        /// <summary>
        /// 转为十六进制字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] values)
        {
            if (values == null || values.Length < 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                builder.Append("0x" + values[i].ToString("X2"));
                if (i < values.Length - 1)
                {
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }



        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(this string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }

            if (strNumber.Length < 1)
            {
                return false;
            }

            foreach (string id in strNumber)
            {
                if (!id.IsNumeric())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
