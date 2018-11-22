using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYSHMAC = System.Security.Cryptography.HMAC;
using System.Security.Cryptography;

namespace ZTImage.Security.Cryptography
{
    /// <summary>
    /// HMAC_SHA1算法
    /// 编码默认utf-8
    /// </summary>
    public class HMACSHA1
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] content, byte[] key)
        {
            //HMACSHA1加密
            System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = key;
            byte[] dataBuffer = content;
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return hashBytes;
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string content, string key, Encoding encoding)
        {
            //HMACSHA1加密
            System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = encoding.GetBytes(key);
            byte[] dataBuffer = encoding.GetBytes(content);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return hashBytes;
        }


    }
}
