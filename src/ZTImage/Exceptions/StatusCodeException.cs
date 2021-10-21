using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Exceptions
{
    public class StatusCodeException:System.Exception
    {
        public HttpStatusCode StatusCode;
        public StatusCodeException(HttpStatusCode statusCode):base("status code:"+statusCode.ToString())
        {
            this.StatusCode = statusCode;
        }
    }
}
