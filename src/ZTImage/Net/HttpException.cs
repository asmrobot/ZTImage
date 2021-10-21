using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Net
{
    public class HttpException:Exception
    {
        public HttpException( string message, Exception inner) :base(message,inner)
        {
            
        }
    }
}
