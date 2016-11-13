using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ZTImage
{
    /// <summary>
    /// 数字操作类
    /// </summary>
    public class NumberUtils
    {
        //大数字码表
        private static char[] codeList = new char[] {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };
        private static Dictionary<char, int> numberList = new Dictionary<char, int>() { {'0',0}, {'1',1}, {'2',2}, {'3',3}, {'4',4}, {'5',5}, {'6',6}, {'7',7}, {'8',8}, {'9',9}, {'a',10}, {'b',11}, {'c',12}, {'d',13}, {'e',14}, {'f',15},{ 'g',16}, {'h',17}, {'i',18}, {'j',19}, {'k',20}, {'l',21}, {'m',22}, {'n',23}, {'o',24}, {'p',25}, {'q',26}, {'r',27}, {'s',28}, {'t',29}, {'u',30}, {'v',31}, {'w',32}, {'x',33}, {'y',34}, {'z',35} };
        private static readonly byte[] Randb = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        #region ZT编码
        /// <summary>
        /// 数字编码成字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetIdentity(int number)
        {
            string ret = "";
            UInt32 _n = (UInt32)number;
            do
            {
                uint mod = _n % 36;
                ret = codeList[mod] + ret;
                _n = _n / 36;

            }
            while (_n != 0);
            return ret;
        }

        /// <summary>
        /// 字符串编码成数字
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int GetNumber(string identity)
        {
            uint ret = 0;
            for (int i = 0, len = identity.Length; i < len; i++)
            {
                checked
                {
                    ret *= 36;
                    ret += (UInt32)numberList[identity[i]];
                }
            }

            return (int)ret;
        }

        #endregion

        #region Helpers
        /// <summary>
        /// 保留round位小数,round为负数时会使整数位变0
        /// </summary>
        /// <param name="d"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string DecimalRoundString(decimal d, int round)
        {
            string s = d.ToString();
            string z = "0000000000000000000000000000";
            s = s.IndexOf(".") > -1 ? s : s + ".";
            int i = round < 0 ? round + s.IndexOf(".") : round + s.IndexOf(".") + 1;
            s = i > 0 ? (i < s.Length ? s.Substring(0, i) + (round < 0 ? z.Substring(0, -round) : "") : s) : "0";
            return decimal.Parse(s).ToString();
        }

        /// <summary>
        /// 汉字数字转阿拉伯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChineseToInt(string str)
        {
            Dictionary<char, char> dic = new Dictionary<char, char>();
            dic.Add('零', '0');
            dic.Add('一', '1');
            dic.Add('二', '2');
            dic.Add('三', '3');
            dic.Add('四', '4');
            dic.Add('五', '5');
            dic.Add('六', '6');
            dic.Add('七', '7');
            dic.Add('八', '8');
            dic.Add('九', '9');

            foreach (KeyValuePair<char, char> k in dic)
            {
                str = str.Replace(k.Key, k.Value);
            }

            if (str.StartsWith("十"))
            {
                str = "1" + str;
            }

            str = str.Replace("十区", "十0区");
            str = str.Replace("十服", "十0服");

            str = str.Replace("十", "");
            str = str.Replace("百", "");



            return str;
        }


        /// <summary>
        /// 舍弃小数位,i为保留的位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double Ceiling(double d, uint i)
        {
            string str = d.ToString();

            int offset = str.IndexOf('.');
            if (offset <= -1)
            {
                return d;
            }

            if ((str.Length - offset - 1) <= i)
            {
                return d;
            }

            int length = offset + (int)i;
            if (i != 0)
            {
                length += 1;
            }
            str = str.Substring(0, length);
            return double.Parse(str);
        }
        #endregion



        #region Random

        /// <summary>
        ///     Generates a positive random number.
        /// </summary>
        private static int GetRandomNumber()
        {
            Rand.GetBytes(Randb);
            int value = BitConverter.ToInt32(Randb, 0);
            return Math.Abs(value);
        }

        /// <summary>
        ///     Generates a positive random number.
        /// </summary>
        public static int GetRandomNumber(int max)
        {
            return GetRandomNumber() % (max + 1);
        }

        /// <summary>
        ///     Generates a positive random number.
        /// </summary>
        public static int GetRandomNumber(int min, int max)
        {
            return GetRandomNumber(max - min) + min;
        }

        #endregion



        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="length">随机码个数</param>
        /// <returns></returns>
        public static string GetAnyRandomNumber(int length)
        {
            int rand;
            string randomcode = String.Empty;

            //生成一定长度的验证码
            System.Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                rand = random.Next(10);
                randomcode += rand.ToString();
            }
            return randomcode;
        }

        

    }
}
