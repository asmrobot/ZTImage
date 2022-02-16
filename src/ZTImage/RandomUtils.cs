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
    public class RandomUtils
    {
        private static readonly byte[] Randb = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

    





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
