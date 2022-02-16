using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ZTImage
{
    /// <summary>
    /// 字符串处理扩展
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="baseChar"></param>
        /// <param name="repeatCount"></param>
        /// <returns></returns>
        public static string Repeat(this string baseChar, int repeatCount)
        {
            if (string.IsNullOrEmpty(baseChar))
            {
                return string.Empty;
            }
            string retString = string.Empty;
            for (int i = 0; i < repeatCount; i++)
            {
                retString += baseChar;
            }
            return retString;
        }


        /// <summary>
        /// 文件名格式化
        /// </summary>
        /// <param name="originFileName"></param>
        /// <param name="pathFormat"></param>
        /// <returns></returns>
        public static string FormatFileName(this string originFileName, string pathFormat)
        {
            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{filename}{rand:6}";
            }

            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            originFileName = invalidPattern.Replace(originFileName, "");

            string extension = Path.GetExtension(originFileName);
            string filename = Path.GetFileNameWithoutExtension(originFileName);

            pathFormat = pathFormat.Replace("{filename}", filename);
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat, new MatchEvaluator(delegate (Match match)
            {
                var digit = 6;
                if (match.Groups.Count > 2)
                {
                    digit = Convert.ToInt32(match.Groups[2].Value);
                }
                var rand = new Random();
                return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
            }));

            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));

            return pathFormat + extension;
        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] Split(this string strContent, string strSplit)
        {
            if (!strContent.IsEmpty())
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    return new string[] { strContent };
                }

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] Split(this string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = strContent.Split(strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }


        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileExists(this string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// 截取中文长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="leng"></param>
        /// <param name="addPoint"></param>
        /// <returns></returns>
        public static string SubChineseString(this string str, int leng, bool addPoint = false)
        {
            string tagetstring = "";
            int index = 0;
            while (leng > 0)
            {
                if (str.Length > index)
                {
                    string c = str.Substring(index, 1);
                    index++;
                    if (IsChinaChar(c))
                    {
                        leng--;
                    }
                    leng--;

                    tagetstring += c;
                }
                else
                {
                    break;
                }
            }
            return tagetstring;
        }


        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(this string ip, string[] iparray)
        {
            string[] userip = ip.Split( @".");

            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = iparray[ipIndex].Split( @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                        return true;

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                            r++;
                        else
                            break;
                    }
                    else
                        break;
                }

                if (r == 4)
                    return true;
            }
            return false;
        }

        #region 基础类型转换

        internal static Dictionary<char, int> ZTCODE_NUMBER = new Dictionary<char, int>() { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'a', 10 }, { 'b', 11 }, { 'c', 12 }, { 'd', 13 }, { 'e', 14 }, { 'f', 15 }, { 'g', 16 }, { 'h', 17 }, { 'i', 18 }, { 'j', 19 }, { 'k', 20 }, { 'l', 21 }, { 'm', 22 }, { 'n', 23 }, { 'o', 24 }, { 'p', 25 }, { 'q', 26 }, { 'r', 27 }, { 's', 28 }, { 't', 29 }, { 'u', 30 }, { 'v', 31 }, { 'w', 32 }, { 'x', 33 }, { 'y', 34 }, { 'z', 35 } };
        
        /// <summary>
        /// ztcode转为int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int32 ZTCodeToInt32(this string str)
        {
            uint ret = 0;
            for (int i = 0, len = str.Length; i < len; i++)
            {
                checked
                {
                    ret *= 36;
                    ret += (UInt32)ZTCODE_NUMBER[str[i]];
                }
            }

            return (int)ret;
        }



        /// <summary>
        /// 汉字数字转阿拉伯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChineseNumberToInt(string str)
        {
            Dictionary<char, char> dic = new Dictionary<char, char>();
            dic.Add('零', '0');
            dic.Add('一', '1');
            dic.Add('二', '2');
            dic.Add('三', '3');
            dic.Add('四', '4');
            dic.Add('五', '5');
            dic.Add('六', '6');
            dic.Add('七', '7');
            dic.Add('八', '8');
            dic.Add('九', '9');

            foreach (KeyValuePair<char, char> k in dic)
            {
                str = str.Replace(k.Key, k.Value);
            }

            if (str.StartsWith("十"))
            {
                str = "1" + str;
            }

            str = str.Replace("十区", "十0区");
            str = str.Replace("十服", "十0服");

            str = str.Replace("十", "");
            str = str.Replace("百", "");



            return str;
        }

        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicode(this string str)
        {
            MatchCollection mCollection2 = Regex.Matches(str, "([\\w]+)|(\\\\u([\\w]{4}))");
            if (mCollection2 != null && mCollection2.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Match m2 in mCollection2)
                {
                    string v = m2.Value;
                    if (v.StartsWith("\\u"))
                    {
                        string word = v.Substring(2);
                        byte[] codes = new byte[2];
                        int code = System.Convert.ToInt32(word.Substring(0, 2), 16);
                        int code2 = System.Convert.ToInt32(word.Substring(2), 16);
                        codes[0] = (byte)code2;
                        codes[1] = (byte)code;
                        sb.Append(Encoding.Unicode.GetString(codes));
                    }
                    else
                    {
                        sb.Append(v);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return str;
            }
        }


        /// <summary>
        /// 转枚举
        /// </summary>
        /// <typeparam name="Tenum"></typeparam>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Tenum ToEnum<Tenum>(this string str, Tenum defValue) where Tenum : struct
        {
            Tenum en;
            if (Enum.TryParse<Tenum>(str, true, out en))
            {
                return en;
            }
            return defValue;
        }

        /// <summary>
        /// 换为bool型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ToBool(this string str)
        {
            if (str != null)
            {
                str = str.ToUpper();
                if (str == "TRUE" || str == "YES" || str == "OK")
                {
                    return true;
                }
                else if (str == "FALSE")
                {
                    return false;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string str, bool defValue)
        {
            return str.ToBool() ?? defValue;
        }

        /// <summary>
        /// 转换为Byte类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Byte? ToByte(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 4)
                return null;

            byte rv;
            if (Byte.TryParse(str, out rv))
            {
                return rv;
            }
                
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Byte ToByte(this string str,Byte defValue)
        {
            return str.ToByte() ?? defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int32? ToInt32(this string str)
        {
            if (string.IsNullOrEmpty(str) || 
                str.Trim().Length >= 11 || 
                !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return null;

            int rv;
            if (Int32.TryParse(str, out rv))
            {
                return rv;
            }


            return null;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt32(this string str, int defValue)
        {
            return str.ToInt32() ?? defValue;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64? ToInt64(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 20 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                return null;
            }

            long rv;
            if (long.TryParse(str, out rv))
            {
                return rv;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int64 ToInt64(this string str, Int64 defValue)
        {
            return str.ToInt64() ?? defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static UInt64? ToUInt64(this string str)
        {
            UInt64 v = 0;
            if (UInt64.TryParse(str, out v))
            {
                return v;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this string str,UInt64 defValue)
        {
            return str.ToUInt64() ?? defValue;
        }



        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float? ToFloat(this string str)
        {
            if ((str == null) || (str.Length > 10))
                return null;

            float intValue = 0.0f;
            if (str != null)
            {
                bool isFloat = Regex.IsMatch(str, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isFloat)
                {
                    if (float.TryParse(str, out intValue))
                    {
                        return intValue;
                    }
                }
                    
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string str, float defValue)
        {
            return str.ToFloat() ?? defValue;
        }

        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Double? ToDouble(this string str)
        {
            double d = 0.00;
            if (double.TryParse(str, out d))
            {
                return d;
            }
            return null;
        }

        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Double ToDouble(this string str, double defValue)
        {
            return str.ToDouble() ?? defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Decimal? ToDecimal(this string str)
        {
            decimal val = 0M;
            if (Decimal.TryParse(str, out val))
            {
                return val;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this string str, decimal defValue)
        {
            return str.ToDecimal() ?? defValue;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return null;
        }

        /// <summary>
        /// 转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ToDateTime(this string str, DateTime defValue)
        {
            return str.ToDateTime() ?? defValue;
        }

        /// <summary>
        /// 字符串转GUID
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Guid? ToGuid(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            Guid v;
            if (Guid.TryParse(str, out v))
            {
                return v;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str, Guid defValue)
        {
            return str.ToGuid() ?? defValue;
        }

        /// <summary>
        /// xx.xx.xx.xx形的IP字符串转换long值
        /// </summary>
        /// <param name="ipStr"></param>
        /// <returns></returns>
        public static Int64? IPStringToInt64(this string ipStr)
        {
            string[] iparr = ipStr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (iparr.Length != 4)
            {
                return null;
            }

            long[] sip = new long[4];
            for (int i = 0; i < 4; i++)
            {
                Int64? v=iparr[i].ToInt64();
                if (!long.TryParse(iparr[i], out sip[i]))
                {
                    return 0;
                }
            }

            return ((sip[0] * 256 + sip[1]) * 256 + sip[2]) * 256 + sip[3];
        }


        /// <summary>
        /// 字符串转Int数组
        /// </summary>
        /// <param name="str">源字符串,中间以","号分隔</param>
        /// <param name="isTrans">是否事务处理,即：有一项转换失败，就返回空数组</param>
        /// <returns></returns>
        public static Int32[] ToIntArray(this string str, bool isTrans)
        {
            string[] intItems = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> retArray = new List<int>();
            int? temp = null;
            for (int i = 0, len = intItems.Length; i < len; i++)
            {
                temp = intItems[i].ToInt32();
                if (!temp.HasValue && isTrans)
                {
                    return new int[0];
                }
                retArray.Add(temp.Value);
            }
            return retArray.ToArray();
        }
        #endregion

        #region 验证

        /// <summary>
        /// 是否中文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinaChar(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                string c = str.Substring(i, 1);
                byte[] b = System.Text.Encoding.GetEncoding("GB2312").GetBytes(c);
                if (b.Length == 2)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否为ASCII字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsASCII(this string str)
        {
            foreach (char c in str)
            {
                if (Convert.ToInt32(c) >= 127)
                {
                    return false;
                }

            }
            return true;
        }


        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string content)
        {
            return string.IsNullOrEmpty(content);
        }

        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsEmptyOrWhiteSpace(this string content)
        {
            return string.IsNullOrWhiteSpace(content);
        }





        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(this string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|\*|!|\']");
        }

        /// <summary>
        /// 是否是日期字符串
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsDate(this string dateString)
        {
            Regex reg = new Regex(@"^\d{4}-\d{1,2}-\d{1,2}\s*(\d{1,2}:\d{1,2}(:\d{1,2})?)?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(dateString);
        }

        // 邮箱
        public static bool IsEmail(this string email)
        {
            Regex reg = new Regex(@"^[\w\.\-]{1,32}@[\w\-\.]{1,30}\.[a-z]{2,8}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }

        // 密码
        public static bool IsPassword(this string password)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9]{6,32}$");
            return reg.IsMatch(password);
        }

        // 网址
        public static bool IsWebSite(this string webSite)
        {
            Regex reg = new Regex(@"[\w\s\.\,\(\)\d]{7,256}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(webSite);
        }

        // 天线高度和距离
        public static bool IsData(this string data)
        {
            Regex reg = new Regex(@"^[-+]?((\d{1,8})\.\d{1,2}|\d{1,11})$");
            return reg.IsMatch(data);
        }

        // 组织名称/公司名称
        public static bool IsOrgName(this string input)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9\-\u4E00-\u9FA5\s\(\-\.\@\&\)\（\）\,]{3,100}$");
            return reg.IsMatch(input);
        }

        // 联系人名称
        public static bool IsName(this string input)
        {
            Regex reg = new Regex(@"^[A-Za-z\u4E00-\u9FA5\s]{2,100}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 电话
        public static bool IsPhone(this string input)
        {
            Regex reg = new Regex(@"^[\(\+\d\-\)]{10,25}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        public static bool IsMobilePhone(this string input)
        {
            Regex reg = new Regex(@"^1\d{10,10}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 邮编
        public static bool IsZip(this string input)
        {
            Regex reg = new Regex(@"^[a-z0-9]{6,10}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 城市名称
        public static bool IsCity(this string input)
        {
            Regex reg = new Regex(@"^[a-z0-9\s]{2,40}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 传真
        public static bool IsFax(this string input)
        {
            Regex reg = new Regex(@"^[\(\+\d\-\)]{10,25}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 地址
        public static bool IsAddress(this string input)
        {
            Regex reg = new Regex(@"^[\(\w\s\-\u4E00-\u9FA5\-\,\.\)\@]{4,256}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 是否为ip
        public static bool IsIP(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsIPSect(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }


        public static bool IsUserIDList(string ids)
        {
            return Regex.IsMatch(ids, @"^(\d{1,},?){1,}$");
        }
        #endregion

        #region 繁简体转换

        private const string simpPYStr = "啊阿埃挨哎唉哀皑癌蔼矮艾碍爱隘鞍氨安俺按暗岸胺案肮昂盎凹敖熬翱袄傲奥懊澳芭捌扒叭吧笆八疤巴拔跋靶把耙坝霸罢爸白柏百摆佰败拜稗斑班搬扳般颁板版扮拌伴瓣半办绊邦帮梆榜膀绑棒磅蚌镑傍谤苞胞包褒剥薄雹保堡饱宝抱报暴豹鲍爆杯碑悲卑北辈背贝钡倍狈备惫焙被奔苯本笨崩绷甭泵蹦迸逼鼻比鄙笔彼碧蓖蔽毕毙毖币庇痹闭敝弊必辟壁臂避陛鞭边编贬扁便变卞辨辩辫遍标彪膘表鳖憋别瘪彬斌濒滨宾摈兵冰柄丙秉饼炳病并玻菠播拨钵波博勃搏铂箔伯帛舶脖膊渤泊驳捕卜哺补埠不布步簿部怖擦猜裁材才财睬踩采彩菜蔡餐参蚕残惭惨灿苍舱仓沧藏操糙槽曹草厕策侧册测层蹭插叉茬茶查碴搽察岔差诧拆柴豺搀掺蝉馋谗缠铲产阐颤昌猖场尝常长偿肠厂敞畅唱倡超抄钞朝嘲潮巢吵炒车扯撤掣彻澈郴臣辰尘晨忱沉陈趁衬撑称城橙成呈乘程惩澄诚承逞骋秤吃痴持匙池迟弛驰耻齿侈尺赤翅斥炽充冲虫崇宠抽酬畴踌稠愁筹仇绸瞅丑臭初出橱厨躇锄雏滁除楚础储矗搐触处揣川穿椽传船喘串疮窗幢床闯创吹炊捶锤垂春椿醇唇淳纯蠢戳绰疵茨磁雌辞慈瓷词此刺赐次聪葱囱匆从丛凑粗醋簇促蹿篡窜摧崔催脆瘁粹淬翠村存寸磋撮搓措挫错搭达答瘩打大呆歹傣戴带殆代贷袋待逮怠耽担丹单郸掸胆旦氮但惮淡诞弹蛋当挡党荡档刀捣蹈倒岛祷导到稻悼道盗德得的蹬灯登等瞪凳邓堤低滴迪敌笛狄涤翟嫡抵底地蒂第帝弟递缔颠掂滇碘点典靛垫电佃甸店惦奠淀殿碉叼雕凋刁掉吊钓调跌爹碟蝶迭谍叠丁盯叮钉顶鼎锭定订丢东冬董懂动栋侗恫冻洞兜抖斗陡豆逗痘都督毒犊独读堵睹赌杜镀肚度渡妒端短锻段断缎堆兑队对墩吨蹲敦顿囤钝盾遁掇哆多夺垛躲朵跺舵剁惰堕蛾峨鹅俄额讹娥恶厄扼遏鄂饿恩而儿耳尔饵洱二贰发罚筏伐乏阀法珐藩帆番翻樊矾钒繁凡烦反返范贩犯饭泛坊芳方肪房防妨仿访纺放菲非啡飞肥匪诽吠肺废沸费芬酚吩氛分纷坟焚汾粉奋份忿愤粪丰封枫蜂峰锋风疯烽逢冯缝讽奉凤佛否夫敷肤孵扶拂辐幅氟符伏俘服浮涪福袱弗甫抚辅俯釜斧脯腑府腐赴副覆赋复傅付阜父腹负富讣附妇缚咐噶嘎该改概钙盖溉干甘杆柑竿肝赶感秆敢赣冈刚钢缸肛纲岗港杠篙皋高膏羔糕搞镐稿告哥歌搁戈鸽胳疙割革葛格蛤阁隔铬个各给根跟耕更庚羹埂耿梗工攻功恭龚供躬公宫弓巩汞拱贡共钩勾沟苟狗垢构购够辜菇咕箍估沽孤姑鼓古蛊骨谷股故顾固雇刮瓜剐寡挂褂乖拐怪棺关官冠观管馆罐惯灌贯光广逛瑰规圭硅归龟闺轨鬼诡癸桂柜跪贵刽辊滚棍锅郭国果裹过哈骸孩海氦亥害骇酣憨邯韩含涵寒函喊罕翰撼捍旱憾悍焊汗汉夯杭航壕嚎豪毫郝好耗号浩呵喝荷菏核禾和何合盒貉阂河涸赫褐鹤贺嘿黑痕很狠恨哼亨横衡恒轰哄烘虹鸿洪宏弘红喉侯猴吼厚候后呼乎忽瑚壶葫胡蝴狐糊湖弧虎唬护互沪户花哗华猾滑画划化话槐徊怀淮坏欢环桓还缓换患唤痪豢焕涣宦幻荒慌黄磺蝗簧皇凰惶煌晃幌恍谎灰挥辉徽恢蛔回毁悔慧卉惠晦贿秽会烩汇讳诲绘荤昏婚魂浑混豁活伙火获或惑霍货祸击圾基机畸稽积箕肌饥迹激讥鸡姬绩缉吉极棘辑籍集及急疾汲即嫉级挤几脊己蓟技冀季伎祭剂悸济寄寂计记既忌际继纪嘉枷夹佳家加荚颊贾甲钾假稼价架驾嫁歼监坚尖笺间煎兼肩艰奸缄茧检柬碱硷拣捡简俭剪减荐槛鉴践贱见键箭件健舰剑饯渐溅涧建僵姜将浆江疆蒋桨奖讲匠酱降蕉椒礁焦胶交郊浇骄娇嚼搅铰矫侥脚狡角饺缴绞剿教酵轿较叫窖揭接皆秸街阶截劫节茎睛晶鲸京惊精粳经井警景颈静境敬镜径痉靖竟竞净炯窘揪究纠玖韭久灸九酒厩救旧臼舅咎就疚鞠拘狙疽居驹菊局咀矩举沮聚拒据巨具距踞锯俱句惧炬剧捐鹃娟倦眷卷绢撅攫抉掘倔爵桔杰捷睫竭洁结解姐戒藉芥界借介疥诫届巾筋斤金今津襟紧锦仅谨进靳晋禁近烬浸尽劲荆兢觉决诀绝均菌钧军君峻俊竣浚郡骏喀咖卡咯开揩楷凯慨刊堪勘坎砍看康慷糠扛抗亢炕考拷烤靠坷苛柯棵磕颗科壳咳可渴克刻客课肯啃垦恳坑吭空恐孔控抠口扣寇枯哭窟苦酷库裤夸垮挎跨胯块筷侩快宽款匡筐狂框矿眶旷况亏盔岿窥葵奎魁傀馈愧溃坤昆捆困括扩廓阔垃拉喇蜡腊辣啦莱来赖蓝婪栏拦篮阑兰澜谰揽览懒缆烂滥琅榔狼廊郎朗浪捞劳牢老佬姥酪烙涝勒乐雷镭蕾磊累儡垒擂肋类泪棱楞冷厘梨犁黎篱狸离漓理李里鲤礼莉荔吏栗丽厉励砾历利傈例俐痢立粒沥隶力璃哩俩联莲连镰廉怜涟帘敛脸链恋炼练粮凉梁粱良两辆量晾亮谅撩聊僚疗燎寥辽潦了撂镣廖料列裂烈劣猎琳林磷霖临邻鳞淋凛赁吝拎玲菱零龄铃伶羚凌灵陵岭领另令溜琉榴硫馏留刘瘤流柳六龙聋咙笼窿隆垄拢陇楼娄搂篓漏陋芦卢颅庐炉掳卤虏鲁麓碌露路赂鹿潞禄录陆戮驴吕铝侣旅履屡缕虑氯律率滤绿峦挛孪滦卵乱掠略抡轮伦仑沦纶论萝螺罗逻锣箩骡裸落洛骆络妈麻玛码蚂马骂嘛吗埋买麦卖迈脉瞒馒蛮满蔓曼慢漫谩芒茫盲氓忙莽猫茅锚毛矛铆卯茂冒帽貌贸么玫枚梅酶霉煤没眉媒镁每美昧寐妹媚门闷们萌蒙檬盟锰猛梦孟眯醚靡糜迷谜弥米秘觅泌蜜密幂棉眠绵冕免勉娩缅面苗描瞄藐秒渺庙妙蔑灭民抿皿敏悯闽明螟鸣铭名命谬摸摹蘑模膜磨摩魔抹末莫墨默沫漠寞陌谋牟某拇牡亩姆母墓暮幕募慕木目睦牧穆拿哪呐钠那娜纳氖乃奶耐奈南男难囊挠脑恼闹淖呢馁内嫩能妮霓倪泥尼拟你匿腻逆溺蔫拈年碾撵捻念娘酿鸟尿捏聂孽啮镊镍涅您柠狞凝宁拧泞牛扭钮纽脓浓农弄奴努怒女暖虐疟挪懦糯诺哦欧鸥殴藕呕偶沤啪趴爬帕怕琶拍排牌徘湃派攀潘盘磐盼畔判叛乓庞旁耪胖抛咆刨炮袍跑泡呸胚培裴赔陪配佩沛喷盆砰抨烹澎彭蓬棚硼篷膨朋鹏捧碰坯砒霹批披劈琵毗啤脾疲皮匹痞僻屁譬篇偏片骗飘漂瓢票撇瞥拼频贫品聘乒坪苹萍平凭瓶评屏坡泼颇婆破魄迫粕剖扑铺仆莆葡菩蒲埔朴圃普浦谱曝瀑期欺栖戚妻七凄漆柒沏其棋奇歧畦崎脐齐旗祈祁骑起岂乞企启契砌器气迄弃汽泣讫掐洽牵扦钎铅千迁签仟谦干黔钱钳前潜遣浅谴堑嵌欠歉枪呛腔羌墙蔷强抢橇锹敲悄桥瞧乔侨巧鞘撬翘峭俏窍切茄且怯窃钦侵亲秦琴勤芹擒禽寝沁青轻氢倾卿清擎晴氰情顷请庆琼穷秋丘邱球求囚酋泅趋区蛆曲躯屈驱渠取娶龋趣去圈颧权醛泉全痊拳犬券劝缺炔瘸却鹊榷确雀裙群然燃冉染瓤壤攘嚷让饶扰绕惹热壬仁人忍韧任认刃妊纫扔仍日戎茸蓉荣融熔溶容绒冗揉柔肉茹蠕儒孺如辱乳汝入褥软阮蕊瑞锐闰润若弱撒洒萨腮鳃塞赛三叁伞散桑嗓丧搔骚扫嫂瑟色涩森僧莎砂杀刹沙纱傻啥煞筛晒珊苫杉山删煽衫闪陕擅赡膳善汕扇缮墒伤商赏晌上尚裳梢捎稍烧芍勺韶少哨邵绍奢赊蛇舌舍赦摄射慑涉社设砷申呻伸身深娠绅神沈审婶甚肾慎渗声生甥牲升绳省盛剩胜圣师失狮施湿诗尸虱十石拾时什食蚀实识史矢使屎驶始式示士世柿事拭誓逝势是嗜噬适仕侍释饰氏市恃室视试收手首守寿授售受瘦兽蔬枢梳殊抒输叔舒淑疏书赎孰熟薯暑曙署蜀黍鼠属术述树束戍竖墅庶数漱恕刷耍摔衰甩帅栓拴霜双爽谁水睡税吮瞬顺舜说硕朔烁斯撕嘶思私司丝死肆寺嗣四伺似饲巳松耸怂颂送宋讼诵搜艘擞嗽苏酥俗素速粟僳塑溯宿诉肃酸蒜算虽隋随绥髓碎岁穗遂隧祟孙损笋蓑梭唆缩琐索锁所塌他它她塔獭挞蹋踏胎苔抬台泰酞太态汰坍摊贪瘫滩坛檀痰潭谭谈坦毯袒碳探叹炭汤塘搪堂棠膛唐糖倘躺淌趟烫掏涛滔绦萄桃逃淘陶讨套特藤腾疼誊梯剔踢锑提题蹄啼体替嚏惕涕剃屉天添填田甜恬舔腆挑条迢眺跳贴铁帖厅听烃汀廷停亭庭挺艇通桐酮瞳同铜彤童桶捅筒统痛偷投头透凸秃突图徒途涂屠土吐兔湍团推颓腿蜕褪退吞屯臀拖托脱鸵陀驮驼椭妥拓唾挖哇蛙洼娃瓦袜歪外豌弯湾玩顽丸烷完碗挽晚皖惋宛婉万腕汪王亡枉网往旺望忘妄威巍微危韦违桅围唯惟为潍维苇萎委伟伪尾纬未蔚味畏胃喂魏位渭谓尉慰卫瘟温蚊文闻纹吻稳紊问嗡翁瓮挝蜗涡窝我斡卧握沃巫呜钨乌污诬屋无芜梧吾吴毋武五捂午舞伍侮坞戊雾晤物勿务悟误昔熙析西硒矽晰嘻吸锡牺稀息希悉膝夕惜熄烯溪汐犀檄袭席习媳喜铣洗系隙戏细瞎虾匣霞辖暇峡侠狭下厦夏吓掀锨先仙鲜纤咸贤衔舷闲涎弦嫌显险现献县腺馅羡宪陷限线相厢镶香箱襄湘乡翔祥详想响享项巷橡像向象萧硝霄削哮嚣销消宵淆晓小孝校肖啸笑效楔些歇蝎鞋协挟携邪斜胁谐写械卸蟹懈泄泻谢屑薪芯锌欣辛新忻心信衅星腥猩惺兴刑型形邢行醒幸杏性姓兄凶胸匈汹雄熊休修羞朽嗅锈秀袖绣墟戌需虚嘘须徐许蓄酗叙旭序畜恤絮婿绪续轩喧宣悬旋玄选癣眩绚靴薛学穴雪血勋熏循旬询寻驯巡殉汛训讯逊迅压押鸦鸭呀丫芽牙蚜崖衙涯雅哑亚讶焉咽阉烟淹盐严研蜒岩延言颜阎炎沿奄掩眼衍演艳堰燕厌砚雁唁彦焰宴谚验殃央鸯秧杨扬佯疡羊洋阳氧仰痒养样漾邀腰妖瑶摇尧遥窑谣姚咬舀药要耀椰噎耶爷野冶也页掖业叶曳腋夜液一壹医揖铱依伊衣颐夷遗移仪胰疑沂宜姨彝椅蚁倚已乙矣以艺抑易邑屹亿役臆逸肄疫亦裔意毅忆义益溢诣议谊译异翼翌绎茵荫因殷音阴姻吟银淫寅饮尹引隐印英樱婴鹰应缨莹萤营荧蝇迎赢盈影颖硬映哟拥佣臃痈庸雍踊蛹咏泳涌永恿勇用幽优悠忧尤由邮铀犹油游酉有友右佑釉诱又幼迂淤于盂榆虞愚舆余俞逾鱼愉渝渔隅予娱雨与屿禹宇语羽玉域芋郁吁遇喻峪御愈欲狱育誉浴寓裕预豫驭鸳渊冤元垣袁原援辕园员圆猿源缘远苑愿怨院曰约越跃钥岳粤月悦阅耘云郧匀陨允运蕴酝晕韵孕匝砸杂栽哉灾宰载再在咱攒暂赞赃脏葬遭糟凿藻枣早澡蚤躁噪造皂灶燥责择则泽贼怎增憎曾赠扎喳渣札轧铡闸眨栅榨咋乍炸诈摘斋宅窄债寨瞻毡詹粘沾盏斩辗崭展蘸栈占战站湛绽樟章彰漳张掌涨杖丈帐账仗胀瘴障招昭找沼赵照罩兆肇召遮折哲蛰辙者锗蔗这浙珍斟真甄砧臻贞针侦枕疹诊震振镇阵蒸挣睁征狰争怔整拯正政帧症郑证芝枝支吱蜘知肢脂汁之织职直植殖执值侄址指止趾只旨纸志挚掷至致置帜峙制智秩稚质炙痔滞治窒中盅忠钟衷终种肿重仲众舟周州洲诌粥轴肘帚咒皱宙昼骤珠株蛛朱猪诸诛逐竹烛煮拄瞩嘱主着柱助蛀贮铸筑住注祝驻抓爪拽专砖转撰赚篆桩庄装妆撞壮状椎锥追赘坠缀谆准捉拙卓桌琢茁酌啄着灼浊兹咨资姿滋淄孜紫仔籽滓子自渍字鬃棕踪宗综总纵邹走奏揍租足卒族祖诅阻组钻纂嘴醉最罪尊遵昨左佐柞做作坐座锕嗳嫒瑷暧霭谙铵鹌媪骜鳌钯呗钣鸨龅鹎贲锛荜哔滗铋筚跸苄缏笾骠飑飙镖镳鳔傧缤槟殡膑镔髌鬓禀饽钹鹁钸骖黪恻锸侪钗冁谄谶蒇忏婵骣觇禅镡伥苌怅阊鲳砗伧谌榇碜龀枨柽铖铛饬鸱铳俦帱雠刍绌蹰钏怆缍鹑辍龊鹚苁骢枞辏撺锉鹾哒鞑骀绐殚赕瘅箪谠砀裆焘镫籴诋谛绨觌镝巅钿癫铫鲷鲽铤铥岽鸫窦渎椟牍笃黩簖怼镦炖趸铎谔垩阏轭锇锷鹗颚颛鳄诶迩铒鸸鲕钫鲂绯镄鲱偾沣凫驸绂绋赙麸鲋鳆钆赅尴擀绀戆睾诰缟锆纥镉颍亘赓绠鲠诟缑觏诂毂钴锢鸪鹄鹘鸹掴诖掼鹳鳏犷匦刿妫桧鲑鳜衮绲鲧埚呙帼椁蝈铪阚绗颉灏颢诃阖蛎黉讧荭闳鲎浒鹕骅桦铧奂缳锾鲩鳇诙荟哕浍缋珲晖诨馄阍钬镬讦诘荠叽哜骥玑觊齑矶羁虿跻霁鲚鲫郏浃铗镓蛲谏缣戋戬睑鹣笕鲣鞯绛缰挢峤鹪鲛疖颌鲒卺荩馑缙赆觐刭泾迳弪胫靓阄鸠鹫讵屦榉飓钜锔窭龃锩镌隽谲珏皲剀垲忾恺铠锴龛闶钪铐骒缂轲钶锞颔龈铿喾郐哙脍狯髋诓诳邝圹纩贶匮蒉愦聩篑阃锟鲲蛴崃徕涞濑赉睐铼癞籁岚榄斓镧褴阆锒唠崂铑铹痨鳓诔缧俪郦坜苈莅蓠呖逦骊缡枥栎轹砺锂鹂疠粝跞雳鲡鳢蔹奁潋琏殓裢裣鲢魉缭钌鹩蔺廪檩辚躏绫棂蛏鲮浏骝绺镏鹨茏泷珑栊胧砻偻蒌喽嵝镂瘘耧蝼髅垆撸噜闾泸渌栌橹轳辂辘氇胪鸬鹭舻鲈脔娈栾鸾銮囵荦猡泺椤脶镙榈褛锊呒唛嬷杩劢缦镘颡鳗麽扪焖懑钔芈谧猕祢渑腼黾缈缪闵缗谟蓦馍殁镆钼铙讷铌鲵辇鲶茑袅陧蘖嗫颟蹑苎咛聍侬哝驽钕傩讴怄瓯蹒疱辔纰罴铍谝骈缥嫔钋镤镨蕲骐绮桤碛颀颃鳍佥荨悭骞缱椠钤嫱樯戗炝锖锵镪羟跄诮谯荞缲硗跷惬锲箧锓揿鲭茕蛱巯赇虮鳅诎岖阒觑鸲诠绻辁铨阕阙悫荛娆桡饪轫嵘蝾缛铷颦蚬飒毵糁缫啬铯穑铩鲨酾讪姗骟钐鳝垧殇觞厍滠畲诜谂渖谥埘莳弑轼贳铈鲥绶摅纾闩铄厮驷缌锶鸶薮馊飕锼谡稣谇荪狲唢睃闼铊鳎钛鲐昙钽锬顸傥饧铴镗韬铽缇鹈阗粜龆鲦恸钭钍抟饨箨鼍娲腽纨绾辋诿帏闱沩涠玮韪炜鲔阌莴龌邬庑怃妩骛鹉鹜饩阋玺觋硖苋莶藓岘猃娴鹇痫蚝籼跹芗饷骧缃飨哓潇骁绡枭箫亵撷绁缬陉荥馐鸺诩顼谖铉镟谑泶鳕埙浔鲟垭娅桠氩厣赝俨兖谳恹闫酽魇餍鼹炀轺鹞鳐靥谒邺晔烨诒呓峄饴怿驿缢轶贻钇镒镱瘗舣铟瘾茔莺萦蓥撄嘤滢潆璎鹦瘿颏罂镛莸铕鱿伛俣谀谕蓣嵛饫阈妪纡觎欤钰鹆鹬龉橼鸢鼋钺郓芸恽愠纭韫殒氲瓒趱錾驵赜啧帻箦谮缯谵诏钊谪辄鹧浈缜桢轸赈祯鸩诤峥钲铮筝骘栉栀轵轾贽鸷蛳絷踬踯觯锺纣绉伫槠铢啭馔颞骓缒诼镯谘缁辎赀眦锱龇鲻偬诹驺鲰镞缵躜鳟讠谫郄勐凼坂垅垴埯埝苘荬荮莜莼菰藁揸吒吣咔咝咴噘噼嚯幞岙嵴彷徼犸狍馀馇馓馕愣憷懔丬溆滟溷漤潴澹甯纟绔绱珉枧桊桉槔橥轱轷赍肷胨飚煳煅熘愍淼砜磙眍钚钷铘铞锃锍锎锏锘锝锪锫锿镅镎镢镥镩镲稆鹋鹛鹱疬疴痖癯裥襁耢颥螨麴鲅鲆鲇鲞鲴鲺鲼鳊鳋鳘鳙鞒鞴齄";
        private const string ftPYStr = "啊阿埃挨哎唉哀皑癌蔼矮艾碍爱隘鞍氨安俺按暗岸胺案肮昂盎凹敖熬翺袄傲奥懊澳芭捌扒叭吧笆八疤巴拔跋靶把耙坝霸罢爸白柏百摆佰败拜稗斑班搬扳般颁板版扮拌伴瓣半办绊邦帮梆榜膀绑棒磅蚌镑傍谤苞胞包褒剥薄雹保堡饱宝抱报暴豹鲍爆杯碑悲卑北辈背贝钡倍狈备惫焙被奔苯本笨崩绷甭泵蹦迸逼鼻比鄙笔彼碧蓖蔽毕毙毖币庇痹闭敝弊必辟壁臂避陛鞭边编贬扁便变卞辨辩辫遍标彪膘表鳖憋别瘪彬斌濒滨宾摈兵冰柄丙秉饼炳病并玻菠播拨钵波博勃搏铂箔伯帛舶脖膊渤泊驳捕卜哺补埠不布步簿部怖擦猜裁材才财睬踩采彩菜蔡餐参蚕残惭惨灿苍舱仓沧藏操糙槽曹草厕策侧册测层蹭插叉茬茶查碴搽察岔差诧拆柴豺搀掺蝉馋谗缠铲产阐颤昌猖场尝常长偿肠厂敞畅唱倡超抄钞朝嘲潮巢吵炒车扯撤掣彻澈郴臣辰尘晨忱沈陈趁衬撑称城橙成呈乘程惩澄诚承逞骋秤吃痴持匙池迟弛驰耻齿侈尺赤翅斥炽充冲虫崇宠抽酬畴踌稠愁筹仇绸瞅丑臭初出橱厨躇锄雏滁除楚础储矗搐触处揣川穿椽传船喘串疮窗幢床闯创吹炊捶锤垂春椿醇唇淳纯蠢戳绰疵茨磁雌辞慈瓷词此刺赐次聪葱囱匆从丛凑粗醋簇促蹿篡窜摧崔催脆瘁粹淬翠村存寸磋撮搓措挫错搭达答瘩打大呆歹傣戴带殆代贷袋待逮怠耽担丹单郸掸胆旦氮但惮淡诞弹蛋当挡党荡档刀捣蹈倒岛祷导到稻悼道盗德得的蹬灯登等瞪凳邓堤低滴迪敌笛狄涤翟嫡抵底地蒂第帝弟递缔颠掂滇碘点典靛垫电佃甸店惦奠淀殿碉叼雕雕刁掉吊钓调跌爹碟蝶叠谍叠丁盯叮钉顶鼎锭定订丢东冬董懂动栋侗恫冻洞兜抖斗陡豆逗痘都督毒犊独读堵睹赌杜镀肚度渡妒端短锻段断缎堆兑队对墩吨蹲敦顿囤钝盾遁掇哆多夺垛躲朵跺舵剁惰堕蛾峨鹅俄额讹娥恶厄扼遏鄂饿恩而儿耳尔饵洱二贰发罚筏伐乏阀法珐藩帆番翻樊矾钒繁凡烦反返范贩犯饭泛坊芳方肪房防妨仿访纺放菲非啡飞肥匪诽吠肺废沸费芬酚吩氛分纷坟焚汾粉奋份忿愤粪丰封枫蜂峰锋风疯烽逢冯缝讽奉凤佛否夫敷肤孵扶拂辐幅氟符伏俘服浮涪福袱弗甫抚辅俯釜斧脯腑府腐赴副覆赋复傅付阜父腹负富讣附妇缚咐噶嘎该改概钙盖溉干甘杆柑竿肝赶感秆敢赣冈刚钢缸肛纲岗港杠篙臯高膏羔糕搞镐稿告哥歌搁戈鸽胳疙割革葛格蛤阁隔铬个各给根跟耕更庚羹埂耿梗工攻功恭龚供躬公宫弓巩汞拱贡共钩勾沟苟狗垢构购够辜菇咕箍估沽孤姑鼓古蛊骨谷股故顾固雇刮瓜剐寡挂褂乖拐怪棺关官冠观管馆罐惯灌贯光广逛瑰规圭矽归龟闺轨鬼诡癸桂柜跪贵刽辊滚棍锅郭国果裹过哈骸孩海氦亥害骇酣憨邯韩含涵寒函喊罕翰撼捍旱憾悍焊汗汉夯杭航壕嚎豪毫郝好耗号浩呵喝荷菏核禾和何合盒貉阂河涸赫褐鹤贺嘿黑痕很狠恨哼亨横衡恒轰哄烘虹鸿洪宏弘红喉侯猴吼厚候后呼乎忽瑚壶葫胡蝴狐糊湖弧虎唬护互沪户花哗华猾滑画划化话槐徊怀淮坏欢环桓还缓换患唤痪豢焕涣宦幻荒慌黄磺蝗簧皇凰惶煌晃幌恍谎灰挥辉徽恢蛔回毁悔慧卉惠晦贿秽会烩汇讳诲绘荤昏婚魂浑混豁活夥火获或惑霍货祸击圾基机畸稽积箕肌饥迹激讥鸡姬绩缉吉极棘辑籍集及急疾汲即嫉级挤几脊己蓟技冀季伎祭剂悸济寄寂计记既忌际继纪嘉枷夹佳家加荚颊贾甲钾假稼价架驾嫁歼监坚尖笺间煎兼肩艰奸缄茧检柬堿硷拣捡简俭剪减荐槛鉴践贱见键箭件健舰剑饯渐溅涧建僵姜将浆江疆蒋桨奖讲匠酱降蕉椒礁焦胶交郊浇骄娇嚼搅铰矫侥脚狡角饺缴绞剿教酵轿较叫窖揭接皆稭街阶截劫节茎睛晶鲸京惊精粳经井警景颈静境敬镜径痉靖竟竞净炯窘揪究纠玖韭久灸九酒厩救旧臼舅咎就疚鞠拘狙疽居驹菊局咀矩举沮聚拒据巨具距踞锯俱句惧炬剧捐鹃娟倦眷卷绢撅攫抉掘倔爵桔杰捷睫竭洁结解姐戒藉芥界借介疥诫届巾筋斤金今津襟紧锦仅谨进靳晋禁近烬浸尽劲荆兢觉决诀绝均菌钧军君峻俊竣浚郡骏喀咖卡咯开揩楷凯慨刊堪勘坎砍看康慷糠扛抗亢炕考拷烤靠坷苛柯棵磕颗科壳咳可渴克刻客课肯啃垦恳坑吭空恐孔控抠口扣寇枯哭窟苦酷库裤夸垮挎跨胯块筷侩快宽款匡筐狂框矿眶旷况亏盔岿窥葵奎魁傀馈愧溃坤昆捆困括扩廓阔垃拉喇蜡腊辣啦莱来赖蓝婪栏拦篮阑兰澜谰揽览懒缆烂滥琅榔狼廊郎朗浪捞劳牢老佬姥酪烙涝勒乐雷镭蕾磊累儡垒擂肋类泪棱楞冷厘梨犁黎篱狸离漓理李里鲤礼莉荔吏栗丽厉励砾历利傈例俐痢立粒沥隶力璃哩俩联莲连镰廉怜涟帘敛脸链恋炼练粮凉梁粱良两辆量晾亮谅撩聊僚疗燎寥辽潦了撂镣廖料列裂烈劣猎琳林磷霖临邻鳞淋凛赁吝拎玲菱零龄铃伶羚淩灵陵岭领另令溜琉榴硫馏留刘瘤流柳六龙聋咙笼窿隆垄拢陇楼娄搂篓漏陋芦卢颅庐炉掳卤虏鲁麓碌露路赂鹿潞禄录陆戮驴吕铝侣旅履屡缕虑氯律率滤绿峦挛孪滦卵乱掠略抡轮伦仑沦纶论萝螺罗逻锣箩骡裸落洛骆络妈麻玛码蚂马骂嘛吗埋买麦卖迈脉瞒馒蛮满蔓曼慢漫谩芒茫盲氓忙莽猫茅锚毛矛铆卯茂冒帽貌贸麽玫枚梅酶霉煤没眉媒镁每美昧寐妹媚门闷们萌蒙檬盟锰猛梦孟眯醚靡糜迷谜弥米秘觅泌蜜密幂棉眠绵冕免勉娩缅面苗描瞄藐秒渺庙妙蔑灭民抿皿敏悯闽明螟鸣铭名命谬摸摹蘑模膜磨摩魔抹末莫墨默沫漠寞陌谋牟某拇牡亩姆母墓暮幕募慕木目睦牧穆拿哪呐钠那娜纳氖乃奶耐奈南男难囊挠脑恼闹淖呢馁内嫩能妮霓倪泥尼拟妳匿腻逆溺蔫拈年碾撵拈念娘酿鸟尿捏聂孽啮镊镍涅您柠狞凝宁拧泞牛扭钮纽脓浓农弄奴努怒女暖虐疟挪懦糯诺哦欧鸥殴藕呕偶沤啪趴爬帕怕琶拍排牌徘湃派攀潘盘磐盼畔判叛乓庞旁耪胖抛咆刨炮袍跑泡呸胚培裴赔陪配佩沛喷盆砰抨烹澎彭蓬棚硼篷膨朋鹏捧碰坯砒霹批披劈琵毗啤脾疲皮匹痞僻屁譬篇偏片骗飘漂瓢票撇瞥拼频贫品聘乒坪苹萍平凭瓶评屏坡泼颇婆破魄迫粕剖扑铺仆莆葡菩蒲埔朴圃普浦谱曝瀑期欺栖戚妻七凄漆柒沏其棋奇歧畦崎脐齐旗祈祁骑起岂乞企启契砌器气迄弃汽泣讫掐洽牵扡釬铅千迁签仟谦干黔钱钳前潜遣浅谴堑嵌欠歉枪呛腔羌墙蔷强抢橇锹敲悄桥瞧乔侨巧鞘撬翘峭俏窍切茄且怯窃钦侵亲秦琴勤芹擒禽寝沁青轻氢倾卿清擎晴氰情顷请庆琼穷秋丘邱球求囚酋泅趋区蛆曲躯屈驱渠取娶龋趣去圈颧权醛泉全痊拳犬券劝缺炔瘸却鹊榷确雀裙群然燃冉染瓤壤攘嚷让饶扰绕惹热壬仁人忍韧任认刃妊纫扔仍日戎茸蓉荣融熔溶容绒冗揉柔肉茹蠕儒孺如辱乳汝入褥软阮蕊瑞锐闰润若弱撒洒萨腮鳃塞赛三三伞散桑嗓丧搔骚扫嫂瑟色涩森僧莎砂杀刹沙纱傻啥煞筛晒珊苫杉山删煽衫闪陜擅赡膳善汕扇缮墒伤商赏晌上尚裳梢捎稍烧芍勺韶少哨邵绍奢赊蛇舌舍赦摄射慑涉社设砷申呻伸身深娠绅神沈审婶甚肾慎渗声生甥牲升绳省盛剩胜圣师失狮施湿诗尸虱十石拾时什食蚀实识史矢使屎驶始式示士世柿事拭誓逝势是嗜噬适仕侍释饰氏市恃室视试收手首守寿授售受瘦兽蔬枢梳殊抒输叔舒淑疏书赎孰熟薯暑曙署蜀黍鼠属术述树束戍竖墅庶数漱恕刷耍摔衰甩帅栓拴霜双爽谁水睡税吮瞬顺舜说硕朔烁斯撕嘶思私司丝死肆寺嗣四伺似饲巳松耸怂颂送宋讼诵搜艘擞嗽苏酥俗素速粟僳塑溯宿诉肃酸蒜算虽隋随绥髓碎岁穗遂隧祟孙损笋蓑梭唆缩琐索锁所塌他它她塔獭挞蹋踏胎苔抬台泰酞太态汰坍摊贪瘫滩坛檀痰潭谭谈坦毯袒碳探叹炭汤塘搪堂棠膛唐糖倘躺淌趟烫掏涛滔绛萄桃逃淘陶讨套特藤腾疼誊梯剔踢锑提题蹄啼体替嚏惕涕剃屉天添填田甜恬舔腆挑条迢眺跳贴铁帖厅听烃汀廷停亭庭挺艇通桐酮瞳同铜彤童桶捅筒统痛偷投头透凸秃突图徒途涂屠土吐兔湍团推颓腿蜕褪退吞屯臀拖托脱鸵陀驮驼椭妥拓唾挖哇蛙洼娃瓦袜歪外豌弯湾玩顽丸烷完碗挽晚皖惋宛婉万腕汪王亡枉网往旺望忘妄威巍微危韦违桅围唯惟为潍维苇萎委伟伪尾纬未蔚味畏胃喂魏位渭谓尉慰卫瘟温蚊文闻纹吻稳紊问嗡翁瓮挝蜗涡窝我斡卧握沃巫呜钨乌污诬屋无芜梧吾吴毋武五捂午舞伍侮坞戊雾晤物勿务悟误昔熙析西硒矽晰嘻吸锡牺稀息希悉膝夕惜熄烯溪汐犀檄袭席习媳喜铣洗系隙戏细瞎虾匣霞辖暇峡侠狭下厦夏吓掀锨先仙鲜纤咸贤衔舷闲涎弦嫌显险现献县腺馅羡宪陷限线相厢镶香箱襄湘乡翔祥详想响享项巷橡像向象萧硝霄削哮嚣销消宵淆晓小孝校肖啸笑效楔些歇蝎鞋协挟携邪斜胁谐写械卸蟹懈泄泻谢屑薪芯锌欣辛新忻心信衅星腥猩惺兴刑型形邢行醒幸杏性姓兄凶胸匈汹雄熊休修羞朽嗅锈秀袖绣墟戌需虚嘘须徐许蓄酗叙旭序畜恤絮婿绪续轩喧宣悬旋玄选癣眩绚靴薛学穴雪血勋熏循旬询寻驯巡殉汛训讯逊迅压押鸦鸭呀丫芽牙蚜崖衙涯雅哑亚讶焉咽阉烟淹盐严研蜒岩延言颜阎炎沿奄掩眼衍演艳堰燕厌砚雁唁彦焰宴谚验殃央鸯秧杨扬佯疡羊洋阳氧仰痒养样漾邀腰妖瑶摇尧遥窑谣姚咬舀药要耀椰噎耶爷野冶也页掖业叶曳腋夜液壹壹医揖铱依伊衣颐夷遗移仪胰疑沂宜姨彜椅蚁倚已乙矣以艺抑易邑屹亿役臆逸肄疫亦裔意毅忆义益溢诣议谊译异翼翌绎茵荫因殷音阴姻吟银淫寅饮尹引隐印英樱婴鹰应缨莹萤营荧蝇迎赢盈影颖硬映哟拥佣臃痈庸雍踊蛹咏泳涌永恿勇用幽优悠忧尤由邮铀犹油游酉有友右佑釉诱又幼迂淤于盂榆虞愚舆余俞逾鱼愉渝渔隅予娱雨与屿禹宇语羽玉域芋郁吁遇喻峪御愈欲狱育誉浴寓裕预豫驭鸳渊冤元垣袁原援辕园员圆猿源缘远苑愿怨院曰约越跃钥岳粤月悦阅耘云郧匀陨允运蕴酝晕韵孕匝砸杂栽哉灾宰载再在咱攒暂赞赃脏葬遭糟凿藻枣早澡蚤躁噪造皂竈燥责择则泽贼怎增憎曾赠扎喳渣札轧铡闸眨栅榨咋乍炸诈摘斋宅窄债寨瞻毡詹粘沾盏斩辗崭展蘸栈占战站湛绽樟章彰漳张掌涨杖丈帐账仗胀瘴障招昭找沼赵照罩兆肇召遮折哲蛰辙者锗蔗这浙珍斟真甄砧臻贞针侦枕疹诊震振镇阵蒸挣睁征狰争怔整拯正政帧症郑证芝枝支吱蜘知肢脂汁之织职直植殖执值侄址指止趾只旨纸志挚掷至致置帜峙制智秩稚质炙痔滞治窒中盅忠钟衷终种肿重仲众舟周州洲诌粥轴肘帚咒皱宙昼骤珠株蛛朱猪诸诛逐竹烛煮拄瞩嘱主着柱助蛀贮铸筑住注祝驻抓爪拽专砖转撰赚篆桩庄装妆撞壮状椎锥追赘坠缀谆准捉拙卓桌琢茁酌啄着灼浊兹咨资姿滋淄孜紫仔籽滓子自渍字鬃棕踪宗综总纵邹走奏揍租足卒族祖诅阻组钻纂嘴醉最罪尊遵昨左佐柞做作坐座锕嗳嫒瑷暧霭谙铵鹌媪骜鳌钯呗钣鸨龅鹎贲锛荜哔滗铋筚跸芐缏笾骠飑飙镖镳鳔傧缤槟殡膑镔髌鬓禀饽钹鹁钸骖黪恻锸侪钗冁谄谶蒇忏婵骣觇禅镡伥苌怅阊鲳砗伧谌榇碜龀枨柽铖铛饬鸱铳俦帱雠刍绌蹰钏怆缍鹑辍龊鹚苁骢枞辏撺锉鹾哒鞑骀绐殚赕瘅箪谠砀裆焘镫籴诋谛绨觌镝巅钿癫铫鲷鲽铤铥岽鸫窦渎椟牍笃黩簖怼镦炖趸铎谔垩阏轭锇锷鹗腭颛鳄诶迩铒鸸鲕钫鲂绯镄鲱偾沣凫驸绂绋赙麸鲋鳆钆赅尴擀绀戆睾诰缟锆纥镉颍亘赓绠鲠诟缑觏诂毂钴锢鸪鹄鹘鸹掴诖掼鹳鳏犷匦刿妫桧鲑鳜衮绲鲧埚呙帼椁蝈铪阚绗颉灏颢诃阖蛎黉讧荭闳鲎浒鹕骅桦铧奂缳锾鲩鳇诙荟哕浍缋珲晖诨馄阍钬镬讦诘荠叽哜骥玑觊齑矶羁虿跻霁鲚鲫郏浃铗镓蛲谏缣戋戬睑鹣笕鲣鞯绛缰挢峤鹪鲛疖颌鲒卺荩馑缙赆觐刭泾迳弪胫靓阄鸠鹫讵屦榉飓钜锔窭龃锩镌隽谲珏皲剀垲忾恺铠锴龛闶钪铐骒缂轲钶锞颔龈铿喾郐哙脍狯髋诓诳邝圹纩贶匮蒉愦聩篑阃锟鲲蛴崃徕涞濑赉睐铼癞籁岚榄斓镧褴阆锒唠崂铑铹痨鳓诔缧俪郦坜苈莅蓠呖逦骊缡枥栎轹砺锂鹂疠粝跞雳鲡鳢蔹奁潋琏殓裢裣鲢魉缭钌鹩蔺廪檩辚躏绫棂蛏鲮浏骝绺镏鹨茏泷珑栊胧砻偻蒌喽嵝镂瘘耧蝼髅垆撸噜闾泸渌栌橹轳辂辘氇胪鸬鹭舻鲈脔娈栾鸾銮囵荦猡泺椤脶镙榈褛锊呒唛嬷杩劢缦镘颡鳗么扪焖懑钔芈谧猕禰渑腼黾缈缪闵缗谟蓦馍殁镆钼铙讷铌鲵辇鲇茑袅陧蘖嗫颟蹑苎咛聍侬哝驽钕傩讴怄瓯蹒疱辔纰罴铍谝骈缥嫔钋镤镨蕲骐绮桤碛颀颃鳍佥荨悭骞缱椠钤嫱樯戗炝锖锵镪羟跄诮谯荞缲硗跷惬锲箧锓揿鲭茕蛱巯赇虮鳅诎岖阒觑鸲诠绻辁铨阕阙悫荛娆桡饪轫嵘蝾缛铷颦蚬飒毵糁缫啬铯穑铩鲨酾讪姗骟钐鳝坰殇觞厍滠畲诜谂沈谥埘莳弑轼贳铈鲥绶摅纾闩铄厮驷缌锶鸶薮馊飕锼谡稣谇荪狲唢脧闼铊鳎钛鲐昙钽锬顸傥饧铴镗韬铽缇鹈阗粜龆鲦恸钭钍抟饨箨鼍娲腽纨绾辋诿帏闱沩涠玮韪炜鲔阌莴龌邬庑怃妩骛鹉鹜饩阋玺觋硖苋莶藓岘猃娴鹇痫蚝籼跹芗饷骧缃飨哓潇骁绡枭箫亵撷绁缬陉荥馐鸺诩顼谖铉镟谑泶鳕埙浔鲟垭娅桠氩厣贗俨兖谳恹闫酽魇餍鼹炀轺鹞鳐靥谒邺晔烨诒呓峄饴怿驿缢轶贻钇镒镱瘗舣铟瘾茔莺萦蓥撄嘤滢潆璎鹦瘿颏罂镛莸铕鱿伛俣谀谕蓣嵛饫阈妪纡觎欤钰鹆鹬龉橼鸢鼋钺郓蕓恽愠纭韫殒氲瓒趱錾驵赜啧帻箦谮缯谵诏钊谪辄鹧浈缜桢轸赈祯鸩诤峥钲铮筝骘栉栀轵轾贽鸷蛳絷踬踯觯钟纣绉伫槠铢啭馔颞骓缒诼镯谘缁辎赀眦锱龇鲻偬诹驺鲰镞缵躜鳟訁谫郤猛氹阪垄垴垵垫檾荬荮蓧蒓菇槁摣咤唚哢咝噅撅劈谑襆嶴脊仿侥犸麅余馇馓饢楞怵懔爿溆滟混滥潴淡宁糸绔绱瑉枧棬案橰橥轱轷賫膁胨飚糊煆溜闵渺砜滚眍钚钷铘铞锃锍锎锏锘锝锪锫锿镅镎镢镥镩镲穭鹋鹛鹱疬屙痖臒襇襁耢颥螨麯鲅鲆鮎鲞鲴鲺鲼鳊鳋鳘鳙鞽韝齇";
        private const string qqPYStr = "娿婀埃挨餀呃哀皑癌蔼婑銰碍嫒隘鞍氨鮟唵洝暗岸胺案肮昻盎凹獓熬翱仸謸奥袄奥妑捌朳朳妑笆仈疤妑菝柭靶妑耙坝覇罢妑皛柏咟摆佰败湃稗癍癍搬扳瘢颁板蝂汾绊柈瓣柈刅绊绑幇梆旁嫎垹蜯嫎蚌镑旁谤苞菢笣褒剥薄雹湺堡怉宝炮蕔懪豹鲍嚗柸碑蕜萆苝辈背赑钡俻狈备惫焙被渀苯夲苯镚绷甭泵嘣逬腷嬶仳啚毣彼碧蓖币滭毙毖币庇痹闭獙弊怭澼壁臂鐴陛鞭笾揙贬碥楩变卞辧辫辫猵摽滮镖表鳖憋莂瘪彬斌濒璸滨摈娦栤窉眪秉饼炳疒并箥菠譒妭钵菠博勃搏铂箔伯帛舶脖膊渤泊訤峬卜誧卟埠芣钸荹簿蔀怖攃猜裁财财财棌棌采埰婇蔡爘傪蛬残慙参灿芲舱仺獊蔵懆鐰槽蓸愺厠憡侧册恻层竲揷紁茬嗏楂楂搽镲岔槎诧拆枈豺搀傪蝉镵谗瀍铲浐阐颤誯猖畼甞瑺苌偿肠厂敞畅晿倡趫莏钞謿謿謿漅訬炒车扯彻掣沏瞮郴烥宸尘曟忱冗陈趁衬撑称峸橙荿珵塖珵惩僜諴承浧骋秤阣痴歭匙肔尺肔肔耻歯侈尺哧趐斥炽茺冲虫漴宠菗絒帱帱婤僽薵仇皗瞅忸溴初炪厨厨躇锄雏蒢篨椘绌储矗搐触处遄巛瑏椽伝船遄賗疮囱幢床闯创欥炊腄腄棰舂椿錞唇錞蒓蠢戥焯疵垐濨雌辞濨瓷词泚剌赐佽聪茐囱茐苁苁凑粗齰簇娖蹿篡窜凗慛慛脆瘁濢濢濢籿洊籿磋撮髊措挫措溚垯答瘩咑汏槑歹傣瀻带殆笩贷袋待曃怠耽泹冄啴郸掸旦旦氮泹惮惔诞弹疍当澢党荡澢叨捣稲箌岛祷导菿稲悼檤盗徳嘚哋簦灯憕等簦凳郰諟彽嘀廸敌廸狄涤翟嫡抵疧哋渧苐渧弚递缔颠掂滇碘点敟靛垫电佃甸扂惦奠淀殿淍汈雕蜩刁鋽铞铞蜩瓞嗲渫渫迭媟疉玎饤汀町嵿鼎锭萣忊丢崬笗蓳慬憅崬侗恫岽狪兠鬦乧跿豆浢哣嘟督毐渎独渎陼睹帾荰镀肚喥喥妒鍴短葮葮断葮碓兑队怼墩沌壿敦顿囤沌盾遁掇哆哆夺垛躱朶跺舵剁媠憜睋睋鹅皒额讹皒悪苊扼遏鄂皒慁洏ル洱尒聂洱②贰泼藅筏浌疺阀珐珐藩泛畨飜樊矾钒瀿泛烦反返笵贩泛粄疺汸淓汸肪房汸妨汸汸汸仿婔悱啡飞萉厞诽吠腓废沸曊棼酚玢氛汾妢坟焚汾帉奋妢忿濆粪仹崶猦蜂峯峯颩疯烽漨溤漨讽唪鳯仏娝玞敷肤孵荴拂辐諨氟苻茯俘棴捊涪湢袱弗甫抚辅椨釜釡脯腑椨腐赴諨覆赋复傅苻阜父腹萯冨讣胕妇缚咐噶嗄姟妀漑钙葢漑迀苷杆柑芉肝迀憾秆噉赣罓碙钢矼釭罁罓港釭禞皋滈膏餻溔镐镐镐哠滒戨搁戈鸽胳疙剨愅噶咯蛤阁隔铬个茖给艮茛畊浭菮羹埂耿梗笁糼糼塨龚栱匑厷営弖巩汞珙贡珙沟芶芶苟豞垢媾媾够辜菇咕箍诂钴箛菇鼔咕蛊嗗唂骰诂顾涸雇剐呱剐寡啩啩乖拐怪菅関菅蒄观涫菅潅遦潅遦洸広迋瑰规圭硅归亀闺匦愧诡癸蓕匮蛫贵刽辊蔉辊煱漷国淉裹过铪骸陔嗨氦亥嗐骇酣憨邯韩浛凾寒凾諴癷翰撼捍猂憾悍猂污汉夯忼航壕嚎濠毫郝恏耗呺滘哬曷嗬菏劾秝啝哬匼盉貉阂菏涸赫褐鹤哿潶嫼痕佷哏悢涥悙横蘅恒轰晎烘渱鸿葓宖宖荭糇糇糇犼厚糇后苸苸唿瑚壶煳箶箶狐煳煳弧唬唬户冱户户埖蕐澕磆磆畵划囮话槐徊怀准坏欢寰桓还缓换漶唤痪豢焕涣宦抝巟巟曂磺蝗簧瑝瑝瑝瑝愰縨恍巟洃媈媈幑恢蛔冋毇珻慧卉惠珻贿秽浍烩汇讳诲浍荤涽殙魂浑婫豁萿钬焱镬戓惑靃货祸击圾樭僟畸稽积箕肌饥迹噭讥鸡姬绩缉咭极棘辑籍潗彶喼疾汲旣嫉级哜凢脊己蓟技冀悸伎祭剂悸哜寄寂计汜旣忌漈继汜嘉枷夹佳家咖荚颊贾曱钾徦糘价泇驾糘姧盬坚尖笺简煎凲肩艰奷缄茧捡柬碱硷拣捡彅倹彅諴荐槛鉴践溅见楗箭件揵舰剑饯渐溅涧踺壃葁将桨茳彊蒋桨奨讲匠酱夅蕉椒礁潐烄茭郊浇娇娇嚼搅铰矫侥脚烄角饺儌烄剿嘋酵轿珓嘂窖揭帹湝秸街阶截劫兯茎聙瞐鲸倞惊棈粳经井檠憬颈静璄擏傹径痉靖獍竞净泂僒啾究纠玖韭玖灸勼氿厩慦旧臼舅咎僦咎鞠佝狙疽剧驹匊局咀怇举沮藂岠琚姖倶岠踞涺倶呴惧岠涺涓鹃涓惓眷卷涓瘚攫决崛崛嚼桔杰啑睫竭洁结解姐悈藉芥鎅徣夰疥诫届凧荕釿唫妗珒噤紧婂仅殣琎靳晋噤菦烬锓浕劲荆兢觉吷吷蕝汮箘呁军焄浚浚浚浚郡浚喀咖鉲咯閞揩揩剀慨刋堪勘坎歃看嫝嵻嵻扛忼囥忼栲洘栲靠坷岢柯锞溘锞萪涜嗑妸渇尅尅愙锞肻肻恳垦妔妔涳恐芤啌抠囗扣簆喖哭崫楛酷厍裤洿垮挎跨胯赽筷侩赽宽窾匡筺诳框纩洭纩况扝盔岿窥葵喹魁傀溃隗溃堒崐涃涃葀拡霩阔柆菈喇腊腊辣菈莱唻攋蓝漤孄拦蓝阑兰澜谰灠灠攋灠灡嚂哴蓈哴蓢蓢蓢烺崂崂窂荖佬粩络络崂嘞泺檑檑檑藞蔂儡垒檑叻类汨棱楞唥厘悡犁黎篱狸蓠漓理李里鲤礼莉荔吏栗婯疠励砾呖悡傈唎俐痢竝粒沥隶劦璃哩唡聅嗹涟镰廉怜涟帘潋脸嗹恋炼炼悢凉梁粱悢俩唡粮凉煷凉嫽窷獠疗獠寥辽潦孒撂镣漻料烮煭烮挘猎啉啉潾霖临邻潾啉凛赁悋柃玪夌蕶龄玪伶玪夌灵夌玪领叧泠媹琉媹硫馏畱嚠媹蓅栁陆泷聋茏茏窿湰泷泷茏溇溇嵝溇屚陋庐卢颅庐炉掳卤虏噜麓碌蕗蕗赂蔍潞禄渌陆戮馿焒焒佀膂履屡缕虑氯侓卛虑渌栾娈孪滦卵乱稤畧囵囵囵仑囵纶囵啰螺啰罗啰罗骡裸落咯咯络妈嫲犸犸犸骉骂嫲嬷埋荬麦卖迈霡慲獌蛮慲嫚嫚嫚嫚谩笀汒吂氓杧漭猫罞锚毝罞铆茆茂萺萺邈贸庅坆枚烸酶莓湈莈葿媒镁烸羙昧寐妺媚閄闷们萠蒙檬擝锰掹梦掹侎醚靡糜洣洣弥洣秘觅泌滵滵幂婂眠婂冕凂勉娩缅媔媌媌媌邈仯缈庿仯篾搣姄抿皿勄悯闽眀螟嘄佲洺掵缪嗼摹嚤嗼嗼嚤嚤嚤沬沬嗼嚜默沬嗼寞帞湈哞湈拇牡亩姆毋募暮募募慕朩朩睦牧穆嗱哪妠妠哪哪妠氖釢艿恧柰遖莮难灢挠悩悩閙淖迡浽禸嫰能妮霓淣狔胒抳沵嫟腻屰溺蔫秥姩碾撵捻淰娘酿茑杘涅嗫糵啮嗫镍涅您柠狞凝苎拧泞犇沑妞狃哝哝哝挵伮怓伮囡暖疟疟挪穤穤喏呃瓯瓯瓯耦呕耦沤啪汃瓟啪啪琶啪棑簰棑湃哌襻沈盘磐昐溿叛判乓厐臱耪眫抛垉铇垉垉垉垉怌胚掊裴婄婄蓜姵沛濆湓泙抨烹澎憉莑堋硼篷膨萠鹏唪湴坯砒噼纰怶噼琵毗啤裨疲怶苉痞僻庇譬萹媥爿骗彯慓瓢嘌潎潎拚频贫板娉乒岼泙泙岼凭甁评屛岥秡櫇嘙岥魄廹粕剖圤舗圤莆匍箁蒲逋圤圃普浦镨曝刨剘剘栖嘁凄⑦凄漆柒沏娸諆渏忮畦崎脐斉旗祈祁骐起岂阣佱晵契砌噐气迄弃汽淇讫拤洽撁扦钎铅芉迁签仟嗛墘黔钱钳湔濳遣浅谴堑嵌芡嗛炝濸腔羌嫱嫱强炝橇锹毃佾乔趭乔乔巧鞘毳趬峭佾窍苆苆苴惬苆钦埐儭蓁噖懄芹檎噙寑沁圊轻氢倾卿凊擎啨氰凊顷埥庆琼穷偢丘邱浗浗囚媨泅趋岖蛆浀躯屈駆渠掫婜龋趣厾圜颧权醛葲洤痊拳吠券勧蒛炔瘸却鹊榷确雀峮羣嘫嘫姌媣瓤壤攘娘让隢扰隢惹慹壬芢亾涊韧姙认刄妊纫扔仍ㄖ戎茸嫆荣瀜嫆嫆嫆绒冗渘渘禸筎蠕濡孺洳媷乳肗叺褥软朊惢瑞锐润润婼弜潵洒蕯腮鳃噻噻彡叁伞潵鎟鎟丧搔騒扫溲瑟脃涩潹僧莏唦摋閷乷纱傻倽繺筛晒姗苫杉屾剼煽钐閁陕擅赡膳僐讪傓缮墒伤啇赏晌仩尙裳哨哨哨烧芍汋韶仯哨卲袑奢赊虵舙舎赦摂射慑渉涻蔎砷妽呻訷裑堔娠訷鉮沈谉婶卙肾慎椮殸泩甥狌圱绳渻墭乗夝圣浉妷浉湤湿诗迉虱拾坧湁溡什喰蚀实识史矢使尸馶始鉽沶仕迣枾倳拭誓迣势湜嗜噬适仕侍释饰氏巿恃厔视鉽荍掱渞垨寿涭售辤痩獣蔬枢梳姝杼瀭埱忬蔋疏书赎孰孰薯濐曙署蜀黍癙属术沭树娕戍竪墅庶薮漱恕唰耍摔缞甩帅拴拴灀叒摤谁渁腄挩吮橓顺橓说硕朔烁凘凘凘偲俬呞咝尸肆峙嗣④伺姒饲巳菘耸怂颂鎹浨讼诵溲艘擞嗽苏酥俗嫊趚粟僳愬溯蹜诉歗酸祘匴虽隋随浽髓谇嵗穗嬘隧祟孙损笋蓑逡逡缩锁鎍鎻葰禢彵咜咜嗒獭挞蹋沓胎苔孡珆溙酞忲忲呔坍摊贪瘫滩墵檀痰憛谭谈钽毯袒湠探叹湠饧溏搪漟橖膛瑭溏倘躺淌趟烫陶涛瑫绦陶洮洮陶陶讨套特駦駦庝誊珶剔踢锑諟趧渧渧軆櫕嚏惕珶珶屟兲婖瑱甶甛恬婖睓狣条迢眺朓萜鉄萜廰厛烃汀侹渟渟侹侹艇嗵秱酮瞳哃恫浵僮硧硧茼统痌偸投头透凸秃湥图徙蒤凃廜汢汢兎湍团蓷颓蹆蜕蹆蹆昋屯臀柂仛脱袉拕駞袉椭鋖沰唾挖哇蛙哇哇咓袜歪迯豌塆塆琓顽丸烷唍涴梚脕皖惋宛啘萭腕忹迋匄忹蛧暀忹望莣妄媙蘶嶶佹韦违桅围惟惟潙潍惟苇崣逶伟沩屗纬沬墛菋嵔媦嵔蘶莅渭媦墛墛衞瘟温螡妏闻鈫沕穏紊问滃暡瓮挝窝煱窉莪斡卧楃沃莁呜钨乌汚莁偓呒芜梧圄呉毋娬伍圄吘橆⑤侮坞戊霚晤粅匢务圄误厝凞唽覀硒矽晰嘻插唶犠浠息唏悉膝汐厝熄烯渓汐犀檄袭席习媳禧铣冼系隙戱细磍虾匣葭辖叚浃浃浃芐厦嗄圷锨锨姺佡鲜汘咸贤衔舷娴涎妶溓显険哯献县腺陥羡宪陥限线楿厢镶萫葙襄湘芗翔祥详想姠啍頙巷潒潒姠潒簘硝霄萷涍嚣销消宵淆哓尒涍校肖啸笑效楔些歇蝎嚡拹挟携峫斜胁喈冩悈衔蟹澥绁泻塮屑蕲芯锌俽厗噺忻杺信衅暒睲睲瑆兴铏侀形郉垳瑆圉莕悻狌凶凶汹匈汹雄熋咻俢馐朽溴琇莠袖绣歔戌濡歔歔湏俆汻蓄酗溆旮垿畜恤絮胥绪续蓒媗媗悬嫙兹选癣妶绚靴薛敩泬膤洫勋熏揗洵咰浔紃廵咰卂训卂逊卂压呷鸦鸭吖吖厊厊蚜崖衙涯蕥哑亚冴漹咽阉烟殗盐严妍蜒啱娫訁颜阎烾沿奄殗眼衍湮滟堰嬿厌砚雁唁彦熖匽谚験殃姎鸯秧昜婸佯疡咩样阳氧昂痒养样羕撽崾岆愮愮尧滛窰愮烑吆舀药婹耀倻噎倻爷嘢冶竾页掖邺旪曳腋液液①壹悘揖铱畩吚扆颐夷遗簃仪胰寲沂宜侇彝掎蚁掎巳乁矣姒兿抑昜邑屹亿役臆逸肄疫洂裔嬑藙忆义谥溢诣议谊译异翼翌绎筃荫洇殷堷隂絪荶檭淫夤飮吚吲陻茚渶璎璎鹰应缨莹萤营荧蝇迊赢盁影颕哽眏哟砽砽臃痈滽澭踊蛹怺怺悀怺恿涌鼡豳沋滺沋尤甴邮铀沋怞游酉洧伖佑佑釉诱叒孧扜菸纡盂榆虞愚舆悇揄揄渔揄揄渔隅予娯雨玙屿禹荢娪羽砡域芋喐吁喁喻峪御匬欲狱唷謍浴寓裕预豫驭鸳棩寃沅垣媴厡瑗辕圎园园猿羱缘逺夗蒝葾阮曰箹樾跞钥捳粤仴哾阅秐囩郧枃殒狁运藴酝晕韵夃匝咂卆酨酨灾宰酨侢茬洎瓒暂瓒賍賍脏糟糟凿藻栆皂璪蚤璪璪慥唣灶璪嫧萚荝泽贼怎熷璔嶒熷紥喳碴札轧铡闸喳栅榨咋咋怍怍擿斋宅榨债寨瞻毡詹秥跕盏斩辗崭蹍蘸栈颭战跕偡绽樟嶂彰漳张礃涨粀扙账账扙胀瘴障妱昭找沼赵燳罩狣肇佋嗻菥悊蛰辙锗锗蔗适淅沴斟嫃甄砧臻浈针浈忱疹沴震桭镇俥篜诤诤姃狰踭姃整拯囸炡帧症郑姃芷汥伎汥倁倁汥脂汥と枳轵矗淔殖秇惪侄歮栺圵趾呮旨只梽挚掷臸臸置帜峙淛潪秩雉质炙痔滞菭窒狆盅筗妕衷蔠种妕偅仲众洀淍酬酬诌粥轴肘帚咒皱宙昼骤咮株咮咮蕏渚诛豩艹烛煑拄瞩瞩炷着炷莇蛀贮铸茿炷炷柷驻抓爪跩抟砖啭撰赚篆桩圧装妆獞匨匨椎锥捶赘坠缀谆痽浞炪婥棹琢茁酌啄着灼浊兹恣粢恣稵淄孜橴仔籽滓ふ洎渍牸鬃琮琮崈琮縂枞邹趉楱楱蒩娖卒蔟袓蒩蒩蒩钻纂觜酔朂嶵澊噂葃咗佐柞莋莋唑蓙锕嗳嫒瑷暧霭谙铵鹌媪骜鳌钯呗钣鸨龅鹎贲锛荜哔滗铋筚跸芐缏笾骠飑飙镖镳鳔傧缤槟殡膑镔髌鬓禀饽钹鹁钸骖黪恻锸侪钗冁谄谶蒇忏婵骣觇禅镡伥苌怅阊鲳砗伧谌榇碜龀枨柽铖铛饬鸱铳俦帱雠刍绌蹰钏怆缍鹑辍龊鹚苁骢枞辏撺锉鹾哒鞑骀绐殚赕瘅箪谠砀裆焘镫籴诋谛绨觌镝巅钿癫铫鲷鲽铤铥岽鸫窦渎椟牍笃黩簖怼镦炖趸铎谔垩阏轭锇锷鹗腭颛鳄诶迩铒鸸鲕钫鲂绯镄鲱偾沣凫驸绂绋赙麸鲋鳆钆赅尴擀绀戆睾诰缟锆纥镉颍亘赓绠鲠诟缑觏诂毂钴锢鸪鹄鹘鸹掴诖掼鹳鳏犷匦刿妫桧鲑鳜衮绲鲧埚呙帼椁蝈铪阚绗颉灏颢诃阖蛎黉讧荭闳鲎浒鹕骅桦铧奂缳锾鲩鳇诙荟哕浍缋珲晖诨馄阍钬镬讦诘荠叽哜骥玑觊齑矶羁虿跻霁鲚鲫郏浃铗镓蛲谏缣戋戬睑鹣笕鲣鞯绛缰挢峤鹪鲛疖颌鲒卺荩馑缙赆觐刭泾迳弪胫靓阄鸠鹫讵屦榉飓钜锔窭龃锩镌隽谲珏皲剀垲忾恺铠锴龛闶钪铐骒缂轲钶锞颔龈铿喾郐哙脍狯髋诓诳邝圹纩贶匮蒉愦聩篑阃锟鲲蛴崃徕涞濑赉睐铼癞籁岚榄斓镧褴阆锒唠崂铑铹痨鳓诔缧俪郦坜苈莅蓠呖逦骊缡枥栎轹砺锂鹂疠粝跞雳鲡鳢蔹奁潋琏殓裢裣鲢魉缭钌鹩蔺廪檩辚躏绫棂蛏鲮浏骝绺镏鹨茏泷珑栊胧砻偻蒌喽嵝镂瘘耧蝼髅垆撸噜闾泸渌栌橹轳辂辘氇胪鸬鹭舻鲈脔娈栾鸾銮囵荦猡泺椤脶镙榈褛锊呒唛嬷杩劢缦镘颡鳗么扪焖懑钔芈谧猕禰渑腼黾缈缪闵缗谟蓦馍殁镆钼铙讷铌鲵辇鲇茑袅陧蘖嗫颟蹑苎咛聍侬哝驽钕傩讴怄瓯蹒疱辔纰罴铍谝骈缥嫔钋镤镨蕲骐绮桤碛颀颃鳍佥荨悭骞缱椠钤嫱樯戗炝锖锵镪羟跄诮谯荞缲硗跷惬锲箧锓揿鲭茕蛱巯赇虮鳅诎岖阒觑鸲诠绻辁铨阕阙悫荛娆桡饪轫嵘蝾缛铷颦蚬飒毵糁缫啬铯穑铩鲨酾讪姗骟钐鳝坰殇觞厍滠畲诜谂沈谥埘莳弑轼贳铈鲥绶摅纾闩铄厮驷缌锶鸶薮馊飕锼谡稣谇荪狲唢脧闼铊鳎钛鲐昙钽锬顸傥饧铴镗韬铽缇鹈阗粜龆鲦恸钭钍抟饨箨鼍娲腽纨绾辋诿帏闱沩涠玮韪炜鲔阌莴龌邬庑怃妩骛鹉鹜饩阋玺觋硖苋莶藓岘猃娴鹇痫蚝籼跹芗饷骧缃飨哓潇骁绡枭箫亵撷绁缬陉荥馐鸺诩顼谖铉镟谑泶鳕埙浔鲟垭娅桠氩厣贗俨兖谳恹闫酽魇餍鼹炀轺鹞鳐靥谒邺晔烨诒呓峄饴怿驿缢轶贻钇镒镱瘗舣铟瘾茔莺萦蓥撄嘤滢潆璎鹦瘿颏罂镛莸铕鱿伛俣谀谕蓣嵛饫阈妪纡觎欤钰鹆鹬龉橼鸢鼋钺郓蕓恽愠纭韫殒氲瓒趱錾驵赜啧帻箦谮缯谵诏钊谪辄鹧浈缜桢轸赈祯鸩诤峥钲铮筝骘栉栀轵轾贽鸷蛳絷踬踯觯钟纣绉伫槠铢啭馔颞骓缒诼镯谘缁辎赀眦锱龇鲻偬诹驺鲰镞缵躜鳟訁谫郤猛氹阪垄垴垵垫檾荬荮蓧蒓菇槁摣咤唚哢咝噅撅劈谑襆嶴脊仿侥犸麅余馇馓饢楞怵懔爿溆滟混滥潴淡宁糸绔绱瑉枧棬案橰橥轱轷賫膁胨飚糊煆溜闵渺砜滚眍钚钷铘铞锃锍锎锏锘锝锪锫锿镅镎镢镥镩镲穭鹋鹛鹱疬屙痖臒襇襁耢颥螨麯鲅鲆鮎鲞鲴鲺鲼鳊鳋鳘鳙鞽韝齇";

        /// <summary>
        /// 转为繁体
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static string ToTraditionalized(this string cc)
        {
            string str = "";
            for (int i = 0; i < cc.Length; i++)
            {
                if (simpPYStr.IndexOf(cc[i]) != -1)
                    str += ftPYStr[(simpPYStr.IndexOf(cc[i]))];
                else if (qqPYStr.IndexOf(cc[i]) != -1)
                    str += ftPYStr[(qqPYStr.IndexOf(cc[i]))];
                else
                    str += cc[i];
            }
            return str;
        }

        /// <summary>
        /// 转为简体
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static string ToSimplized(this string cc)
        {
            string str = "";
            for (int i = 0; i < cc.Length; i++)
            {
                if (ftPYStr.IndexOf(cc[i]) != -1)
                    str += simpPYStr[(ftPYStr.IndexOf(cc[i]))];
                else if (qqPYStr.IndexOf(cc[i]) != -1)
                    str += simpPYStr[(qqPYStr.IndexOf(cc[i]))];
                else
                    str += cc[i];
            }
            return str;
        }
        #endregion

        #region Base64
        public static string EncodeBase64(this string value)
        {
            return EncodeBase64(value, Encoding.UTF8);
        }

        public static string EncodeBase64(this string value, Encoding code)
        {
            byte[] bytes = code.GetBytes(value);
            return bytes.EncodeBase64();
        }

        

        public static string DecodeBase64(this string value)
        {
            return DecodeBase64(value, Encoding.UTF8);
        }

        public static string DecodeBase64(this string value, Encoding code)
        {
            byte[] outputb = Convert.FromBase64String(value);
            return code.GetString(outputb);
        }

        /// <summary>
        /// url安全的base64转化为普通base64字符串
        /// </summary>
        /// <param name="safeBase64"></param>
        /// <returns></returns>
        public static string UrlSafeBase64ToCommonBase64(this string safeBase64)
        {
            string safeb = safeBase64.Replace("-", "+").Replace("_", "/");
            int i = 4 - safeb.Length % 4;
            if (i == 1)
            {
                safeb += "=";
            }
            else if (i == 2)
            {
                safeb += "==";
            }
            return safeb;
        }

        /// <summary>
        /// 普通base64字符串 转化为 url安全的base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string CommonBase64ToUrlSafeBase64(this string base64)
        {
            return base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
        #endregion

        #region Escape
        public static string Escape(this string code, Encoding encoding)
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

        public static string Escape(this string temp)
        {
            return Escape(temp, Encoding.UTF8);
        }


        public static string Unescape(this string code, Encoding encoding)
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

        public static string Unescape(this string temp)
        {
            return Unescape(temp, Encoding.UTF8);
        }

        #endregion

        #region  EncodeURI
        /// <summary>
        /// RFC 1738 编码
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncodeURI(this string temp, Encoding encoding)
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

        public static string EncodeURI(this string temp)
        {
            return EncodeURI(temp, Encoding.UTF8);
        }

        public static string DecodeURI(this string temp, Encoding encoding)
        {
            return HttpUtility.UrlDecode(temp, encoding);
        }

        public static string DecodeURI(this string temp)
        {
            return DecodeURI(temp, Encoding.UTF8);
        }


        #endregion
    }
}
