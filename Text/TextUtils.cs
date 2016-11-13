using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Text
{
    public class TextUtils
    {
        #region Random
        private const string randomChars= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 得到随机字符
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetRandomText(string chars, int count)
        {
            int lenght = NumberUtils.GetRandomNumber(count, count);
            var output = new StringBuilder(lenght);

            for (int i = 0; i < lenght; i++)
            {
                int randomIndex = NumberUtils.GetRandomNumber(chars.Length - 1);
                output.Append(chars[randomIndex]);
            }

            return output.ToString();
        }

        /// <summary>
        /// 得到随机字符
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetAnyRandomText(int count)
        {

            var output = new StringBuilder(count);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = NumberUtils.GetRandomNumber(randomChars.Length - 1);
                output.Append(randomChars[randomIndex]);
            }

            return output.ToString();
        }


        /// <summary>
        /// 得到随机字符
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetAnyRandomNumber(int count)
        {

            var output = new StringBuilder(count);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = NumberUtils.GetRandomNumber(9);
                output.Append(randomIndex.ToString ());
            }

            return output.ToString();
        }
        #endregion


        #region Helper
        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="baseChar"></param>
        /// <param name="repeatCount"></param>
        /// <returns></returns>
        public static string RepeatString(string baseChar, int repeatCount)
        {
            if (string.IsNullOrEmpty(baseChar))
            {
                return string.Empty;
            }
            string retString = string.Empty;
            for (int i = 0; i < repeatCount; i++)
            {
                retString += baseChar;
            }
            return retString;
        }
        #endregion
    }
}
