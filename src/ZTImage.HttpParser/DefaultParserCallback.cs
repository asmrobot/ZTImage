using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class DefaultParserCallback : IParserCallback
    {
        public unsafe int on_body(HttpFrame frame, ArraySegment<byte> datas)
        {
            string str = Encoding.ASCII.GetString(datas.Array, datas.Offset, datas.Count);
            Console.WriteLine("on_body,body:{0}|" , str);
            return 0;
        }

        public int on_chunk_complete(HttpFrame frame)
        {
            Console.WriteLine("on_chunk_complete");
            return 0;
        }

        public int on_chunk_header(HttpFrame frame)
        {
            Console.WriteLine("on_chunk_header");
            return 0;
        }

        public int on_headers_complete(HttpFrame frame)
        {
            Console.WriteLine("on_headers_complete");
            return 0;
        }

        public unsafe int on_header_field(HttpFrame frame, ArraySegment<byte> datas)
        {
            string field = Encoding.ASCII.GetString(datas.Array, datas.Offset, datas.Count);
            Console.WriteLine("on_header_field,field:{0}|" , field);
            return 0;
        }

        public unsafe int on_header_value(HttpFrame frame, ArraySegment<byte> datas)
        {
            string fieldValue = Encoding.ASCII.GetString(datas.Array, datas.Offset, datas.Count);
            Console.WriteLine("on_header_value:{0}|" , fieldValue);
            return 0;
        }

        public int on_message_begin(HttpFrame frame)
        {
            Console.WriteLine("on_message_begin");
            return 0;
        }

        public int on_message_complete(HttpFrame frame)
        {
            Console.WriteLine("on_message_complete");
            return 0;
        }
        

        public unsafe int on_status(HttpFrame frame, Int32 statusCode,  ArraySegment<byte> datas)
        {
            string statusDescription = Encoding.ASCII.GetString(datas.Array, datas.Offset, datas.Count);
            Console.WriteLine("on_status,statusCode:{0},descript:{1}|",statusCode,statusDescription);
            return 0;
        }
        

        public unsafe int on_uri(HttpFrame frame, ArraySegment<byte> datas)
        {
            string uri = Encoding.ASCII.GetString(datas.Array, datas.Offset, datas.Count);
            Console.WriteLine("on_url,uri:{0}|" , uri);
            return 0;
        }
        
    }
}
