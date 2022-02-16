using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using ZTImage.Exceptions;
using System.Text;

namespace ZTImage.Net
{
    /// <summary>
    /// http request 
    /// the header range format:-----Range:bytes=0-100 
    /// </summary>
    public class HttpEx
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public enum RequestMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }



        /// <summary>
        /// 请求超时时间
        /// </summary>
        public static Int32 RequestTimeout = 15;

        #region Get
        public static HttpResult Get(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return RequestStreamAsync(RequestMethod.GET, url, null, setting, headers).Result;
        }

        public async static Task<HttpResult> GetAsync(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.GET, url, null, setting, headers);
        }

        public static HttpResult Get(string url, string data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (url.IndexOf("?") > 0)
                {
                    url += "&" + data;
                }
                else
                {
                    url += "?" + data;
                }
            }
            return RequestStreamAsync(RequestMethod.GET, url, null, setting, headers).Result;
        }

        public async static Task<HttpResult> GetAsync(string url, string data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {

            if (!string.IsNullOrEmpty(data))
            {
                if (url.IndexOf("?") > 0)
                {
                    url += "&" + data;
                }
                else
                {
                    url += "?" + data;
                }
            }

            return await RequestStreamAsync(RequestMethod.GET, url, null, setting, headers);
        }
        #endregion

        #region Post

        public static HttpResult Post(string url, Dictionary<string, string> headers = null,HttpSetting setting=null)
        {
            return RequestStreamAsync(RequestMethod.POST, url, null, setting, headers).Result;
        }


        public async static Task<HttpResult> PostAsync(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.POST, url, null, setting, headers);
        }

        public static HttpResult Post(string url, string data, Encoding encoding, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            byte[] bdata = null;
            if (!string.IsNullOrEmpty(data))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                bdata = encoding.GetBytes(data);

            }
            return RequestStreamAsync(RequestMethod.POST, url, bdata, setting, headers).Result;
        }

        public async static Task<HttpResult> PostAsync(string url, string data, Encoding encoding, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            byte[] bdata = null;
            if (!string.IsNullOrEmpty(data))
            {
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                bdata = encoding.GetBytes(data);

            }
            return await RequestStreamAsync(RequestMethod.POST, url, bdata, setting, headers);
        }

        public static HttpResult Post(string url, byte[] data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return RequestStreamAsync(RequestMethod.POST, url, data, setting, headers).Result;
        }

        public async static Task<HttpResult> PostAsync(string url, byte[] data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.POST, url, data, setting, headers);
        }
        #endregion

        #region PUT
        public static HttpResult Put(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return RequestStreamAsync(RequestMethod.PUT, url, null, setting, headers).Result;
        }

        public async static Task<HttpResult> PutAsync(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.PUT, url, null, setting, headers);
        }

        public static HttpResult Put(string url, string data, Encoding encoding, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            byte[] bdata = null;
            if (!string.IsNullOrEmpty(data))
            {
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                bdata = encoding.GetBytes(data);

            }

            return RequestStreamAsync(RequestMethod.PUT, url, bdata, setting, headers).Result;
        }

        public async static Task<HttpResult> PutAsync(string url, string data, Encoding encoding, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            byte[] bdata = null;
            if (!string.IsNullOrEmpty(data))
            {
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                bdata = encoding.GetBytes(data);
            }

            return await RequestStreamAsync(RequestMethod.PUT, url, bdata, setting, headers);
        }

        public static HttpResult Put(string url, byte[] data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return RequestStreamAsync(RequestMethod.PUT, url, data, setting, headers).Result;
        }


        public async static Task<HttpResult> PutAsync(string url, byte[] data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.PUT, url, data, setting, headers);
        }

        #endregion

        #region DELETE
        public static HttpResult Delete(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return RequestStreamAsync(RequestMethod.DELETE, url, null, setting, headers).Result;
        }

        public async static Task<HttpResult> DeleteAsync(string url, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            return await RequestStreamAsync(RequestMethod.DELETE, url, null, setting, headers);
        }

        public static HttpResult Delete(string url, string data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (url.IndexOf("?") > 0)
                {
                    url += "&" + data;
                }
                else
                {
                    url += "?" + data;
                }
            }
            return RequestStreamAsync(RequestMethod.DELETE, url, null, setting, headers).Result;
        }

        public async static Task<HttpResult> DeleteAsync(string url, string data, Dictionary<string, string> headers = null, HttpSetting setting = null)
        {

            if (!string.IsNullOrEmpty(data))
            {
                if (url.IndexOf("?") > 0)
                {
                    url += "&" + data;
                }
                else
                {
                    url += "?" + data;
                }
            }

            return await RequestStreamAsync(RequestMethod.DELETE, url, null, setting, headers);
        }
        #endregion



        //Accept	        由 Accept 属性设置。
        //Connection	    由 Connection 属性和 KeepAlive 属性设置。
        //Content-Length	由 ContentLength 属性设置。
        //Content-Type	    由 ContentType 属性设置。
        //Expect	        由 Expect 属性设置。
        //Date	            由 Date 属性设置。
        //If-Modified-Since	由 IfModifiedSince 属性设置。
        //Referer	        由 Referer 属性设置。
        //Transfer-Encoding	由 TransferEncoding 属性设置（SendChunked 属性必须为 true）。
        //User-Agent	    由 UserAgent 属性设置。

        //Host	            由 Host 属性设置。
        //Range	            由 AddRange 方法设置。
        private static Dictionary<string, Action<HttpWebRequest,string>> HeaderConfile = new Dictionary<string, Action<HttpWebRequest, string>>(ZTImage.Text.IgnoreCaseComparer.Default) {
            { "Accept",(request,value)=>{ request.Accept=value; } },
            { "Connection",(request,value)=>{ request.Connection=value; } },
            { "Content-Length",(request,value)=>{
                Int64 val=value.ToInt64(0);
                request.ContentLength=val;
            } },
            { "Content-Type",(request,value)=>{ request.ContentType=value; } },
            { "ContentType",(request,value)=>{ request.ContentType=value; } },
            { "Expect",(request,value)=>{ request.Expect=value; } },
            { "Date",(request,value)=>{
                DateTime dt=value.ToDateTime(DateTime.Now);
                request.Date=dt;
            } },
            { "If-Modified-Since",(request,value)=>{
                DateTime dt=value.ToDateTime(DateTime.Now);
                request.IfModifiedSince=dt;
            } },
            { "Referer",(request,value)=>{ request.Referer=value; } },
            { "Transfer-Encoding",(request,value)=>{ request.TransferEncoding=value; } },
            { "User-Agent",(request,value)=>{ request.UserAgent=value; } },
            { "Host",(request,value)=>{ request.Host=value; } },
            { "Range",(request,value)=>{
                //bytes=0-100
                if(value.Length<9)
                {
                    return;
                }
                if(value[0]!='b'||value[1]!='y'||value[2]!='t'||value[3]!='e'||value[4]!='s'||value[5]!='=')
                {
                    return;
                }
                string[] vals=value.Substring(6).Split(new char[]{ '-'},StringSplitOptions.RemoveEmptyEntries);
                if(vals.Length!=2)
                {
                    return;
                }
                int from=vals[0].ToInt32(0);
                int to=vals[1].ToInt32(0);
                request.AddRange(from,to);
            } },
        };
        
        private static void AddHeaders(HttpWebRequest request, Dictionary<string, string> headers)
        {
            if (request == null)
            {
                return;
            }
            if (headers == null || headers.Count <= 0)
            {
                return;
            }
            if (request.Headers == null)
            {
                request.Headers = new WebHeaderCollection();
            }
            
            foreach (var item in headers)
            {
                if (HeaderConfile.ContainsKey(item.Key))
                {
                    HeaderConfile[item.Key](request, item.Value);
                    continue;
                }

                request.Headers.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 异步创建请求流
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async static Task<HttpResult> RequestStreamAsync(RequestMethod method,string url, byte[] data, HttpSetting setting, Dictionary<string, string> headers = null)
        {
            if (setting == null)
            {
                setting = HttpSetting.Default;
            }
            try
            {
                HttpWebRequest request = CreateRequest(method, url, headers,setting);
                if (data != null && data.Length > 0)
                {
                    Stream requestStream = await request.GetRequestStreamAsync();
                    BinaryWriter writer = new BinaryWriter(requestStream);
                    writer.Write(data);
                    writer.Flush();
                }
                
                WebResponse response = await request.GetResponseAsync();
                HttpWebResponse hwerep = response as HttpWebResponse;

                HttpResult result = new HttpResult(hwerep);
                return result;
            }
            catch (WebException webEx)
            {
                if (webEx.Response == null)
                {
                    throw new HttpException(webEx.Message, webEx);
                }
                WebResponse response = webEx.Response;
                HttpWebResponse hwerep = response as HttpWebResponse;

                HttpResult result = new HttpResult(hwerep);
                return result;
            }
            catch (System.Exception ex)
            {
                if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Simple)
                {
                    ZTImage.Log.Trace.Error("异步网络请求失败", ex);
                }
                else if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Detail)
                {
                    ZTImage.Log.Trace.Error("异步网络请求失败,请求URL:" + url, ex);
                }

                throw new HttpException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 同步创建请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(RequestMethod method,string url,Dictionary<string,string> headers,HttpSetting setting)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            switch (method)
            {
                case RequestMethod.POST:
                    request.Method = "POST";
                    break;
                case RequestMethod.GET:
                    request.Method = "GET";
                    break;
                case RequestMethod.DELETE:
                    request.Method = "DELETE";
                    break;
                case RequestMethod.PUT:
                    request.Method = "PUT";
                    break;
            }
            
            AddHeaders(request, headers);
            request.Timeout = setting.TimeoutMillSecond;
            return request;
        }
        
    }
}
