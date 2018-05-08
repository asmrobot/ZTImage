﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace ZTImage
{
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
        public static string SyncGet(string url, Dictionary<string, string> headers = null)
        {
            Stream content = SyncRequestStream(RequestMethod.GET, url, null,headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }

        public async static Task<string> Get(string url, Dictionary<string, string> headers = null)
        {
            Stream content = await RequestStream(RequestMethod.GET,url, null,headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static string SyncGet(string url, string data, Dictionary<string, string> headers = null)
        {
            Stream content = SyncGetStream(url, data,headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }


        public async static Task<string> Get(string url, string data, Dictionary<string, string> headers = null)
        {
            Stream content = await GetStream(url, data,headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static Stream SyncGetStream(string url, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.GET, url, null,headers);
        }

        public async static Task<Stream> GetStream(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStream(RequestMethod.GET,url, null,headers);
        }

        public static Stream SyncGetStream(string url, string data, Dictionary<string, string> headers = null)
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
            return SyncRequestStream(RequestMethod.GET, url, null, headers);
        }

        public async static Task<Stream> GetStream(string url, string data, Dictionary<string, string> headers = null)
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

            return await RequestStream(RequestMethod.GET, url, null, headers);
        }
        #endregion
        
        #region Post

        public static String SyncPost(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = SyncRequestStream(RequestMethod.POST,url, null,headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public async static Task<string> Post(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStream(RequestMethod.POST, url, null,headers);
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
            Stream stream = SyncRequestStream(RequestMethod.POST, url, bdata,headers);
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
            Stream stream = await RequestStream(RequestMethod.POST, url, bdata,headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPost(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = SyncRequestStream(RequestMethod.POST, url, data,headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Post(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream= await RequestStream(RequestMethod.POST, url, data,headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static Stream SyncPostStream(string url, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.POST, url, null,headers);
        }

        public async static Task<Stream> PostStream(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStream(RequestMethod.POST, url, null,headers);
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


            return SyncRequestStream(RequestMethod.POST, url, bdata,headers);
        }

        public async static Task<Stream> PostStream(string url, string data,System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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


            return await RequestStream(RequestMethod.POST, url,bdata,headers);
        }

        public static Stream SyncPostStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.POST, url, data, headers);
        }


        public async static Task<Stream> PostStream(string url, byte[] data, Dictionary<string, string> headers=null)
        {
            return await RequestStream(RequestMethod.POST, url, data, headers);
        }

        #endregion

        #region PUT

        public static String SyncPut(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = SyncRequestStream(RequestMethod.PUT,url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public async static Task<string> Put(string url, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStream(RequestMethod.PUT,url, null, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPut(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = SyncRequestStream(RequestMethod.PUT,url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Put(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            Stream stream = await RequestStream(RequestMethod.PUT,url, bdata, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPut(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = SyncRequestStream(RequestMethod.PUT,url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Put(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            Stream stream = await RequestStream(RequestMethod.PUT,url, data, headers);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static Stream SyncPutStream(string url, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.PUT,url, null, headers);
        }

        public async static Task<Stream> PutStream(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStream(RequestMethod.PUT,url, null, headers);
        }

        public static Stream SyncPutStream(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            
            return SyncRequestStream(RequestMethod.PUT,url, bdata, headers);
        }

        public async static Task<Stream> PutStream(string url, string data, System.Text.Encoding encoding, Dictionary<string, string> headers = null)
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
            
            return await RequestStream(RequestMethod.PUT,url, bdata, headers);
        }

        public static Stream SyncPutStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.PUT, url, data, headers);
        }


        public async static Task<Stream> PutStream(string url, byte[] data, Dictionary<string, string> headers = null)
        {
            return await RequestStream(RequestMethod.PUT, url, data, headers);
        }

        #endregion


        #region DELETE
        public static string SyncDelete(string url, Dictionary<string, string> headers = null)
        {
            Stream content = SyncRequestStream(RequestMethod.DELETE, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }

        public async static Task<string> Delete(string url, Dictionary<string, string> headers = null)
        {
            Stream content = await RequestStream(RequestMethod.DELETE, url, null, headers);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }
        
        public static Stream SyncDeleteStream(string url, Dictionary<string, string> headers = null)
        {
            return SyncRequestStream(RequestMethod.DELETE, url, null, headers);
        }

        public async static Task<Stream> DeleteStream(string url, Dictionary<string, string> headers = null)
        {
            return await RequestStream(RequestMethod.DELETE, url, null, headers);
        }

        public static Stream SyncDeleteStream(string url, string data, Dictionary<string, string> headers = null)
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
            return SyncRequestStream(RequestMethod.DELETE, url, null, headers);
        }

        public async static Task<Stream> DeleteStream(string url, string data, Dictionary<string, string> headers = null)
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

            return await RequestStream(RequestMethod.DELETE, url, null, headers);
        }
        #endregion


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
                request.Headers.Add(item.Key, item.Value);
            }
        }


        private static Stream SyncRequestStream(RequestMethod method,string url, byte[] data, Dictionary<string, string> headers = null)
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
            catch
            {
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
        private async static Task<Stream> RequestStream(RequestMethod method,string url, byte[] data, Dictionary<string, string> headers = null)
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


                WebResponse wrep = await request.GetResponseAsync();

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
            catch
            {
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
