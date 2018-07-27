using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] bytedata)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytedata);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }

        public static string EncryptFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }

       

    }
}
