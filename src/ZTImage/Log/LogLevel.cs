using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Log
{
    /// <summary>
    /// DEBUG->INFO->ERROR
    /// </summary>
    public enum LogLevel:Int32
    {
        DEBUG=1,
        INFO=2,
        ERROR=3
    }
}
