using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ZTImage.HttpParser
{
    public class ZTResponse:ZTHttpFrame
    {
        public ZTResponse()
        {
            this.ContentEncoding = ContentEncoding.None;
        }
        public ContentEncoding ContentEncoding
        {
            get;
            private set;
        }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// 字符编码类型
        /// </summary>
        public string Charset
        {
            get;
            private set;
        }

        public override void AddHeader(string headerKey, string headerValue)
        {
            base.AddHeader(headerKey, headerValue);
            if (headerKey.Equals("Content-Encoding", StringComparison.OrdinalIgnoreCase))
            {
                if (headerValue.Equals("GZIP", StringComparison.OrdinalIgnoreCase))
                {
                    this.ContentEncoding = ContentEncoding.GZip;
                }
                else if (headerValue.Equals("DEFLATE", StringComparison.OrdinalIgnoreCase))
                {
                    this.ContentEncoding = ContentEncoding.DEFLATE;
                }
                else
                {
                    this.ContentEncoding = ContentEncoding.Other;
                }
            }
            else if (headerKey.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
            {
                string[] typeArray = headerValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (typeArray.Length > 0)
                {
                    if (typeArray[0].IndexOf('/') > -1)
                    {
                        this.ContentType = typeArray[0];
                    }
                }

                if (typeArray.Length == 2)
                {
                    this.Charset = typeArray[1];
                }

            }

            
        }


        //public override void AddContent(ArraySegment<byte> chunk)
        //{
        //    if (this.ContentEncoding == ContentEncoding.GZip)
        //    {
        //        chunk = DecodeGzipData(chunk);
        //    }
        //    else if (this.ContentEncoding == ContentEncoding.DEFLATE)
        //    {
        //        chunk = DecodeDeflateData(chunk);
        //    }


        //    base.AddContent(chunk);
        //}


        public override string GetContent()
        {
            Encoding encode = DefaultEncoding;
            if (!string.IsNullOrEmpty(this.Charset))
            {
                try
                {
                    encode = System.Text.Encoding.GetEncoding(this.Charset);
                }
                catch
                {
                    encode = DefaultEncoding;
                }
            }

            byte[] datas = null;
            if (this.ContentEncoding == ContentEncoding.GZip)
            {
                datas = DecodeGzipData();
            }
            else if (this.ContentEncoding == ContentEncoding.DEFLATE)
            {
                datas = DecodeDeflateData();
            }

            if (datas.Length > 0)
            {
                return encode.GetString(datas);
            }
            return string.Empty;
        }


        private byte[] DecodeGzipData()
        {
            MemoryStream targetStream = new MemoryStream();
            byte[] data = new byte[1024];
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < this.mContent.Count; i++)
            {
                ms.Write(this.mContent[i].Array, this.mContent[i].Offset, this.mContent[i].Count);
            }
            
            using (GZipStream stream = new GZipStream(ms, CompressionMode.Decompress))
            {
                Int32 count = 0;
                do
                {
                    count = stream.Read(data, 0, data.Length);
                    targetStream.Write(data, 0, count);
                } while (count > 0);
                return targetStream.ToArray();                
            }
        }

        private byte[] DecodeDeflateData()
        {
            MemoryStream targetStream = new MemoryStream();
            byte[] data = new byte[1024];
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < this.mContent.Count; i++)
            {
                ms.Write(this.mContent[i].Array, this.mContent[i].Offset, this.mContent[i].Count);
            }

            using (DeflateStream stream = new DeflateStream(ms, CompressionMode.Decompress))
            {
                Int32 count = 0;
                do
                {
                    count = stream.Read(data, 0, data.Length);
                    targetStream.Write(data, 0, count);
                } while (count > 0);
                return targetStream.ToArray();
            }
        }
    }
}
