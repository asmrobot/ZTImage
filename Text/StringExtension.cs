using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZTImage.Text
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        #region 文本操作
        /// <summary>
        /// 得到指定分隔符左边的内容,没有则返回空串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetRightPart(this string content,char separator)
        {
            if (content.LastIndexOf(separator) > -1)
            {
                return content.Substring(content.LastIndexOf(separator) + 1);
            }
            return string.Empty;
        }


        /// <summary>
        /// 得到多个分隔符之一右侧内容,没有则返回空串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetRightPart(this string content, char[] separator)
        {
            for (int i = 0; i < separator.Length; i++)
            {
                if (content.LastIndexOf(separator[i]) > -1)
                {
                    return content.Substring(content.LastIndexOf(separator[i]) + 1);
                }
            }
            return content;
        }


        /// <summary>
        /// 移除指定分隔符右侧内容,返回移除后的内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <param name="removeSeparator"></param>
        /// <returns></returns>
        public static string RemoveRightPart(this string content, char separator, bool removeSeparator)
        {
            if (content.LastIndexOf(separator) > -1)
            {
                if (removeSeparator)
                {
                    content = content.Substring(0, content.LastIndexOf(separator));
                }
                else
                {
                    content = content.Substring(0, content.LastIndexOf(separator)+1);
                }
                
            }
            return content;
        }



        /// <summary>
        /// 得到分隔符左侧内容，如果没有则返回空串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetLeftPart(this string content, char separator)
        {
            if (content.IndexOf(separator) > -1)
            {
                return content.Substring(0, content.IndexOf(separator));
            }
            return string.Empty;
        }


        /// <summary>
        /// 得到多个分隔符之一左侧内容,没有则返回空串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetLeftPart(this string content, char[] separator)
        {
            for (int i = 0; i < separator.Length; i++)
            {
                if (content.IndexOf(separator[i]) > -1)
                {
                    return content.Substring(0,content.LastIndexOf(separator[i]));
                }
            }
            return content;
        }


        /// <summary>
        /// 移除指定分隔符左侧内容，返回移除后的字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="separator"></param>
        /// <param name="removeSeparator"></param>
        /// <returns></returns>
        public static string RemoveLeftPart(this string content, char separator, bool removeSeparator)
        {
            if (content.LastIndexOf(separator) > -1)
            {
                if (removeSeparator)
                {
                    content = content.Substring( content.LastIndexOf(separator)+1);
                }
                else
                {
                    content = content.Substring(content.LastIndexOf(separator));
                }

            }
            return content;
        }
        #endregion

        #region 判断相关
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string content)
        {
            return string.IsNullOrEmpty(content);
        }
        #endregion

        #region 转换操作
        /// <summary>
        /// 转化成Int数组，是否事务化处理
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str, bool trans)
        {
            string[] sInt = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> ints = new List<int>();
            int temp = 0;
            for (int i = 0, len = sInt.Length; i < len; i++)
            {
                if (sInt[i] == "0")
                {
                    ints.Add(0);
                }
                else
                {
                    temp = sInt[i].ToInt(0);
                    if (temp == 0 && trans)
                    {
                        return new int[0];
                    }
                    ints.Add(temp);
                }
            }
            return ints.ToArray();
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(this string str,int defaultVal=0)
        {
            return ZTImage.TypeConverter.StringToInt(str, defaultVal);
        }



        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ToFloat(this string strValue, float defaultVal = 0)
        {
            return ZTImage.TypeConverter.StringToFloat(strValue, defaultVal);
        }
        #endregion

        
    }
}
