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
    public class SHA256
    {
        public static string Encrypt(string str)
        {
            return Encrypt(str, Encoding.ASCII);

        }
        
        public static string Encrypt(string str, Encoding encoding)
        {
            return Encrypt(encoding.GetBytes(str));
        }

        public static string Encrypt(byte[] bytes)
        {
            byte[] tmpByte;
            SHA256Managed sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(bytes);
            sha256.Clear();
            return System.Text.Encoding.ASCII.GetString(tmpByte);
        }

    }
}
