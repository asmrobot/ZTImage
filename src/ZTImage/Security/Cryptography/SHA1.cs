using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZTImage.Security.Cryptography
{

    /// <summary>
    /// SHA1加密
    /// 默认utf-8
    /// </summary>
    public class SHA1
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            return Encrypt(value, Encoding.Default);
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt(string value,Encoding encoding)
        {
            byte[] sourceDatas = encoding.GetBytes(value);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            sourceDatas = iSHA.ComputeHash(sourceDatas);
            StringBuilder result = new StringBuilder();
            foreach (byte iByte in sourceDatas)
            {
                result.AppendFormat("{0:x2}", iByte);
            }
            return result.ToString();
        }



        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] EncryptToBytes(string value)
        {
            byte[] sourceDatas = Encoding.Default.GetBytes(value);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            byte[] result = iSHA.ComputeHash(sourceDatas);
            return result;
        }

    }
}
