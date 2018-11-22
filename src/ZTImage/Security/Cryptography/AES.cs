using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Security.Cryptography
{
    /// <summary>
    /// AES加解密
    /// </summary>
    public class AES
    {
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="content">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static byte[] Encrypt(string content, string key)
        {
            return Encrypt(content, key, Encoding.Default);
        }
        
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="content">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static byte[] Encrypt(string content, string key,Encoding encoding)
        {
            if (string.IsNullOrEmpty(content)) return null;
            Byte[] source = encoding.GetBytes(content);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = encoding.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(source, 0, source.Length);
            return resultArray;
        }




        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="base64Content">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static string Decrypt(string base64Content, string key)
        {
            return Decrypt(base64Content, key, Encoding.Default);
        }


        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="base64Content">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string Decrypt(string base64Content, string key,Encoding encoding)
        {
            if (string.IsNullOrEmpty(base64Content)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(base64Content);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = encoding.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return encoding.GetString(resultArray);
        }
    }
}
