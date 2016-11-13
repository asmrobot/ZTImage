using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ZTImage.Text
{

    public class Coding
    {
        #region  EncodeURI
        /// <summary>
        /// RFC 1738 编码
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncodeURI(string temp, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k;
                switch (t)
                {
                    case "'":
                        t = "%27";
                        builder.Append(t);
                        break;

                    case " ":
                        t = "%20";
                        builder.Append(t);
                        break;

                    case "(":
                        t = "%28";
                        builder.Append(t);
                        break;

                    case ")":
                        t = "%29";
                        builder.Append(t);
                        break;

                    case "!":
                        t = "%21";
                        builder.Append(t);
                        break;

                    case "*":
                        t = "%2A";
                        builder.Append(t);
                        break;

                    default:
                        k = HttpUtility.UrlEncode(t, encoding);

                        if (t == k)
                        {
                            builder.Append(t);
                        }
                        else
                        {
                            builder.Append(k.ToUpper());
                        }
                        break;
                }
            }
            return builder.ToString();
        }

        public static string EncodeURI(string temp)
        {
            return EncodeURI(temp, Encoding.UTF8);
        }

        public static string DecodeURI(string temp, Encoding encoding)
        {
            return HttpUtility.UrlDecode(temp, encoding);
        }

        public static string DecodeURI(string temp)
        {
            return DecodeURI(temp, Encoding.UTF8);
        }


        #endregion


        #region Escape
        public static string Escape(string code, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            int len = code.Length;
            int i = 0;

            string temp = "*@-_+./";
            while (i < len)
            {
                byte[] b = System.Text.Encoding.BigEndianUnicode.GetBytes(code[i].ToString());
                if (b[0] == 0)
                {
                    if (temp.IndexOf(code[i]) > -1 || Char.IsLetterOrDigit(code[i]))
                    {
                        builder.Append(code[i]);
                    }
                    else
                    {
                        builder.Append("%" + b[1].ToString("X"));
                    }

                }
                else
                {
                    builder.Append("%u" + b[0].ToString("X2") + b[1].ToString("X2"));
                }

                i++;
            }
            return builder.ToString();
        }

        public static string Escape(string temp)
        {
            return Escape(temp, Encoding.UTF8 );
        }      


        public static string Unescape(string code, Encoding encoding)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "";
            }
            int len = code.Length;
            int i = 0;
            StringBuilder builder = new StringBuilder();
            while (i < len)
            {
                if (code[i] == '%')
                {
                    if (code[i + 1] == 'u' || code[i + 1] == 'U')
                    {
                        //Unicode
                        code.Substring(i + 2, 4);
                        byte[] t = new byte[] { Convert.ToByte(code.Substring(i + 2, 2), 16), Convert.ToByte(code.Substring(i + 4, 2), 16) };
                        builder.Append(System.Text.UnicodeEncoding.BigEndianUnicode.GetString(t));
                        i += 6;
                    }
                    else
                    {
                        //普通字符
                        builder.Append(Uri.HexUnescape(code, ref i));
                    }
                }
                else
                {
                    builder.Append(code[i]);
                    i++;
                }
            }
            return builder.ToString();
        }

        public static string Unescape(string temp)
        {
            return Unescape(temp,Encoding.UTF8 );
        }

        #endregion


        #region Base64
        public static string EncodeBase64(string value)
        {
            return EncodeBase64(value, Encoding.UTF8);
        }

        public static string EncodeBase64(string value, Encoding code)
        {
            byte[] bytes = code.GetBytes(value);
            return EncodeBase64(bytes);
        }

        public static string EncodeBase64(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static string DecodeBase64(string value)
        {
            return DecodeBase64(value, Encoding.UTF8);
        }

        public static string DecodeBase64(string value, Encoding code)
        {
            byte[] outputb = Convert.FromBase64String(value);
            return code.GetString(outputb);
        }
        #endregion
    }
}
