using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Security.Cryptography
{

    /// <summary>
    /// MD5加密
    /// 默认utf-8
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            return Encrypt(value, Encoding.UTF8);
        }


        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Encrypt(string value, Encoding code)
        {
            byte[] b = code.GetBytes(value);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            ret = ret.ToLower();
            return ret;
        }
    }
}
