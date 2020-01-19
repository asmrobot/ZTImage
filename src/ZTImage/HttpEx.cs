using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace ZTImage
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

        #region Get
        public static string Get(string url, Dictionary<string, string> headers = null)
        {
            Stream content = RequestStream(RequestMethod.GET, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }

        public async static Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            Stream content = await RequestStreamAsync(RequestMethod.GET, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static string Get(string url, string data, Dictionary<string, string> headers = null)
        {
            Stream content = GetStream(url, data, headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }


        public async static Task<string> GetAsync(string url, string data, Dictionary<string, string> headers = null)
        {
            Stream content = await GetStreamAsync(url, data, headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static Stream GetStream(string url, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.GET, url, null, headers);
        }

        public async static Task<Stream> GetStreamAsync(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.GET, url, null, headers);
        }

        public static Stream GetStream(string url, string data, Dictionary<string, string> headers = null)
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
            return RequestStream(RequestMethod.GET, url, null, headers);
        }

        public async static Task<Stream> GetStreamAsync(string url, string data, Dictionary<string, string> headers = null)
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

            return await RequestStreamAsync(RequestMethod.GET, url, null, headers);
        }
        #endregion

        #region Post

        public static String SyncPost(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = RequestStream(RequestMethod.POST, url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public async static Task<string> Post(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStreamAsync(RequestMethod.POST, url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPost(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = RequestStream(RequestMethod.POST, url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Post(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = await RequestStreamAsync(RequestMethod.POST, url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPost(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = RequestStream(RequestMethod.POST, url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Post(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStreamAsync(RequestMethod.POST, url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static Stream SyncPostStream(string url, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.POST, url, null, headers);
        }

        public async static Task<Stream> PostStream(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.POST, url, null, headers);
        }

        public static Stream SyncPostStream(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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


            return RequestStream(RequestMethod.POST, url, bdata, headers);
        }

        public async static Task<Stream> PostStream(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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


            return await RequestStreamAsync(RequestMethod.POST, url, bdata, headers);
        }

        public static Stream SyncPostStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.POST, url, data, headers);
        }


        public async static Task<Stream> PostStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.POST, url, data, headers);
        }

        #endregion

        #region PUT

        public static String Put(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = RequestStream(RequestMethod.PUT, url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public async static Task<string> PutAsync(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStreamAsync(RequestMethod.PUT, url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string Put(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = RequestStream(RequestMethod.PUT, url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> PutAsync(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = await RequestStreamAsync(RequestMethod.PUT, url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string Put(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = RequestStream(RequestMethod.PUT, url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> PutAsync(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStreamAsync(RequestMethod.PUT, url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static Stream PutStream(string url, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.PUT, url, null, headers);
        }

        public async static Task<Stream> PutStreamAsync(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.PUT, url, null, headers);
        }

        public static Stream PutStream(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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

            return RequestStream(RequestMethod.PUT, url, bdata, headers);
        }

        public async static Task<Stream> PutStreamAsync(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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

            return await RequestStreamAsync(RequestMethod.PUT, url, bdata, headers);
        }

        public static Stream PutStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.PUT, url, data, headers);
        }


        public async static Task<Stream> PutStreamAsync(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.PUT, url, data, headers);
        }

        #endregion


        #region DELETE
        public static string Delete(string url, Dictionary<string, string> headers = null)
        {
            Stream content = RequestStream(RequestMethod.DELETE, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }

        public async static Task<string> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            Stream content = await RequestStreamAsync(RequestMethod.DELETE, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static Stream DeleteStream(string url, Dictionary<string, string> headers = null)
        {
            return RequestStream(RequestMethod.DELETE, url, null, headers);
        }

        public async static Task<Stream> DeleteStreamAsync(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStreamAsync(RequestMethod.DELETE, url, null, headers);
        }

        public static Stream DeleteStream(string url, string data, Dictionary<string, string> headers = null)
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
            return RequestStream(RequestMethod.DELETE, url, null, headers);
        }

        public async static Task<Stream> DeleteStreamAsync(string url, string data, Dictionary<string, string> headers = null)
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

            return await RequestStreamAsync(RequestMethod.DELETE, url, null, headers);
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
                Int64 val=TypeConverter.StringToLong(value,0);
                request.ContentLength=val;
            } },
            { "Content-Type",(request,value)=>{ request.ContentType=value; } },
            { "ContentType",(request,value)=>{ request.ContentType=value; } },
            { "Expect",(request,value)=>{ request.Expect=value; } },
            { "Date",(request,value)=>{
                DateTime dt=TypeConverter.StringToDate(value,DateTime.Now);
                request.Date=dt;
            } },
            { "If-Modified-Since",(request,value)=>{
                DateTime dt=TypeConverter.StringToDate(value,DateTime.Now);
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
                int from=TypeConverter.StringToInt(vals[0],0);
                int to=TypeConverter.StringToInt(vals[1],0);
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


        private static Stream RequestStream(RequestMethod method,string url, byte[] data, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpWebRequest request = CreateRequest(method, url, headers);
                if (data != null && data.Length > 0)
                {
                    Stream requestStream = request.GetRequestStream();
                    BinaryWriter writer = new BinaryWriter(requestStream);
                    writer.Write(data);
                    writer.Flush();
                }

                WebResponse wrep = request.GetResponse();
                HttpWebResponse hwerep = wrep as HttpWebResponse;
                if (hwerep.StatusCode == HttpStatusCode.OK)
                {
                    return hwerep.GetResponseStream();
                }
                else
                {
                    return Stream.Null;
                }
            }
            catch(Exception ex)
            {
                if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Simple)
                {
                    ZTImage.Log.Trace.Error("异步网络请求失败", ex);
                }
                else if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Detail)
                {
                    string d = string.Empty;
                    if (data != null && data.Length > 0)
                    {
                        d = TypeConverter.ByteArrayToString(data);
                    }
                    ZTImage.Log.Trace.Error("异步网络请求失败,请求URL:" + url + ",请求数据:" + d, ex);
                }
                
                return Stream.Null;
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
        private async static Task<Stream> RequestStreamAsync(RequestMethod method,string url, byte[] data, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpWebRequest request = CreateRequest(method, url, headers);
                if (data != null && data.Length > 0)
                {
                    Stream requestStream = await request.GetRequestStreamAsync();
                    BinaryWriter writer = new BinaryWriter(requestStream);
                    writer.Write(data);
                    writer.Flush();
                }


                WebResponse response = await request.GetResponseAsync();

                HttpWebResponse hwerep = response as HttpWebResponse;
                if (hwerep.StatusCode == HttpStatusCode.OK)
                {
                    return hwerep.GetResponseStream();
                }
                else
                {
                    return Stream.Null;
                }
            }
            catch(Exception ex)
            {
                if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Simple)
                {
                    ZTImage.Log.Trace.Error("异步网络请求失败", ex);
                }
                else if (Settings.Global.ExceptionLevel == Settings.ExceptionLevel.Detail)
                {
                    string d = string.Empty;
                    if (data != null && data.Length > 0)
                    {
                        d=TypeConverter.ByteArrayToString(data);
                    }
                    ZTImage.Log.Trace.Error("异步网络请求失败,请求URL:"+url+",请求数据:"+d, ex);
                }
                
                return Stream.Null;
            }
        }

        /// <summary>
        /// 同步创建请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(RequestMethod method,string url,Dictionary<string,string> headers)
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
            return request;
        }
        
    }
}
