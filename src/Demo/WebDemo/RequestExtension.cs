using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using ZTImage.Text;
using System.IO;
using ZTImage.Reflection;
using ZTImage.Reflection.Reflector;
using Microsoft.Extensions.Primitives;
using ZTImage;

namespace WebDemo
{
    public static class RequestExtension
    {
        #region cookie
        /// <summary>
        /// 得到Cookie,键值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetCookieString(this HttpRequest request,string key)
        {
            string val = string.Empty;
            if (request.Cookies.TryGetValue(key, out val))
            {
                return val;
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到Cookie,键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetCookieInt(this HttpRequest request, string key, int _default)
        {
            string str = string.Empty;
            if (!request.Cookies.TryGetValue(key, out str))
            {
                return _default;
            }

            int i = _default;
            if (!Int32.TryParse(str, out i))
            {
                return _default;
            }
            return i;
        }
        #endregion
        
        #region server var

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost(this HttpRequest request)
        {
            return request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet(this HttpRequest request)
        {
            return request.Method.Equals("GET",StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP(this HttpRequest request)
        {
            string result = request.Headers["REMOTE_ADDR"].FirstOrDefault();
            if (string.IsNullOrEmpty(result))
                result = request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                result = request.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            if (string.IsNullOrEmpty(result) || !Valid.IsIP(result))
                return "127.0.0.1";

            return result;
        }


        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount(this HttpRequest request)
        {
            return request.Form.Count + request.Query.Count;
        }
        #endregion
        
        #region query string

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(this HttpRequest request,string strName)
        {
            return GetQueryString(request,strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(this HttpRequest request,string strName, bool sqlSafeCheck)
        {
            if (_GetQueryString(request,strName) == null)
                return "";

            if (sqlSafeCheck && !Valid.IsSafeSqlString(_GetQueryString(request,strName)))
                return "unsafe string";
            return _GetQueryString(request,strName);
        }


        /// <summary>
        /// 查询字符串中得到枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TEnum GetQueryEnum<TEnum>(this HttpRequest request, string strName, TEnum defaultValue) where TEnum : struct
        {
            return TypeConverter.StringToEnum<TEnum>(_GetQueryString(request,strName), defaultValue);
        }

        /// <summary>
        /// 以时间的格式得到Query中指定的键
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetQueryDate(this HttpRequest request, string strName, DateTime defaultValue)
        {
            return TypeConverter.StringToDate(_GetQueryString(request,strName), defaultValue);
        }


        /// <summary>
        /// 以时间的格式得到Query中指定的键
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetQueryNullableDate(this HttpRequest request, string strName)
        {
           
            return TypeConverter.StringToNullableDate(_GetQueryString(request,strName));
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(this HttpRequest request, string strName)
        {
            return ZTImage.TypeConverter.StringToInt(_GetQueryString(request, strName), 0);
        }





        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(this HttpRequest request, string strName, int defValue)
        {
            return ZTImage.TypeConverter.StringToInt(_GetQueryString(request, strName), defValue);
        }

        /// <summary>
        /// 获得指定表单参数的byte类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static byte GetQueryByte(this HttpRequest request, string strName, byte defValue)
        {
            return ZTImage.TypeConverter.StringToByte(_GetQueryString(request, strName), defValue);
        }

        /// <summary>
        /// 获得指定Url参数的long类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static long GetQueryLong(this HttpRequest request, string strName)
        {
            return ZTImage.TypeConverter.StringToLong(_GetQueryString(request, strName), 0);
        }


        /// <summary>
        /// 获得指定Url参数的long类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static long GetQueryLong(this HttpRequest request, string strName, long defaultValue)
        {
            return ZTImage.TypeConverter.StringToLong(_GetQueryString(request, strName), defaultValue);
        }


        public static bool GetQueryStringBoolean(this HttpRequest request, string strName, bool defValue)
        {
            return ZTImage.TypeConverter.StringToBool(_GetQueryString(request, strName), defValue);
        }

        public static bool GetQueryIntBoolean(this HttpRequest request, string strName, bool defValue)
        {
            int v = GetQueryInt(request,strName, 0);
            if (v == 0)
            {
                return defValue;
            }
            return v > 0 ? true : false;
        }
        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(this HttpRequest request, string strName, float defValue)
        {
            return TypeConverter.StringToFloat(_GetQueryString(request, strName), defValue);
        }


        public static double GetQueryDouble(this HttpRequest request, string strName, double defValue)
        {
            return TypeConverter.StringToDouble(_GetQueryString(request, strName), defValue);
        }


        public static decimal GetQueryDecimal(this HttpRequest request, string strName, decimal defValue)
        {
            return TypeConverter.StringToDecimal(_GetQueryString(request,strName), defValue);
        }

        /// <summary>
        /// 得到Query中指定KEY参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string _GetQueryString(HttpRequest request,string key)
        {
            StringValues _val = string.Empty;
            if (request.Query.TryGetValue(key, out _val))
            {
                return String.Join(",",_val);
            }
            return string.Empty;
        }
        #endregion

        #region Form

        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static bool SaveRequestFile(this HttpRequest request, string path)
        {
            try
            {
                if (request.Form.Files.Count > 0)
                {
                    using (Stream stream = request.Form.Files[0].OpenReadStream())
                    {
                        using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            stream.CopyTo(file);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 得到请求的数据
        /// </summary>
        /// <returns></returns>
        public static string GetPostData(this HttpRequest request)
        {
            string postdata = string.Empty;
            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                    {
                        postdata = reader.ReadToEnd();
                    }
                }
                catch(Exception ex)
                {
                    ZTImage.Log.Trace.Error(ex.ToString());
                    postdata = string.Empty;
                }
            }
            return postdata;

            //if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (request.Body == null || request.Body.Length <= 0)
            //    {
            //        return string.Empty;
            //    }
            //    byte[] data = new byte[request.Body.Length];
            //    request.Body.Read(data, 0, data.Length);
            //    //if (stream.CanSeek)
            //    //{
            //    //    stream.Seek(0, SeekOrigin.Begin);
            //    //}
            //    request.Body.Close();
            //    return System.Text.Encoding.UTF8.GetString(data);
            //}
            //return string.Empty;
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(this HttpRequest request, string strName, float defValue)
        {
            return ZTImage.TypeConverter.StringToFloat(_GetFormString(request, strName), defValue);
        }


        public static double GetFormDouble(this HttpRequest request, string strName, double defValue)
        {
            return ZTImage.TypeConverter.StringToDouble(_GetFormString(request, strName), defValue);
        }



        public static decimal GetFormDecimal(this HttpRequest request, string strName, decimal defValue)
        {
            return ZTImage.TypeConverter.StringToDecimal(_GetFormString(request, strName), defValue);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static bool GetFormIntBoolean(this HttpRequest request, string strName, bool defValue)
        {
            int v = GetFormInt(request,strName, 0);
            if (v == 0)
            {
                return defValue;
            }
            return v > 0 ? true : false;
        }

        /// <summary>
        /// 提交表单中得到枚举 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TEnum GetFormEnum<TEnum>(this HttpRequest request, string strName, TEnum defaultValue) where TEnum : struct
        {
            return ZTImage.TypeConverter.StringToEnum<TEnum>(_GetFormString(request, strName), defaultValue);
        }
        
        /// <summary>
        /// 以时间的格式得到Form中指定的键
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetFormDate(this HttpRequest request, string strName, DateTime defaultValue)
        {
            return TypeConverter.StringToDate(_GetFormString(request, strName), defaultValue);
        }


        /// <summary>
        /// 以时间的格式得到Form中指定的键
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetFormNullableDate(this HttpRequest request, string strName)
        {
            return TypeConverter.StringToNullableDate(_GetFormString(request, strName));
        }


        /// <summary>
        /// 得到post提交的long类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetFormLong(this HttpRequest request, string strName, long defaultValue)
        {
            return ZTImage.TypeConverter.StringToLong(_GetFormString(request, strName), defaultValue);
        }






        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(this HttpRequest request, string strName, int defValue)
        {
            return ZTImage.TypeConverter.StringToInt(_GetFormString(request, strName), defValue);
        }

        /// <summary>
        /// 获得指定表单参数的byte类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static byte GetFormByte(this HttpRequest request, string strName, byte defValue)
        {
            return ZTImage.TypeConverter.StringToByte(_GetFormString(request, strName), defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static bool GetFormStringBoolean(this HttpRequest request, string strName, bool defValue)
        {
            return ZTImage.TypeConverter.StringToBool(_GetFormString(request, strName), defValue);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(this HttpRequest request, string strName)
        {
            return _GetFormString(request, strName);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(this HttpRequest request,string strName, bool sqlSafeCheck)
        {
            string _val = _GetFormString(request, strName);

            if (sqlSafeCheck && !Valid.IsSafeSqlString(_val))
                return "unsafe string";

            return _val;
        }

        /// <summary>
        /// 得到Query中指定KEY参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string _GetFormString(HttpRequest request, string key)
        {
            if (!request.Method.Equals( "POST"))
            {
                return string.Empty;
            }
            StringValues _val = string.Empty;
            if (request.Form.TryGetValue(key, out _val))
            {
                return String.Join(",", _val);
            }
            return string.Empty;
        }
        #endregion

        #region Form & QueryString
        
        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(this HttpRequest request,string strName, float defValue)
        {
            if (GetQueryFloat(request,strName, defValue) == defValue)
                return GetFormFloat(request,strName, defValue);
            else
                return GetQueryFloat(request,strName, defValue);
        }

        public static double GetDouble(this HttpRequest request, string strName, double defValue)
        {
            if (GetQueryDouble(request, strName, defValue) == defValue)
                return GetFormDouble(request, strName, defValue);
            else
                return GetQueryDouble(request, strName, defValue);
        }


        public static decimal GetDecimal(this HttpRequest request, string strName, decimal defValue)
        {
            if (GetQueryDecimal(request, strName, defValue) == defValue)
                return GetFormDecimal(request, strName, defValue);
            else
                return GetQueryDecimal(request, strName, defValue);
        }

        public static bool GetIntBoolean(this HttpRequest request, string strName, bool defValue)
        {
            int v = GetInt(request, strName, 0);
            if (v == 0)
            {
                return defValue;
            }
            return v > 0 ? true : false;
        }

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(this HttpRequest request, string strName, int defValue)
        {
            if (GetQueryInt(request, strName, defValue) == defValue)
                return GetFormInt(request, strName, defValue);
            else
                return GetQueryInt(request, strName, defValue);
        }


        /// <summary>
        /// 获得指定Url或表单参数的Byte类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static byte GetByte(this HttpRequest request, string strName, byte defValue)
        {
            if (GetQueryByte(request, strName, defValue) == defValue)
                return GetFormByte(request, strName, defValue);
            else
                return GetQueryByte(request, strName, defValue);
        }

        /// <summary>
        /// 得到提交时间
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetNullableDate(this HttpRequest request, string strName)
        {
            if (GetQueryNullableDate(request, strName).HasValue)
            {
                return GetQueryNullableDate(request, strName);
            }
            else
            {
                return GetFormNullableDate(request, strName);
            }
        }




        public static bool GetStringBoolean(this HttpRequest request, string strName, bool defValue)
        {
            bool v = GetFormStringBoolean(request, strName, defValue);
            if (v == defValue)
            {
                return GetQueryStringBoolean(request, strName, defValue);
            }
            return v;
        }
        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(this HttpRequest request, string strName)
        {
            return GetString(request, strName, false);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(this HttpRequest request, string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(request, strName)))
                return GetFormString(request, strName, sqlSafeCheck);
            else
                return GetQueryString(request, strName, sqlSafeCheck);
        }

        /// <summary>
        /// 得到请求中Guid
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static Guid? GetGuid(this HttpRequest request, string strName)
        {
            string guid = GetString(request, strName);
            return ZTImage.TypeConverter.StringToGuid(guid);
        }

        /// <summary>
        /// 得到请求中strName的值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetLong(this HttpRequest request, string strName, long defaultValue)
        {
            if (GetQueryLong(request, strName, defaultValue) != defaultValue)
            {
                return GetQueryLong(request, strName, defaultValue);
            }
            else
            {
                return GetFormLong(request, strName, defaultValue);
            }
        }


        /// <summary>
        /// 客户端 发送的参数中得到枚举 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TEnum GetEnum<TEnum>(this HttpRequest request, string strName, TEnum defaultValue) where TEnum : struct
        {
            TEnum temp = GetQueryEnum<TEnum>(request, strName, defaultValue);

            if (!temp.Equals(defaultValue))
            {
                return temp;
            }
            else
            {
                return GetFormEnum(request, strName, defaultValue);
            }
        }
        /// <summary>
        /// 得到提交时间
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDate(this HttpRequest request, string strName, DateTime defaultValue)
        {
            if (GetQueryDate(request,strName, defaultValue) != defaultValue)
            {
                return GetQueryDate(request, strName, defaultValue);
            }
            else
            {
                return GetFormDate(request, strName, defaultValue);
            }
        }


        #endregion
        
        #region Inflate Object & 填充对象

        /// <summary>
        /// 用Request Form填充对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T InflateObjectBYForm<T>(this HttpRequest request) where T : class, new()
        {
            KubiuReflector reflector = KubiuReflector.Cache(typeof(T), false);
            
            T model = reflector.NewObject() as T;
            foreach (var property in reflector.Properties)
            {
                StringValues val = string.Empty;
                if (request.Form.TryGetValue(property.Name,out val))
                {
                    SetValue(model, property,  string.Join(",",val));
                }
            }

            return model;
        }

        /// <summary>
        /// 用Request QueryString填充对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T InflateObjectBYQueryString<T>(this HttpRequest request) where T : class, new()
        {
            KubiuReflector reflector = KubiuReflector.Cache(typeof(T), false);

            T model = reflector.NewObject() as T;
            foreach (var property in reflector.Properties)
            {
                StringValues val = string.Empty;
                if (request.Query.TryGetValue(property.Name, out val))
                {
                    SetValue(model, property, string.Join(",", val));
                }
            }

            return model;
        }

        /// <summary>
        /// Form转移至SortedDictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> InflateDicBYForm(this HttpRequest request)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            foreach (var item in request.Form)
            {
                dic.Add(item.Key, string.Join(",",item.Value));
            }
            return dic;
        }

        /// <summary>
        /// QueryString转移至SortedDictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> InflateDicBYQueryString(this HttpRequest request)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            foreach (var item in request.Query)
            {
                dic.Add(item.Key, string.Join(",", item.Value));
            }
            return dic;
        }
        

        private static void SetValue(object model, ObjectProperty property, string value)
        {
            switch (property.MemberType.Name)
            {
                case "Boolean":
                    ToBoolean(model, property, value);
                    break;
                case "Char":
                    ToChar(model, property, value);
                    break;
                case "SByte":
                    ToSByte(model, property, value);
                    break;
                case "Byte":
                    ToByte(model, property, value);
                    break;
                case "Int16":
                    ToInt16(model, property, value);
                    break;
                case "UInt16":
                    ToUInt16(model, property, value);
                    break;
                case "Int32":
                    ToInt32(model, property, value);
                    break;
                case "UInt32":
                    ToUInt32(model, property, value);
                    break;
                case "Int64":
                    ToInt64(model, property, value);
                    break;
                case "UInt64":
                    ToUInt64(model, property, value);
                    break;
                case "Single":
                    ToSingle(model, property, value);
                    break;
                case "Double":
                    ToDouble(model, property, value);
                    break;
                case "Decimal":
                    ToDecimal(model, property, value);
                    break;
                case "DateTime":
                    ToDateTime(model, property, value);
                    break;
                case "Guid":
                    ToGuid(model, property, value);
                    break;
                default:
                    ToDefault(model, property, value);
                    break;
            }
        }


        private static void ToDefault(object model, ObjectProperty property, string val)
        {
            property.TrySetValue(model, val);
        }

        private static void ToBoolean(object model, ObjectProperty property, string val)
        {
            Boolean setter = false;
            if (!Boolean.TryParse(val, out setter))
            {
                int i = 0;
                if (Int32.TryParse(val, out i))
                {
                    if (i > 0)
                    {
                        setter = true;
                    }
                }
            }
            property.TrySetValue(model, setter);
        }

        private static void ToGuid(object model, ObjectProperty property, string val)
        {
            Guid setter;
            if (Guid.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }

        }

        private static void ToChar(object model, ObjectProperty property, string val)
        {
            Char setter;
            if (Char.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSByte(object model, ObjectProperty property, string val)
        {
            SByte setter;
            if (SByte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToByte(object model, ObjectProperty property, string val)
        {
            Byte setter;
            if (Byte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt16(object model, ObjectProperty property, string val)
        {
            Int16 setter;
            if (Int16.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt16(object model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt32(object model, ObjectProperty property, string val)
        {
            Int32 setter;
            if (Int32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt32(object model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt64(object model, ObjectProperty property, string val)
        {
            Int64 setter;
            if (Int64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt64(object model, ObjectProperty property, string val)
        {
            UInt64 setter;
            if (UInt64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSingle(object model, ObjectProperty property, string val)
        {
            Single setter;
            if (Single.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDouble(object model, ObjectProperty property, string val)
        {
            Double setter;
            if (Double.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDecimal(object model, ObjectProperty property, string val)
        {
            Decimal setter;
            if (Decimal.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDateTime(object model, ObjectProperty property, string val)
        {
            DateTime setter;
            if (DateTime.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }
        #endregion
    }
}
