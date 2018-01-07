using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public enum http_parser_url_fields
    {
        UF_SCHEMA = 0
      , UF_HOST = 1
      , UF_PORT = 2
      , UF_PATH = 3
      , UF_QUERY = 4
      , UF_FRAGMENT = 5
      , UF_USERINFO = 6
      , UF_MAX = 7
    }
}
