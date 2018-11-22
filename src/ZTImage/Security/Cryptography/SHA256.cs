using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZTImage.Security.Cryptography
{

    /// <summary>
    /// SHA256加密
    /// 默认utf-8
    /// </summary>
    public class SHA256
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            return Encrypt(str, Encoding.Default);

        }
        
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">使用此编码得到字符串的二进制数组</param>
        /// <returns></returns>
        public static string Encrypt(string str, Encoding encoding)
        {
            return Encrypt(encoding.GetBytes(str));
        }

        
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] bytes)
        {
            System.Security.Cryptography.SHA256 sha256 = new SHA256Managed();
            byte[] tmpByte = sha256.ComputeHash(bytes);
            sha256.Clear();
            string outString = string.Empty;
            for (int i = 0; i < tmpByte.Length; i++)
            {
                outString += tmpByte[i].ToString("x2");
            }
            return outString;
        }

    }
}
