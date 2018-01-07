using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    /// <summary>
    /// for ParserEngine.flags field
    /// </summary>
    public enum flags:byte
    {
        F_CHUNKED = 1 << 0
      , F_CONNECTION_KEEP_ALIVE = 1 << 1
      , F_CONNECTION_CLOSE = 1 << 2
      , F_CONNECTION_UPGRADE = 1 << 3
      , F_TRAILING = 1 << 4
      , F_UPGRADE = 1 << 5
      , F_SKIPBODY = 1 << 6
      , F_CONTENTLENGTH = 1 << 7
    }
}
