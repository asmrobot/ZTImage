using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public struct field_data
    {
        public UInt16 off;
        public UInt16 len;
    }
    public struct http_parser_url
    {
        public UInt16 field_set;
        public UInt16 port;

        public field_data[] field_data;


    }
}
