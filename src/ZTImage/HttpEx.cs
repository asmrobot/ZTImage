using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace ZTImage
{
    public class HttpEx
    {
        #region Get
        public static string SyncGet(string url)
        {
            Stream content = SyncGetStream(url, null);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }

        public async static Task<string> Get(string url)
        {
            Stream content = await GetStream(url, null);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static string SyncGet(string url, string data)
        {
            Stream content = SyncGetStream(url, data);

            StreamReader reader = new StreamReader(content);
            return reader.ReadToEnd();
        }


        public async static Task<string> Get(string url, string data)
        {
            Stream content = await GetStream(url, data);

            StreamReader reader = new StreamReader(content);
            return await reader.ReadToEndAsync();
        }

        public static Stream SyncGetStream(string url)
        {
            return SyncGetStream(url, null);
        }

        public async static Task<Stream> GetStream(string url)
        {
            return await GetStream(url, null);
        }

        public static Stream SyncGetStream(string url, string data)
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

            try
            {

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                HttpWebResponse r = response as HttpWebResponse;
                if (r.StatusCode == HttpStatusCode.OK)
                {
                    return r.GetResponseStream();
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

        public async static Task<Stream> GetStream(string url, string data)
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

            try
            {

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "GET";
                WebResponse response = await request.GetResponseAsync();

                HttpWebResponse r = response as HttpWebResponse;
                if (r.StatusCode == HttpStatusCode.OK)
                {
                    return r.GetResponseStream();
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
        #endregion
        
        #region Post

        public static String SyncPost(string url)
        {
            Stream stream = SyncPostStream(url, null);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public async static Task<string> Post(string url)
        {
            Stream stream = await PostStream(url, null);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPost(string url, string data, System.Text.Encoding encoding)
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
            Stream stream = SyncPostStream(url, bdata);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Post(string url, string data, System.Text.Encoding encoding)
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
            Stream stream = await PostStream(url, bdata);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string SyncPost(string url, byte[] data)
        {
            Stream stream = SyncPostStream(url, data);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async static Task<string> Post(string url, byte[] data)
        {
            Stream stream= await PostStream(url, data);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static Stream SyncPostStream(string url)
        {
            return SyncPostStream(url, null);
        }

        public async static Task<Stream> PostStream(string url)
        {
            return await PostStream(url, null);
        }

        public static Stream SyncPostStream(string url, string data, System.Text.Encoding encoding)
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


            return SyncPostStream(url, bdata);
        }

        public async static Task<Stream> PostStream(string url, string data,System.Text.Encoding encoding)
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


            return await PostStream(url,bdata);
        }

        public static Stream SyncPostStream(string url, byte[] data)
        {
            try
            {

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
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


        public async static Task<Stream> PostStream(string url, byte[] data)
        {
            try
            {

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
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

        #endregion
    }
}
