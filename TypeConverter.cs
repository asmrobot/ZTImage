using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace ZTImage
{
    public sealed class TypeConverter
    {
        /// <summary>
        /// 每天的秒数
        /// </summary>
        public const Int64 DaySecond = 86400;

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="Tenum"></typeparam>
        /// <param name="enu"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Tenum StringToEnum<Tenum>(string enu, Tenum defaultValue) where Tenum :struct
        {
            Tenum en;
            if (Enum.TryParse<Tenum>(enu,true, out en))
            {
                return en;
            }
            return defaultValue;
        }


        /// <summary>
        /// int转枚举
        /// </summary>
        /// <typeparam name="Tenum"></typeparam>
        /// <param name="enu"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Tenum IntToEnum<Tenum>(int enu, Tenum defaultValue) where Tenum : struct
        {
            Tenum en;
            if (Enum.TryParse<Tenum>(enu.ToString (), out en))
            {
                return en;
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] values)
        {
            if (values == null || values.Length < 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                builder.Append("0x" + values[i].ToString("X2"));
                if (i < values.Length-1)
                {
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }

        public static string ToUnicode(string input)
        {
            MatchCollection mCollection2 = Regex.Matches(input, "([\\w]+)|(\\\\u([\\w]{4}))");
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
                return input;
            }
        }



        public static byte ObjectToByte(object expression)
        {
            byte b;
            try
            {
                b = Convert.ToByte(expression);
            }
            catch
            {
                b = (byte)0;
            }
            return b;
        }

        

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StringToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                expression=expression.ToUpper();
                if(expression =="TRUE" || expression=="YES" || expression=="OK")
                {
                    return true ;
                }
                else if(expression=="FALSE")
                {
                    return false ;
                }
            }
            return defValue;
        }


        

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StringToInt(expression.ToString(), defValue);

            return defValue;
        }


        public static decimal ObjectToDecimal(object expression)
        {
            return ObjectToDecimal(expression, 0M);
        }

        public static decimal ObjectToDecimal(object expression,decimal defValue)
        {
            if (expression != null)
            {
                return StringToDecimal(expression.ToString(), defValue);
            }
            return defValue;
        }



        public static decimal StringToDecimal(string str)
        {
            return StringToDecimal(str, 0M);
        }

        public static decimal StringToDecimal(string str, decimal defaultVal)
        {
            decimal val = 0M;
            if (Decimal.TryParse(str, out val))
            {
                return val;
            }
            return defaultVal;
            
        }
        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StringToInt(string str)
        {
            return StringToInt(str, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StringToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(StringToFloat(str, defValue));
        }



        /// <summary>
        /// 将对象转换为Byte类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static byte StringToByte(string str, byte defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 4 )
                return defValue;

            byte rv;
            if (Byte.TryParse(str, out rv))
                return rv;
            return defValue;
        }

        /// <summary>
        /// 字符串转Int数组
        /// </summary>
        /// <param name="str">源字符串,中间以","号分隔</param>
        /// <param name="trans">是否事务处理</param>
        /// <returns></returns>
        public static int[] StringToIntArray(string str, bool trans)
        {
            string[] sInt = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> ints = new List<int>();
            int temp = 0;
            for (int i = 0, len = sInt.Length; i < len; i++)
            {
                if (sInt[i] == "0")
                {
                    ints .Add ( 0);
                }
                else
                {
                    temp = StringToInt(sInt[i], 0);
                    if (temp == 0 && trans)
                    { 
                        return new int[0];
                    }
                    ints.Add(temp);
                }
            }
            return ints.ToArray();
        }



        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static long StringToLong(string str, long defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 20 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            long rv;
            if (long.TryParse(str, out rv))
                return rv;

            return defValue;
        }


        /// <summary>
        /// 将对象转换为Long类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Long类型结果</returns>
        public static long ObjectToLong(object obj, long defValue)
        {
            if (obj != null)
            {
                return StringToLong(obj.ToString(), defValue);
            }
            else
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将对象转换为Double类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Double类型结果</returns>
        public static double ObjectToDouble(object obj, double defValue)
        {
            if (obj != null)
            {

                return StringToDouble(obj.ToString(), defValue);
            }
            else
            {
                return defValue;
            }
        }


        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StringToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StringToFloat(object strValue)
        {
            if ((strValue == null))
                return 0;

            return StringToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StringToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StringToDate(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StringToDate(string str)
        {
            return StringToDate(str, DateTime.Now);
        }

        public static DateTime? StringToNullableDate(string str)
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
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDate(object obj)
        {
            return StringToDate(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDate(object obj, DateTime defValue)
        {
            if (obj == null)
            {
                obj = "";
            }
            return StringToDate(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为布尔值
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值 </param>
        /// <returns></returns>
        public static bool ObjectToBool(object obj, bool defValue)
        {
            if (obj == null)
            {
                return defValue;
            }

            return StringToBool(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为布尔值,不为空为真
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns></returns>
        public static bool ObjectToBool(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return StringToBool(obj.ToString(), false);
        }

        /// <summary>
        /// 整型转布尔值,大于0为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IntToBool(int value)
        {
            return value > 0 ? true : false;
        }


        /// <summary>
        /// 转换时间为unix时间戳
        /// </summary>
        /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static long DateToLong(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                date=date.ToUniversalTime();
            }

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        public static long DateToDayLong(DateTime date)
        {
            Int64 t = DateToLong(date);
            return (t - t % DaySecond);
        }



        /// <summary>
        /// 把时间戳转化为DateTime类型(UTC时间)
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime LongToDate(long date)
        {
            DateTime orgin = new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
            return orgin.AddSeconds(date);
        }



        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double StringToDouble(string str)
        {
            return StringToDouble(str, 0.00);
        }

        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double StringToDouble(string str, double defaultval)
        {
            double d = 0.00;
            if (double.TryParse(str, out d))
            {
                return d;
            }
            return defaultval;
        }



        /// <summary>
        /// IP地址转字符串
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static string IPToString(long ipaddress)
        {
            long sip1 = ipaddress / 256 / 256 / 256;
            long sip12 = sip1 * 256 * 256 * 256;
            long sip2 = (ipaddress - sip12) / 256 / 256;
            long sip3 = (ipaddress - sip12 - sip2 * 256 * 256) / 256;
            long sip4 = ipaddress % 256;

            return sip1 + "." + sip2 + "." + sip3 + "." + sip4;
        }

        /// <summary>
        /// IP地址转字符串
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static string NetworkIPToString(long ipaddress)
        {
            long sip1 = ipaddress / 256 / 256 / 256;
            long sip12 = sip1 * 256 * 256 * 256;
            long sip2 = (ipaddress - sip12) / 256 / 256;
            long sip3 = (ipaddress - sip12 - sip2 * 256 * 256) / 256;
            long sip4 = ipaddress % 256;

            
            return sip4 + "." + sip3 + "." + sip2 + "." + sip1;
        }

        /// <summary>
        /// IP地址变换为IP数值格式
        /// </summary>
        /// <param name="ipStr"></param>
        /// <returns></returns>
        public static long StringToIP(string ipStr)
        {
            string[] iparr = ipStr.Split(new char[] {'.' }, StringSplitOptions.RemoveEmptyEntries);
            if (iparr.Length < 4)
            {
                return 0;
            }

            long[] sip = new long[4];
            for (int i = 0; i < 4; i++)
            {
                if (!long.TryParse(iparr[i], out sip[i]))
                {
                    return 0;
                }
            }

            return ((sip[0] * 256 + sip[1]) * 256 + sip[2]) * 256 + sip[3];
        }

        /// <summary>
        /// IP地址变换为IP数值格式
        /// </summary>
        /// <param name="ipStr"></param>
        /// <returns></returns>
        public static long StringToNetworkIP(string ipStr)
        {
            string[] iparr = ipStr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (iparr.Length < 4)
            {
                return 0;
            }

            long[] sip = new long[4];
            for (int i = 0; i < 4; i++)
            {
                if (!long.TryParse(iparr[i], out sip[i]))
                {
                    return 0;
                }
            }

            return ((sip[3] * 256 + sip[2]) * 256 + sip[1]) * 256 + sip[0];
        }

        /// <summary>
        /// 月份转英文，月数从1开始
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static String GetEnMonth(int month)
        {
            if (month <= 0 || month > 12)
            {
                month = 1;
            }
            string[] en_month = { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return en_month[month];
        }

        /// <summary>
        /// 月份转中文，月数从1开始
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetCnMonth(int month)
        {
            return "";
        }

        /// <summary>
        /// 天转英文，天从0开始
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static String GetEnDay(int day)
        {
            if (day <= 0 || day > 31)
            {
                day = 1;
            }
            string[] en_day = { "", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th", "11th", "12th", "13th", "14th", "15th", "16th", "17th", "18th", "19th", "20th", "21th", "22th", "23th", "24th", "25th", "26th", "27th", "28th", "29th", "30th", "31th" };
            return en_day[day];
        }

        /// <summary>
        /// 天转中文，天从0开始
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetCnDay(int day)
        {
            return "";
        }



        /// <summary>
        /// 字符串转GUID
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Guid? StringToGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            Guid v;
            if (Guid.TryParse(value, out v))
            {
                return v;
            }
            return null;
        }
        

        /// <summary>
        /// 对象转GUID
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Guid ObjectToGuid(object value)
        {
            if (value == null)
            {
                return Guid.Empty;
            }
            Guid v;
            if (Guid.TryParse(value.ToString(), out v))
            {
                return v;
            }
            return Guid.Empty;
        }



    }
}
