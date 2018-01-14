using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class ZTParserCallback<T> : IParserCallback where T:ZTHttpFrame
    {
        public int on_body(HttpFrame frame, ArraySegment<byte> body)
        {
            T ztFrame = frame as T;
            if (ztFrame == null)
            {
                return 0;
            }

            if (body.Count > 0)
            {
                

                ztFrame.AddContent(body);
            }
            
            Console.WriteLine("body");
            return 0;
        }

        public int on_chunk_complete(HttpFrame frame)
        {
            Console.WriteLine("chunk complete");
            return 0;
        }

        public int on_chunk_header(HttpFrame frame)
        {
            Console.WriteLine("chunk_header");
            return 0;
        }

        public int on_headers_complete(HttpFrame frame)
        {
            return 0;
        }

        private string LastField = string.Empty;
        public int on_header_field(HttpFrame frame, ArraySegment<byte> fieldName)
        {
            this.LastField = System.Text.Encoding.UTF8.GetString(fieldName.Array, fieldName.Offset, fieldName.Count);
            return 0;
        }

        public int on_header_value(HttpFrame frame, ArraySegment<byte> fieldValue)
        {
            if (!string.IsNullOrWhiteSpace(this.LastField))
            {
                T ztFrame = frame as T;
                if (ztFrame == null)
                {
                    return 0;
                }
                string val = System.Text.Encoding.UTF8.GetString(fieldValue.Array, fieldValue.Offset, fieldValue.Count);
                ztFrame.AddHeader(this.LastField, val);
            }
            return 0;
        }

        public int on_message_begin(HttpFrame frame)
        {
            return 0;
        }

        public int on_message_complete(HttpFrame frame)
        {
            return 0;
        }

        public int on_status(HttpFrame frame, int statusCode, ArraySegment<byte> status)
        {
            return 0;
        }

        public int on_uri(HttpFrame frame, ArraySegment<byte> uri)
        {
            return 0;
        }
    }
}
