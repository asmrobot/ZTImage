using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Net
{
    public class HttpResult
    {
        public HttpResult(HttpWebResponse response)
        {
            this.Response = response;
            this.StatusCode = response.StatusCode;
            StatusNumber = (Int32)StatusCode;            
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode StatusCode 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 状态码值
        /// </summary>
        public int StatusNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// 回应
        /// </summary>
        public HttpWebResponse Response { get; set; }

        private Stream _responseStream = null;
        /// <summary>
        /// 返回流
        /// </summary>
        public Stream ResponseStream 
        {
            get
            {
                if (_responseStream == null)
                {
                    _responseStream = this.Response.GetResponseStream();
                }
                return _responseStream;
            }
        }



        private string _content=null;
        /// <summary>
        /// 读取内容
        /// </summary>
        public async Task<string> ReadContentAsync()
        {
            if (_content == null)
            {
                StreamReader reader = new StreamReader(this.ResponseStream);
                _content = await reader.ReadToEndAsync();
            }
            return _content;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <returns></returns>
        public string ReadContent()
        {
            if (_content == null)
            {
                StreamReader reader = new StreamReader(this.ResponseStream);
                _content = reader.ReadToEnd();
            }
            return _content;
        }










    }
}
