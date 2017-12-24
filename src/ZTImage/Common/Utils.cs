using ZTImage.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;



namespace ZTImage
{


    /// <summary>
    /// 日期中的节
    /// </summary>
    public enum DateTimeSection
    { 
        Year,
        Month,
        Week,
        Day,
        Hour,
        Minute,
        Second
    }
    /// <summary>
    /// 常用工具类
    /// </summary>
    public class Utils
    {
        public static double Ceiling(decimal val, int bit)
        {
            return decimal.ToDouble(Math.Round(val, bit));
        }

        public static double Ceiling(double val, int bit)
        {
            return Math.Round(val, bit);
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            return Valid.IsNumericArray(strNumber);
        }

        /// <summary>
        /// 初始化数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] InitNumberArray<T>(T[] arr, T value) where T : struct
        {
            if (arr != null && arr.Length > 0)
            {
                return arr;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
            return arr;
        }

        /// <summary>
        /// 是否在数组中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool InNumberArray(Int32[] arr, Int32 value) 
        {
            if (arr == null || arr.Length<=0)
            {
                return false;
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (value == arr[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 修正时间为一个时间段
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="state">0:年,1:月,2:日,3:时，4:分</param>
        public static void RequireDateTime(ref DateTime start, ref DateTime end, DateTimeSection state)
        {
            start = RequireDateTime(start, state);
            end = RequireDateTime(end, state);
            
            switch (state)
            { 
                case DateTimeSection.Year :
                    end=end.AddYears(1);
                    break;
                case DateTimeSection.Month :
                    end=end.AddMonths(1);
                    break;
                case DateTimeSection.Week:
                    end = end.AddDays (7);
                    break;
                case DateTimeSection .Day :
                    end=end.AddDays(1);
                    break;
                case DateTimeSection .Hour :
                    end=end.AddHours(1);
                    break;
                case   DateTimeSection.Minute :
                    end=end.AddMinutes(1);
                    break;
                case DateTimeSection .Second :
                    end=end.AddSeconds(1);
                    break;
                default :
                    start = DateTime.Now.AddDays(-1);
                    end = DateTime.Now;
                    break;
            }
            end=end.AddSeconds(-1);
        }

        /// <summary>
        /// 修正时间至某一节，保留那一节
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static DateTime RequireDateTime(DateTime dt, DateTimeSection state)
        {
            switch (state)
            {
                case DateTimeSection.Year:
                    return new DateTime(dt.Year, 1, 1, 0, 0, 0, dt.Kind);
                case DateTimeSection.Month:
                    return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind);
                case DateTimeSection.Week:
                    dt = dt.AddDays(-(int)dt.DayOfWeek);
                    return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind);
                case DateTimeSection.Day:
                    return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind);
                case DateTimeSection.Hour:
                    return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, dt.Kind);
                case DateTimeSection.Minute:
                    return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
                case DateTimeSection.Second:
                    return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Kind);
                default:
                    return dt;
            }
        }

        /// <summary>
        /// 连接数组中的字符串，中间用unionchar连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="unionChar"></param>
        /// <returns></returns>
        public static string ConcatString(string[] arr, string unionChar,string preChar="",string afterChar="")
        {
            if (arr == null || arr.Length <= 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                builder.Append(preChar+arr[i]+afterChar);
                if (i != (arr.Length - 1))
                {
                    builder.Append(unionChar);
                }
            }
            return builder.ToString();
        }


        /// <summary>
        /// 连接数组中的字符串，中间用unionchar连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="unionChar"></param>
        /// <returns></returns>
        public static string ConcatString<T>(T[] arr, string unionChar, string preChar = "", string afterChar = "")
        {
            if (arr == null || arr.Length <= 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                builder.Append(preChar + arr[i].ToString () + afterChar);
                if (i != (arr.Length - 1))
                {
                    builder.Append(unionChar);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 连接数组中的字符串，中间用unionchar连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="unionChar"></param>
        /// <returns></returns>
        public static string ConcatString(long[] arr, string unionChar, string preChar = "", string afterChar = "")
        {
            if (arr == null || arr.Length <= 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                builder.Append(preChar + arr[i].ToString () + afterChar);
                if (i != (arr.Length - 1))
                {
                    builder.Append(unionChar);
                }
            }
            return builder.ToString();
        }


        /// <summary>
        /// 连接数组中的字符串，中间用unionchar连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="unionChar"></param>
        /// <returns></returns>
        public static string ConcatString(double?[] arr, string unionChar, string preChar = "", string afterChar = "",string defaultnull="null")
        {
            if (arr == null || arr.Length <= 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].HasValue)
                {
                    builder.Append(preChar + arr[i] + afterChar);
                }
                else
                {
                    builder.Append(preChar + defaultnull + afterChar);
                }
                

                if (i != (arr.Length - 1))
                {
                    builder.Append(unionChar);
                }
            }
            return builder.ToString();
        }


        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] userip = Utils.SplitString(ip, @".");

            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.SplitString(iparray[ipIndex], @".");
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

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!Utils.StrIsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

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
        /// 判断字符串是否为空或者为空串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == string.Empty)
            {
                return true;
            }
            return false;
        }


        

        /// <summary>
        /// 判断是否为ASCII字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsASCII(string str)
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
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        

        /// <summary>
        /// 是否中文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinaChar(string str)
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


        public static string SubStr(string str,int leng,bool addPoint=false)
        {
            string tagetstring = "";
            int index=0;
            while(leng >0)
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
                    
                    tagetstring +=c;
                }
                else
                {
                    break;
                }
            }
            return tagetstring;
        }





        



        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
           return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 通过经纬度计算距离
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }





        

    }

    

}
