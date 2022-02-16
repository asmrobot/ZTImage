using ZTImage.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;




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

        private const double EARTH_RADIUS = 6378.137;//地球半径
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
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
