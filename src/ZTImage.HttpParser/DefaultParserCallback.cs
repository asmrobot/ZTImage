using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class DefaultParserCallback : IParserCallback
    {
        public unsafe int on_body(HttpFrame frame, byte* data)
        {
            Console.WriteLine("on_body");
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

        public unsafe int on_header_field(HttpFrame frame, byte* data)
        {
            Console.WriteLine("on_header_field");
            return 0;
        }

        public unsafe int on_header_value(HttpFrame frame, byte* data)
        {
            Console.WriteLine("on_header_value");
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
        

        public unsafe int on_status(HttpFrame frame, byte* data)
        {
            Console.WriteLine("on_status");
            return 0;
        }
        

        public unsafe int on_url(HttpFrame frame, byte* data)
        {
            Console.WriteLine("on_url");
            return 0;
        }
    }
}
