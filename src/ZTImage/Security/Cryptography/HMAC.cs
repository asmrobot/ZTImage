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
    public class HMAC
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string value,string key)
        {
            return Encrypt(value, key, Encoding.UTF8);
        }


        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string value,string key,Encoding code)
        {
            //使用SHA1的HMAC
            SYSHMAC hmac = HMACSHA1.Create();
            hmac.Key = code.GetBytes(key);
            byte[] hash = hmac.ComputeHash(code.GetBytes(value));
            return hash;
        }

       
    }
}
