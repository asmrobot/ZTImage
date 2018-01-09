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
    public enum HttpParserType
    {
        HTTP_REQUEST,//请求
        HTTP_RESPONSE,//应答
        HTTP_BOTH
    }
}
