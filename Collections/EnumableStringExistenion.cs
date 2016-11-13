using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Collections
{
    public static class EnumableStringExistenion
    {
        #region 字符串连接

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="enumer"></param>
        /// <param name="func"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string Concat<TSource>(this IEnumerable<TSource> enumer, Func<TSource, string> func, string sperator = "")
        {
            if (enumer == null)
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();
            bool tag = false;
            foreach (TSource item in enumer)
            {
                if (tag)
                {
                    builder.Append(sperator);
                }

                builder.Append(func(item));
                tag = true;
            }
            return builder.ToString();
        }


        /// <summary>
        /// 是否存在字符串 
        /// </summary>
        /// <param name="enumer"></param>
        /// <param name="val"></param>
        /// <param name="igore"></param>
        /// <returns></returns>
        public static bool Has(this IEnumerable<string> enumer, string val, bool igore = true)
        {
            if (enumer == null || enumer.Count() <= 0)
            {
                return false;
            }

            if (igore)
            {
                val = val.ToUpper();
            }
            foreach (var item in enumer)
            {
                var v = item;
                if (igore)
                {
                    v = item.ToUpper();
                }

                if (v == val)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


    }
}
