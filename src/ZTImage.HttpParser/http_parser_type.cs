using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    /// <summary>
    /// 解析类型
    /// </summary>
    public enum http_parser_type
    {
        HTTP_REQUEST,
        HTTP_RESPONSE,
        HTTP_BOTH
    }
}
