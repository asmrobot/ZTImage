using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Reflection.Reflector;

namespace ZTImage
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 换为bool型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool? ObjectToBool(this object obj)
        {
            return obj?.ToString().ToBool();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ObjectToBool(this object obj, bool defValue)
        {
            return obj.ObjectToBool() ?? defValue;
        }

        /// <summary>
        /// 转换为Byte类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Byte? ObjectToByte(this object obj)
        {
            return obj?.ToString().ToByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Byte ObjectToByte(this object obj, Byte defValue)
        {
            return obj.ObjectToByte() ?? defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int32? ObjectToInt32(this object obj)
        {
            return obj?.ToString().ToInt32();
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt32(this object obj, int defValue)
        {
            return obj.ObjectToInt32() ?? defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int64? ObjectToInt64(this object obj)
        {
            return obj?.ToString().ToInt64();
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int64 ObjectToInt64(this object obj, Int64 defValue)
        {
            return obj.ObjectToInt64() ?? defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static UInt64? ObjectToUInt64(this object obj)
        {
            return obj?.ToString().ToUInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static UInt64 ObjectToUInt64(this object obj, UInt64 defValue)
        {
            return obj.ObjectToUInt64() ?? defValue;
        }



        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float? ObjectToFloat(this object obj)
        {
            return obj?.ToString().ToFloat();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float ObjectToFloat(this object obj, float defValue)
        {
            return obj.ObjectToFloat() ?? defValue;
        }

        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Double? ObjectToDouble(this object obj)
        {
            return obj?.ToString().ToDouble();
        }

        /// <summary>
        /// 字符串转浮点数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Double ObjectToDouble(this object obj, double defValue)
        {
            return obj.ObjectToDouble() ?? defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Decimal? ObjectToDecimal(this object obj)
        {
            return obj?.ToString().ToDecimal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Decimal ObjectToDecimal(this object obj, decimal defValue)
        {
            return obj.ObjectToDecimal() ?? defValue;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjectToDateTime(this object obj)
        {
            return obj?.ToString().ToDateTime();
        }

        /// <summary>
        /// 转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(this object obj, DateTime defValue)
        {
            return obj.ObjectToDateTime() ?? defValue;
        }

        /// <summary>
        /// 字符串转GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid? ObjectToGuid(this object obj)
        {
            return obj?.ToString().ToGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Guid ObjectToGuid(this object obj, Guid defValue)
        {
            return obj.ObjectToGuid() ?? defValue;
        }

        /// <summary>
        /// 将对象属性名和属性值转为字典,属性类型不支持复杂类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(object value)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ZTReflector reflector = ZTReflector.Cache(value.GetType(), false);
            object val;
            foreach (var item in reflector.Properties)
            {
                val = item.GetValue(value);
                dic.Add(item.Name, val==null?null:val.ToString());
            }

            return dic;
        }
    }
}
