using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class EmptyParserCallback : IParserCallback
    {
        public readonly static EmptyParserCallback Define = new EmptyParserCallback();

        public unsafe int on_body(HttpFrame frame, ArraySegment<byte> datas)
        {
            return 0;
        }

        public int on_chunk_complete(HttpFrame frame)
        {
            return 0;
        }

        public int on_chunk_header(HttpFrame frame)
        {
            return 0;
        }

        public int on_headers_complete(HttpFrame frame)
        {
            return 0;
        }

        public unsafe int on_header_field(HttpFrame frame, ArraySegment<byte> datas)
        {
            return 0;
        }

        public unsafe int on_header_value(HttpFrame frame, ArraySegment<byte> datas)
        {
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
        

        public unsafe int on_status(HttpFrame frame, Int32 statusCode,  ArraySegment<byte> datas)
        {
            return 0;
        }
        

        public unsafe int on_uri(HttpFrame frame, ArraySegment<byte> datas)
        {
            return 0;
        }
        
    }
}
