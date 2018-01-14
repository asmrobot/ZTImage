using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public enum ContentEncoding:byte
    {
        None,
        GZip,
        DEFLATE,
        Other
    }
}
