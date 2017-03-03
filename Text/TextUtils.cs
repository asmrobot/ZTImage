using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

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



        /// <summary>
        /// 文件名格式化
        /// </summary>
        /// <param name="originFileName"></param>
        /// <param name="pathFormat"></param>
        /// <returns></returns>
        public static string FileNameFormat(string originFileName, string pathFormat)
        {
            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{filename}{rand:6}";
            }

            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            originFileName = invalidPattern.Replace(originFileName, "");

            string extension = Path.GetExtension(originFileName);
            string filename = Path.GetFileNameWithoutExtension(originFileName);

            pathFormat = pathFormat.Replace("{filename}", filename);
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat, new MatchEvaluator(delegate (Match match)
            {
                var digit = 6;
                if (match.Groups.Count > 2)
                {
                    digit = Convert.ToInt32(match.Groups[2].Value);
                }
                var rand = new Random();
                return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
            }));

            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));

            return pathFormat + extension;
        }
    }
}
